using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using FMAplication.common;
using FMAplication.Core.Params;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Task;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.MobileModels.DailyTasks;
using FMAplication.MobileModels.Products;
using FMAplication.Models.PosmAssign;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;
using FMAplication.Models.Transaction;
using FMAplication.Models.Users;
using FMAplication.Repositories;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.DailyActivities.Interfaces;
using FMAplication.Services.DailyTasks.Interfaces;
using FMAplication.Services.FileUtility.Implementation;
using FMAplication.Services.FileUtility.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using X.PagedList;

namespace FMAplication.Services.DailyActivities.Implementation
{
    public class PosmAssignService : IPosmAssignService
    {
        private readonly IRepository<POSMProduct> _posm;
        private readonly IRepository<CMUser> _cmUser;
        private readonly IFileService _fileService;
        private readonly IRepository<SalesPointStock> _salesPointStock;
        private readonly IRepository<PosmTaskAssign> _posmAssign;
        private readonly IRepository<CmsUserSalesPointMapping> _cmUserSalesPointMapping;
        private readonly IRepository<SalesPoint> _salesPoint;
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly IRepository<DailyPosmTask> _dailyPosmTask;
        private readonly ICommonService _common;


        public PosmAssignService(IRepository<POSMProduct> posm, IRepository<CMUser> cmUser, IFileService fileService, 
            IRepository<SalesPointStock> salesPointStock, IRepository<PosmTaskAssign> posmAssign, 
            IRepository<CmsUserSalesPointMapping> cmUserSalesPointMapping, IRepository<SalesPoint> salesPoint, 
            IRepository<DailyTask> dailyTask, IRepository<DailyPosmTask> dailyPosmTask, 
            ICommonService common)
        {
            _posm = posm;
            _cmUser = cmUser;
            _fileService = fileService;
            _salesPointStock = salesPointStock;
            _posmAssign = posmAssign;
            _cmUserSalesPointMapping = cmUserSalesPointMapping;
            _salesPoint = salesPoint;
            _dailyTask = dailyTask;
            _dailyPosmTask = dailyPosmTask;
            _common = common;
        }
        public async Task<FileData> GetExcelFileForPosmAssign(ExportPosmAssignViewModel model)
        {

            var spStock = _salesPointStock.GetAllActive().Where(x => x.SalesPointId == model.SalesPointId && x.Status == Status.Active
                && x.POSMProduct.IsJTIProduct).Include(x => x.POSMProduct).ToList().MapToModel();
            _common.InsertAvalableSpQuantity(spStock);
            spStock = spStock.Where(x => x.AvailableQuantity > 0).ToList();

            var cmUsers = _cmUserSalesPointMapping.FindAllInclude(x => x.SalesPointId == model.SalesPointId, 
                    x=>x.CmUser)
                    .Where(x=> x.CmUser.Status == Status.Active &&  x.CmUser.UserType == CMUserType.CMR).Select(x => x.CmUser);

            var cmUserIds = cmUsers.Select(x => x.Id).ToList();
            
            var dates = EachDay(model.From, model.To);
            var posmAssigns = _posmAssign.FindAllInclude(x => x.SalesPointId == model.SalesPointId && x.TaskStatus == PosmTaskAssignStatus.Confirmed &&
                                                       cmUserIds.Any(c => c == x.CmUserId) && dates.Contains(x.Date),
                assign => assign.PosmProduct, assign => assign.CmUser).ToList();

            spStock = spStock.Where(x=> posmAssigns.All(p => p.PosmProductId != x.POSMProductId)).ToList();
            
            var salesPoint = _salesPoint.Find(x => x.Id == model.SalesPointId);
            ExcelPackage excel = new ExcelPackage();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            List<string> headers = new List<string>() { "User Name (CODE)", "Date" };
            headers.AddRange(posmAssigns.Select(x=>x.PosmProduct.Name).Distinct().ToList());
            headers.AddRange(spStock.Select(x=>x.POSMProduct.Name).ToList());
            SetTableStyle(workSheet, headers.Count);
            SetHeaderStyle(workSheet, headers.Count);
            InsertHeaders(headers, workSheet);
            InsertRows(model,cmUsers.ToList(), workSheet, headers, posmAssigns);
            _fileService.AutoExcelFitColumns(headers.Count, workSheet);

            FileData fileData = GetFileData(await excel.GetAsByteArrayAsync());
            fileData.Name = $"POSM_Task_Assignment-{model.From.ToBangladeshTime():d}-{model.To.ToBangladeshTime():d}-{salesPoint.Code}";
            excel.Dispose();
            return fileData;
        }


