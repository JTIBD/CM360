using fm_application.Models.DailyActivity;
using FMAplication.Controllers.Common;
using FMAplication.Core;
using FMAplication.Extensions;
using FMAplication.Filters;
using FMAplication.Models.Examples;
using FMAplication.Services.DailyActivities.Interfaces;
using FMAplication.Services.Interfaces;
using FMAplication.Services.QuestionDetails.Interfaces;
using FMAplication.Services.Sales.Interfaces;
using FMAplication.Services.Users.Interfaces;
using FMAplication.Models.Questions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Models.DailyActivity;
using FMAplication.Models.Reports;
using FMAplication.RequestModels.Reports;

namespace FMAplication.Controllers
{
    [ApiController]
    //[JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class DailyCMActivityController : BaseController
    {
        private readonly ILogger<DailyCMActivityController> _logger;
        private readonly IUserInfoService _user;
        private readonly ICMUserService _cmUser;
        private readonly IOutletService _outlet;
        private readonly INodeService _node;
        private readonly IDailyCMActivityService _dailyCMActivityService;        

        public DailyCMActivityController(ILogger<DailyCMActivityController> logger,
            IUserInfoService user,
            IOutletService outlet,
            ICMUserService cmUser,
            IDailyCMActivityService dailyCMActivityService,
            INodeService node)
        {
            _logger = logger;
            _user = user;
            _outlet = outlet;
            _cmUser = cmUser;
            _dailyCMActivityService = dailyCMActivityService;
            _node = node;
        }

        #region dropdown Generation
        [HttpGet("GetFMUsers")]
        public async Task<IActionResult> Get()
        {
            try
            {
                //var result = await _user.GetFMUsersAsync(0);
                var result = await _user.GetUsersAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetAllChannel")]
        public async Task<IActionResult> GetAllChannel()
        {
            try
            {
                //var result = await _user.GetFMUsersAsync(0);
                var result = await _node.GetAllChannelAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetOutletByRoute/{id}")]
        public async Task<IActionResult> GetOutletByRoute(int id)
        {
            try
            {
                var result = await _outlet.GetOutletsByRouteId(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [HttpGet("GetOutletByChannel/{channelid}/{salespointid}")]
        public async Task<IActionResult> GetOutletByChannel(int channelid, int salespointid)
        {
            try
            {
                var result = await _outlet.GetOutletsByChannelIdandSalespointId(channelid, salespointid);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetRouteBySalesPoint/{id}")]
        public async Task<IActionResult> GetRouteBySalesPoint(int id)
        {
            try
            {
                var result = await _outlet.GetRoutesBySalesPointId(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetRouteByFM/{id}")]
        public async Task<IActionResult> GetRouteByFM(int id)
        {
            try
            {
                var result = await _outlet.GetRoutesByFMId(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetSalesPointByFM/{id}")]
        public async Task<IActionResult> GetSalesPointByFM(int id)
        {
            try
            {
                var result = await _outlet.GetSalesPointByFMId(id);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        //[HttpGet("GetCMUserByFM/{id}")]
        //public async Task<IActionResult> GetCMUserByFM(int id)
        //{
        //    try
        //    {
        //        var result = await _cmUser.GetCMUserByFMIdAsync(id);
        //        return OkResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        #endregion dropdown endregion

        [HttpGet("GetCMTask/{Id}")]
        public async Task<IActionResult> GetCMTaskById(int id)
        {
            try
            {
                var result = await _dailyCMActivityService.GetCMTaskById(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("GetDailyCMActivitiesByCurrentUser")]
        public async Task<IActionResult> GetDailyCMActivities(int pageIndex,int pageSize,string search)
        {
            try
            {
                var result = await _dailyCMActivityService.GetDailyCMActivitesByCurrentUserAsync(pageIndex, pageSize, search);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
        [JwtAuthorize]
        [HttpGet("GetDailyCMActivitiesByCMUser")]
        public async Task<ActionResult<List<DailyCMActivityModel>>> GetDailyCMActivities()
        {

            try
            {
                var userId = AppIdentity.AppUser.UserId;
                List<DailyCMActivityModel> result = await _dailyCMActivityService.GetDailyCMActivitesByCMUserAsync(userId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [JwtAuthorize]
        [HttpGet("DownloadExcelFormateOfTask")]
        public async Task<FileContentResult> DownloadExcelFormateOfTask()
        {
            try { 
                var fileData = _dailyCMActivityService.DownloadExcelFormatOfTask();
                FileContentResult result = File(fileData.Data, fileData.ContentType);
                return result;
            }catch(Exception e)
            {
                //return BadRequest(e.Message);
                throw new Exception(e.Message);
            }
        }


        [HttpGet("GetCompleteReport")]
        public async Task<IActionResult> GetCompleteRoport()
        {
            try
            {
                var result = await _dailyCMActivityService.GetDailyCMActivitiesReportsByCurrentUserAsync(1,5000);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpGet("GetPOSMReport")]
        public async Task<IActionResult> GetPOSMRoport(int pageIndex,int pageSize,string search)
        {
            try
            {
                var result = await _dailyCMActivityService.GetPOSMReportsByCurrentUserAsync(pageIndex,pageSize,search);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

         [HttpGet("GetAuditReport")]
        public async Task<IActionResult> GetAuditRoport(int pageIndex, int pageSize, string search)
        {
            try
            {
                var result = await _dailyCMActivityService.GetAuditReportsByCurrentUserAsync(pageIndex, pageSize, search);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

         [HttpGet("GetSurveyReport")]
        public async Task<IActionResult> GetSurveyRoport(int pageIndex, int pageSize, string search)
        {
            try
            {
                var result = await _dailyCMActivityService.GetSurveyReportsByCurrentUserAsync(pageIndex, pageSize, search);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        

        [HttpPost("SaveCMTask")]
        public async Task<IActionResult> SaveCMTask([FromBody] List<DailyCMActivityModel> model)
        {
            try
            {
                var result = new List<DailyCMActivityModel>();
                var existsCount = 0;
                model = await _dailyCMActivityService.mapSurveyReportQuestions(model);               
                foreach (var item in model)
                {
                    var res = await _dailyCMActivityService.SaveCMTask(item);

                    if(res.IsExists) 
                    {
                        existsCount++;
                        continue;
                    }

                    result.Add(res.Data);
                }

                var msg = result.Count + " new data saved" + (existsCount > 0 ? " and " + existsCount + " data already exists." : ".");

                return OkResult(result, msg);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("GetFilteredCMTask")]
        public async Task<IActionResult> GetFilteredCMTask([FromBody] SearchDailyCMActivityModel searchModel)
        {
            try
            {
                var result = await _dailyCMActivityService.GetFilteredCMTask(searchModel);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("updateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] DailyCMActivityModel model)
        {
            try
            {
                var result = await _dailyCMActivityService.UpdateStatusAsync(model);               
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpPost("updateBatchStatus")]
        public async Task<IActionResult> UpdateBatchStatus([FromBody] BatchStatusChangeModel model)
        {
            try
            {
                var result = await _dailyCMActivityService.UpdateBatchStatusAsync(model);               
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDCMAReportsInDetails2")]
        public async Task<ActionResult<DCMAReportsInDetailsResponse>> GetDCMAReportsInDetails2(int pageIndex, int pageSize, string search)
        {

                DCMAReportsInDetailsResponse result = await _dailyCMActivityService.GetDCMAReportsInDetails2(pageIndex, pageSize, search);
                return Ok(result);
        }

        [HttpPost("GetExecutionReportOutletWise")]
        public async Task<ActionResult<List<Dictionary<string, dynamic>>>> GetExecutionReportOutletWise(GetExecutionReport payload)
        {

            var result = await _dailyCMActivityService.GetExecutionReportOutletWise(payload);
            return Ok(result);
        }

        [HttpPost("GetExecutionReportSalesPointWise")]
        public async Task<ActionResult<List<Dictionary<string,dynamic>>>> GetExecutionReportsSpWise(GetExecutionReport payload)
        {

            var result = await _dailyCMActivityService.GetExecutionReportsSpWise(payload);
            return Ok(result);
        }

        [HttpPost("GetExecutionReportTeritoryWise")]
        public async Task<ActionResult<List<Dictionary<string, dynamic>>>> GetExecutionReportTeritoryWise(GetExecutionReport payload)
        {
            var result = await _dailyCMActivityService.GetExecutionReportTeritoryWise(payload);
            return Ok(result);
        }

        [HttpPost("GetExecutionReportAreaWise")]
        public async Task<ActionResult<List<Dictionary<string, dynamic>>>> GetExecutionReportAreaWise(GetExecutionReport payload)
        {

            var result = await _dailyCMActivityService.GetExecutionReportAreaWise(payload);
            return Ok(result);
        }

        [HttpPost("GetExecutionReportRegionWise")]
        public async Task<ActionResult<List<Dictionary<string, dynamic>>>> GetExecutionReportRegionWise(GetExecutionReport payload)
        {

            var result = await _dailyCMActivityService.GetExecutionReportRegionWise(payload);
            return Ok(result);
        }

        [HttpPost("GetExecutionReportNational")]
        public async Task<ActionResult<List<Dictionary<string, dynamic>>>> GetExecutionReportNational(GetExecutionReport payload)
        {

            var result = await _dailyCMActivityService.GetExecutionReportNational(payload);
            return Ok(result);
        }


        [HttpGet("GetDCMAReportsInDetails")]
        public async Task<IActionResult> GetDCMAReportsInDetails(int pageIndex, int pageSize, string search,bool isAll)
        {
            try
            {
                var result = await _dailyCMActivityService.GetDCMAReportsInDetailsByCurrentUserAsync(pageIndex, pageSize, search, isAll);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDCMAReportsSalesPointWise")]
        public async Task<IActionResult> GetDCMAReportsSalesPointWise(int pageIndex, int pageSize, string search,bool isAll)
        {
            try
            {
                var result = await _dailyCMActivityService.GetDCMAReportsSalesPointWiseByCurrentUserAsync( pageIndex,  pageSize, search,isAll);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDCMAReportsSalesPointWise2")]
        public async Task<ActionResult<DCMAReportsInDetailsResponse>> GetDCMAReportsSalesPointWise2(int pageIndex, int pageSize, string search)
        {
            DCMAReportsInDetailsResponse result = await _dailyCMActivityService.GetDCMAReportsSalesPointWiseByCurrentUserAsync2(pageIndex, pageSize, search);
            return Ok(result);
        }

        [HttpGet("GetDCMAReportsTerritoryWise")]
        public async Task<IActionResult> GetDCMAReportsTerritorytWise(int pageIndex, int pageSize, string search, bool isAll)
        {
            try
            {
                var result = await _dailyCMActivityService.GetDCMAReportsTerritoryWiseByCurrentUserAsync(pageIndex, pageSize, search,isAll);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDCMAReportsTerritoryWise2")]
        public async Task<ActionResult<DCMAReportsInDetailsResponse>> GetDCMAReportsTerritorytWise2(int pageIndex, int pageSize, string search)
        {

                DCMAReportsInDetailsResponse result = await _dailyCMActivityService.GetDCMAReportsTerritoryWiseByCurrentUserAsync2(pageIndex, pageSize, search);
                return Ok(result);
        }

        [HttpGet("GetDCMAReportsAreaWise")]
        public async Task<IActionResult> GetDCMAReportsAreaWise(int pageIndex, int pageSize, string search, bool isAll)
        {
            try
            {
                var result = await _dailyCMActivityService.GetDCMAReportsAreaWiseByCurrentUserAsync(pageIndex, pageSize, search, isAll);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDCMAReportsAreaWise2")]
        public async Task<ActionResult<DCMAReportsInDetailsResponse>> GetDCMAReportsAreaWise2(int pageIndex, int pageSize, string search)
        {
            DCMAReportsInDetailsResponse result = await _dailyCMActivityService.GetDCMAReportsAreaWiseByCurrentUserAsync2(pageIndex, pageSize, search);
            return Ok(result);
        }

        [HttpGet("GetDCMAReportsRegionWise")]
        public async Task<IActionResult> GetDCMAReportsRegionWise(int pageIndex, int pageSize, string search,bool isAll)
        {
            try
            {
                var result = await _dailyCMActivityService.GetDCMAReportsRegionWiseByCurrentUserAsync(pageIndex, pageSize, search, isAll);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("GetDCMAReportsRegionWise2")]
        public async Task<ActionResult<DCMAReportsInDetailsResponse>> GetDCMAReportsRegionWise2(int pageIndex, int pageSize, string search)
        {

                DCMAReportsInDetailsResponse result = await _dailyCMActivityService.GetDCMAReportsRegionWiseByCurrentUserAsync2(pageIndex, pageSize, search);
                return Ok(result);

            
        }

    }
}
