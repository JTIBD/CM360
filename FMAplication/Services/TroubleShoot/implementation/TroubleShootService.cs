using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Products;
using FMAplication.Helpers;
using FMAplication.Repositories;
using FMAplication.Services.TroubleShoot.interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMAplication.Services.TroubleShoot.implementation
{
    public class TroubleShootService:ITroubleShootService
    {
        private readonly IRepository<Domain.AVCommunications.AvCommunication> _avCommunication;
        private readonly IRepository<DailyInformationTask> _informationTask;
        private readonly IRepository<DailyPosmTaskItems> _dailyPosmTaskItems;
        private readonly IRepository<DailyPosmTask> _dailyPosmTask;
        private readonly IRepository<POSMProduct> _posmProduct;
        private readonly IRepository<Product> _product;

        public TroubleShootService(IRepository<Domain.AVCommunications.AvCommunication> avCommunication, IRepository<DailyInformationTask> informationTask, IRepository<DailyPosmTaskItems> dailyPosmTaskItems, IRepository<DailyPosmTask> dailyPosmTask, IRepository<POSMProduct> posmProduct, IRepository<Product> product)
        {
            _avCommunication = avCommunication;
            _informationTask = informationTask;
            _dailyPosmTaskItems = dailyPosmTaskItems;
            _dailyPosmTask = dailyPosmTask;
            _posmProduct = posmProduct;
            _product = product;
        }

        public async Task FixFileUrls()
        {
            await FixFileUrlsOfAvCommunications();
            await FixFileUrlsOfDailyInformationTasks();
            await FixFileUrlsOfDailyPosmTaskItems();
            await FixFileUrlsOfDailyPosmTasks();
            await FixFileUrlsOfPosmProducts();
            await FixFileUrlsOfProducts();
        }

        private async Task FixFileUrlsOfProducts()
        {
            var records = await _product.GetAll().ToListAsync();
            foreach (var record in records)
            {
                record.ImageUrl = Utility.GetRealUrl(record.ImageUrl);
            }

            await _product.UpdateListAsync(records);

        }

        private async Task FixFileUrlsOfPosmProducts()
        {
            var records = await _posmProduct.GetAll().ToListAsync();
            foreach (var record in records)
            {
                record.PlanogramImageUrl = Utility.GetRealUrl(record.PlanogramImageUrl);
                record.ImageUrl = Utility.GetRealUrl(record.ImageUrl);
            }

            await _posmProduct.UpdateListAsync(records);

        }

        private async Task FixFileUrlsOfDailyPosmTasks()
        {
            var records = await _dailyPosmTask.GetAll().ToListAsync();
            foreach (var record in records)
            {
                record.ExistingImage = Utility.GetRealUrl(record.ExistingImage);
                record.NewImage = Utility.GetRealUrl(record.NewImage);
            }

            await _dailyPosmTask.UpdateListAsync(records);

        }

        private async Task FixFileUrlsOfDailyPosmTaskItems()
        {
            var records = await _dailyPosmTaskItems.GetAll().ToListAsync();
            foreach (var record in records)
            {
                record.Image = Utility.GetRealUrl(record.Image);
            }

            await _dailyPosmTaskItems.UpdateListAsync(records);

        }

        private async Task FixFileUrlsOfDailyInformationTasks()
        {
            var records = await _informationTask.GetAll().ToListAsync();
            foreach (var record in records)
            {
                record.InsightImage = Utility.GetRealUrl(record.InsightImage);
                record.RequestImage = Utility.GetRealUrl(record.RequestImage);
            }

            await _informationTask.UpdateListAsync(records);
        }

        private async Task FixFileUrlsOfAvCommunications()
        {
            var records = await _avCommunication.GetAll().ToListAsync();
            foreach (var record in records)
            {
                record.FilePath = Utility.GetRealUrl(record.FilePath);
            }

            await _avCommunication.UpdateListAsync(records);
        }
    }
}