        private void InsertRows(ExportPosmAssignViewModel  model ,List<CMUser> cmUsers, ExcelWorksheet workSheet, List<string> headers, List<PosmTaskAssign> posmTaskAssigns)
        {
            int startingRowNumberForData = 2;
            int currentRowNumberForData = startingRowNumberForData;
            foreach (var day in EachDay(model.From, model.To))
            {
                var dayInBdTime = day.ToBangladeshTime();
                if (model.ExcludeFriday && dayInBdTime.DayOfWeek == DayOfWeek.Friday) continue;
                //var filterByDate = posmTaskAssigns.FirstOrDefault(x => x.Date.Date == day.Date);

                foreach (var t in cmUsers)
                {
                    workSheet.Cells[currentRowNumberForData, 1].Value = $"{t.Name}({t.Code})";
                    workSheet.Cells[currentRowNumberForData, 2].Value = day.ToBangladeshTime().ToString("d");

                    for (int headeIndex = 3; headeIndex <= headers.Count; headeIndex++)
                    {
                        
                        var posmProduct = posmTaskAssigns.FirstOrDefault(x => x.PosmProduct != null &&
                                                                              x.PosmProduct.Name == headers[headeIndex-1] && x.Date.Date == day.Date &&
                                                                              x.CmUser.Code == t.Code); 
                          
                        if (posmProduct != null) workSheet.Cells[currentRowNumberForData, headeIndex].Value = posmProduct.Quantity;
                    }
                    currentRowNumberForData++;
                }
            }
            
        }

        private FileData GetFileData(byte[] bytes)
        {
            string fileName = Guid.NewGuid() + ".xlsx";
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            FileData fileData = new FileData()
            {
                ContentType = contentType,
                Data = bytes,
                Name = fileName
            };
            return fileData;
        }

        private void SetTableStyle(ExcelWorksheet workSheet, int columnCount)
        {
            workSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;
        }

        public void SetHeaderStyle(ExcelWorksheet workSheet, int headCount)
        {
            var header = workSheet.Cells[1, 1, 1, headCount];
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.Font.Bold = true;
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(194, 194, 194));
            header.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            header.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        private static void InsertHeaders(List<string> headers, ExcelWorksheet workSheet)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                workSheet.Cells[1, i + 1].Value = headers[i];
            }
        }
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.BangladeshDateInUtc(); day.Date <= thru.BangladeshDateInUtc(); day = day.AddDays(1))
                yield return day;
        }

        public async Task<(IEnumerable<PosmAssignModel> Data, List<UserDataErrorViewModel> Errors)> ExcelSaveToDatabaseAsync(DataTable datatable)
        {
            var items = new List<PosmTaskAssign>();
            var errorData = new List<UserDataErrorViewModel>();
            List<string> Columns = new List<string>();

            var cmUsers = _cmUser.GetAllActive().Where(x=>x.UserType == CMUserType.CMR).ToList();
            var posmProducts  =  _posm.GetAllActive().Where(x=>x.IsJTIProduct).ToList();
            //int index = 1;
            int salesPointId = 0;

            foreach (DataColumn column in datatable.Columns)
            {
             Columns.Add(column.ColumnName);   
            }

            for (var productIndex = 3; productIndex <= Columns.Count; productIndex++)
            {
                var productName = Columns[productIndex - 1];
                var headerError = new List<UserDataErrorViewModel>();
                await GetProductValidation(productName, 1, headerError);
                errorData.AddRange(headerError);
                foreach (DataRow row in datatable.Rows)
                {
                    //index++;
                    var rowIndex = datatable.Rows.IndexOf(row)+2;
                    var item = new PosmTaskAssign();

                    var user = row[Columns[0]].ObjectToString("NULL");
                    var date = row["Date"].ObjectToString("NULL");
                    var productQuantity = row[Columns[productIndex - 1]].ObjectToString("NULL");
                    if (productQuantity == "") productQuantity = "0";
                    item.Quantity = int.Parse(productQuantity);

                    if (item.Quantity == 0 )
                        continue;

                    var errors = await ValidatePosmAssignData(user, date, productName,item.Quantity, rowIndex);
                    var errorLength = errors.Count + headerError.Count;

                    if (errorLength > 0)
                    {
                        errorData.AddRange(errors);
                        continue;
                    };

                    var userData = getContentBetweenBracketsFrom(user);
                    var cmUser = GetCMUser(userData.Name, userData.code, cmUsers);
                    var cmUserSalesPoint = await _cmUserSalesPointMapping.FindAsync(x => x.CmUserId == cmUser.Id);
                    item.Date = DateTime.Parse(date).AddHours(-6);

                    item.CmUserId = cmUser.Id;
                    item.SalesPointId = cmUserSalesPoint.SalesPointId;
                    salesPointId = item.SalesPointId;
                    var product = GetPOSMProduct(productName, posmProducts);
                    item.PosmProductId = product.Id;

                    items.Add(item);
                }
            }

            if (errorData.Count > 0) return (null, errorData.OrderBy(x=>x.Row).ToList());

            if (items.Count == 0)
                throw new Exception("There is no data to be imported, please fill up the data correctly");

            var posmNewAndExisting = CheckGetItemsStock(items, salesPointId, posmProducts);

            var newItems = posmNewAndExisting.Where(x => x.Id == 0).ToList();
            var existingItems = posmNewAndExisting.Where(x => x.Id > 0).ToList();

            var result = await _posmAssign.CreateListAsync(newItems);
            await PosmTaskAssignToDailyTask(result);
            var existingResult = await _posmAssign.UpdateListAsync(existingItems);
            if (existingResult != null) result.AddRange(existingResult);

            var data = result.ToMap<PosmTaskAssign, PosmAssignModel>();
            return (data, null);
        }

      

        public async Task<Pagination<PosmAssignModel>> GetPosmAssigns(PomsAssignParams searchParams)
        {
            if (searchParams.FromDate > searchParams.ToDate) (searchParams.FromDate, searchParams.ToDate) = (searchParams.ToDate, searchParams.FromDate);

            var cmUsers = (await _cmUser.FindAllAsync(x => x.UserType == CMUserType.CMR)).ToMap<CMUser, CmUserModel>();

            var salesPointIds = _posmAssign.GetAll().Select(x => x.SalesPointId).Distinct().ToList();
            var salesPoints = _salesPoint.GetAll().Where(x => salesPointIds.Contains(x.Id)).ToList();

            var query = _posmAssign.GetAllActive().Where(x =>  x.Date>= searchParams.FromDate&& x.Date<= searchParams.ToDate);


            if (!string.IsNullOrWhiteSpace(searchParams.Search))
            {
                var spIds = _salesPoint.GetAllActive().Where(x => x.Code.Contains(searchParams.Search)).Select(x => x.SalesPointId).ToList();
                query = query.Where(x => spIds.Contains(x.SalesPointId));
            }
            if (searchParams.SalesPointId > 0)
            {
                query = query.Where(x => x.SalesPointId == searchParams.SalesPointId);
            }

            var data = query.GroupBy(x => new {x.CmUserId, x.SalesPointId, x.TaskStatus, x.Date})
                .OrderBy(x=>x.Key.Date)
                .Select(x =>
                    new PosmAssignModel
                    {

                        CmUserId = x.Key.CmUserId,
                        SalesPointId = x.Key.SalesPointId,
                        TaskStatus = x.Key.TaskStatus,
                        Date = x.Key.Date,
                        SumOfQuantity = x.Sum(assign => assign.Quantity),
                        Lines = x.Count()
                    }).ToList();

            _common.InsertSalesPoints(data);

            var result = await data.ToPagedListAsync(searchParams.PageIndex, searchParams.PageSize);
            var finalResult = result.ToList();
            foreach (var d in finalResult)
            {
                d.CmUser = cmUsers.Find(x => x.Id == d.CmUserId);
                d.SalesPoint = salesPoints.Find(x => x.Id == d.SalesPointId).ToMap<SalesPoint, SalesPointModel>();
            }
           
            return new Pagination<PosmAssignModel>(searchParams.PageIndex, searchParams.PageSize, result.TotalItemCount, finalResult);
        }


        public async Task<List<PosmTaskMBModel>> GetPosmTasks(int userId, int salesPointId, DateTime date)
        {
            try
            {
                var posmAssignData = await _posmAssign.GetAll()
                    .Where(x => x.CmUserId == userId && x.SalesPointId == salesPointId && x.Date.Date == date.Date)
                    .GroupBy(x => x.PosmProductId)
                    .Select(x =>
                        new
                        {
                            Quantity = x.Sum(assign => assign.Quantity),
                            productId = x.Key

                        }).ToListAsync();

                var posms = await _posm.GetAllAsync();
                var posmTasks = new List<PosmTaskMBModel>();
                var dailyTask = await GetOrCreateDailyTask(userId, new DailyTaskMBModel { SalesPointId = salesPointId, DateTime = date });

                foreach (var posmAssign in posmAssignData)
                {
                    var product = posms.FirstOrDefault(x => x.Id == posmAssign.productId).ToMap<POSMProduct, POSMProductMBModel>();
                    posmTasks.Add(new PosmTaskMBModel { PosmProduct = product, Quantity = posmAssign.Quantity, SalesPointId = salesPointId, DailyTaskId = dailyTask.Id });
                }

                var dailyTaskIds = posmTasks.Select(x => x.DailyTaskId).ToList();
                var submittedPosmTasks = _dailyPosmTask.GetAllActive().Include(inc => inc.DailyTask)
                    .Where(x => x.DailyTask.IsSubmitted && dailyTaskIds.Contains(x.DailyTaskId)).ToList();

                var submittedDailyTaskIds = submittedPosmTasks.Select(x => x.DailyTaskId).ToList();

                if (submittedDailyTaskIds.Count > 0)
                    posmTasks = posmTasks.Where(x => !submittedDailyTaskIds.Contains(x.DailyTaskId)).ToList();


                return posmTasks;
            }
            catch (Exception e)
            {
                return new List<PosmTaskMBModel>();
            }
           
        }

        private List<PosmTaskAssign> CheckGetItemsStock(List<PosmTaskAssign> items, int salesPointId, List<POSMProduct> posmProducts)
        {
            var productIds = items.Select(x => x.PosmProductId).Distinct().ToList();
            var userIds = items.Select(x => x.CmUserId).Distinct().ToList();
            var dates = items.Select(x => x.Date.Date).ToList();
            var existingPosmAssign = _posmAssign.FindAll(x => x.SalesPointId == salesPointId && x.TaskStatus == PosmTaskAssignStatus.Confirmed &&
                                                              productIds.Contains(x.PosmProductId) &&
                                                              userIds.Contains(x.CmUserId) && dates.Contains(x.Date.Date)).ToList();


            var posmNewAndExisting = new List<PosmTaskAssign>();
            var posmStock = _salesPointStock.FindAll(x => x.SalesPointId == salesPointId &&
                                                          productIds.Contains(x.POSMProductId)).ToList().MapToModel();
            _common.InsertAvalableSpQuantity(posmStock);

            Dictionary<int, int> increasedQuantityOfExsiting = new Dictionary<int, int>();
            existingPosmAssign.Select(x=>x.PosmProductId).Distinct().ToList().ForEach(id => increasedQuantityOfExsiting.Add(id,0));

            foreach (var i in items)
            {
                var exstingItem = existingPosmAssign.FirstOrDefault(x => x.Date == i.Date &&
                                                                         x.PosmProductId == i.PosmProductId &&
                                                                         x.CmUserId == i.CmUserId);

                if (exstingItem != null)
                {
                    var increasedAmount =  i.Quantity - exstingItem.Quantity;
                    if (increasedAmount < 0) increasedAmount = 0;
                    increasedQuantityOfExsiting[i.PosmProductId] += increasedAmount;

                    exstingItem.Quantity = i.Quantity;
                    i.Id = exstingItem.Id;
                    posmNewAndExisting.Add(exstingItem);
                }
                else
                    posmNewAndExisting.Add(i);
            }
            foreach (var productId in productIds)
            {
                var stockQuantity = 0;
                var stock = posmStock.FirstOrDefault(x => x.POSMProductId == productId);
                if (stock is object) stockQuantity = stock.AvailableQuantity;

                var newQuantity = posmNewAndExisting.FindAll(x => x.PosmProductId == productId && x.Id == 0).Sum(x => x.Quantity);
                var existingNewSum = newQuantity;
                if (increasedQuantityOfExsiting.Keys.Contains(productId))
                    existingNewSum += increasedQuantityOfExsiting[productId];

                if (existingNewSum > stockQuantity)
                {
                    var product = posmProducts.FirstOrDefault(x => x.Id == productId);
                    if (product != null)
                        throw new Exception(
                            $"Failed to create posm Assign, Stock not available for product : {product.Code} - {product.Name}. Available quantity-{stockQuantity}");
                }
            }

            return posmNewAndExisting;
        }


        private async Task<List<UserDataErrorViewModel>> ValidatePosmAssignData(string user, string date, string productName, int quantity, int row)
        {
            var errors = new List<UserDataErrorViewModel>();


           await GetUserDataValidation(user, row, errors);
           //await GetProductValidation(productName, row, errors);

           if (quantity < 0)
               errors.Add(new UserDataErrorViewModel { ColumnName = productName, ErrorMessage = "Quantity can't be negative", Row = row });
            if (string.IsNullOrWhiteSpace(date))
                errors.Add(new UserDataErrorViewModel { ColumnName = productName, ErrorMessage = "Date can't be empty", Row = row });


            return errors;
        }

        private async Task GetProductValidation(string productName, int row, List<UserDataErrorViewModel> errors)
        {
            if (String.IsNullOrWhiteSpace(productName))
                errors.Add(new UserDataErrorViewModel
                    {ColumnName = productName, ErrorMessage = "Product Name can't be empty", Row = row});
            else
            {
                var result = await  _posm.Where(x=>x.IsJTIProduct && x.Status == Status.Active).AnyAsync(x => x.Name.Trim().ToLower() == productName.Trim().ToLower());
                if (!result)
                    errors.Add(new UserDataErrorViewModel
                        { ColumnName = productName, ErrorMessage = $"Product \"{productName}\" not found", Row = row });
            }
        }

        private  async Task GetUserDataValidation(string user, int row, List<UserDataErrorViewModel> errors)
        {
            if (string.IsNullOrWhiteSpace(user))
                errors.Add(new UserDataErrorViewModel
                    {ColumnName = "User Name (CODE)", ErrorMessage = "User can't be empty", Row = row});


            else
            {
                var data = getContentBetweenBracketsFrom(user);
                var result = await _cmUser.AnyAsync(x => x.Code == data.code && x.Name.ToLower() == data.Name.Trim().ToLower());
                if (!result)
                    errors.Add(new UserDataErrorViewModel
                        { ColumnName = "User Name (CODE)", ErrorMessage = "User not found", Row = row });
            }
        }


        private (string Name, string code) getContentBetweenBracketsFrom(string data)
        {
            var code = data.Split(new char[] {'(', ')'}).ElementAt(1);
            string name = data.Substring(0, data.IndexOf("(", StringComparison.Ordinal));
            return (name, code);
        }

        private  CMUser GetCMUser(string name, string code, List<CMUser> cmUsers)
        {
            return  cmUsers.FirstOrDefault(x => x.Code.ToLower() == code.ToLower() && x.Name.Trim().ToLower() == name.Trim().ToLower());
        }
        
        private POSMProduct GetPOSMProduct(string name, List<POSMProduct> products)
        {
            return products.FirstOrDefault(x =>  x.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        private async Task<DailyTaskMBModel> GetOrCreateDailyTask(int cmUserId, DailyTaskMBModel model)
        {
            var checkExisting = await _dailyTask.FindAsync(x =>
                x.SalesPointId == model.SalesPointId && x.CmUserId == cmUserId &&
                x.DateTime.Date == model.DateTime.Date);

            if (checkExisting != null)
                return checkExisting.ToMap<DailyTask, DailyTaskMBModel>();

            var dailyTaskModel = new DailyTask();
            dailyTaskModel.SalesPointId = model.SalesPointId;
            dailyTaskModel.CmUserId = cmUserId;
            dailyTaskModel.DateTime = model.DateTime;

            await _dailyTask.CreateAsync(dailyTaskModel);
            var dailyTask = dailyTaskModel.ToMap<DailyTask, DailyTaskMBModel>();

            return dailyTask;
        }

        private async Task PosmTaskAssignToDailyTask(List<PosmTaskAssign> result)
        {
            var data = result.GroupBy(x => new { x.CmUserId, x.SalesPointId, x.TaskStatus, x.Date })
                .OrderBy(x => x.Key.Date)
                .Select(x =>
                    new PosmAssignModel
                    {

                        CmUserId = x.Key.CmUserId,
                        SalesPointId = x.Key.SalesPointId,
                        TaskStatus = x.Key.TaskStatus,
                        Date = x.Key.Date
                    }).ToList();

            foreach (var posmTaskAssign in data)
            {
                await GetOrCreateDailyTask(posmTaskAssign.CmUserId,
                    new DailyTaskMBModel
                    {
                        DateTime = posmTaskAssign.Date,
                        SalesPointId = posmTaskAssign.SalesPointId
                    });
            }
        }

    }
}
