using FMAplication.Domain.DailyActivities;
using FMAplication.Domain.Examples;
using FMAplication.Extensions;
using FMAplication.Models.DailyPOSM;
using FMAplication.Models.Examples;
using FMAplication.Repositories;
using FMAplication.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.Implementation
{
    public class DailyPOSMService : IDailyPOSMService
    {
        private readonly IRepository<DailyPOSM> _dailyposm;
        private DbContext _context;
        public DailyPOSMService(IRepository<DailyPOSM> dailyposm, DbContext context)
        {
            _dailyposm = dailyposm;
            _context = context;
        }


        public async Task<DailyPOSMModel> CreateAsync(DailyPOSMModel model)
        {
            var example = model.ToMap<DailyPOSMModel, DailyPOSM>();
            var result = await _dailyposm.CreateAsync(example);
            return result.ToMap<DailyPOSM, DailyPOSMModel>();

            
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _dailyposm.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsCodeExistAsync(int activityId, int id)
        {
            var result = id <= 0
                ? await _dailyposm.IsExistAsync(s => s.DailyCMActivityId == activityId)
                : await _dailyposm.IsExistAsync(s => s.DailyCMActivityId == activityId &&  s.Id != id);

            return result;
        }
        public async Task<DailyPOSMModel> GetDailyPOSMAsync(int id)
        {
            var result = await _dailyposm.FindAsync(s => s.Id == id);
            return result.ToMap<DailyPOSM, DailyPOSMModel>();
        }

        public async Task<IEnumerable<DailyPOSMModel>> GetDailyPOSMAsync()
        {
            var result = await _dailyposm.GetAllAsync();
            return result.ToMap<DailyPOSM, DailyPOSMModel>();
        }

        public async Task<IPagedList<DailyPOSMModel>> GetPagedDailyPOSMAsync(int pageNumber, int pageSize)
        {
            var result = await _dailyposm.GetAll().OrderBy(s => s.Id).ToPagedListAsync(pageNumber, pageSize);
            return result.ToMap<DailyPOSM, DailyPOSMModel>();

        }

        public async Task<IEnumerable<DailyPOSMModel>> GetQueryDailyPOSMAsync()
        {
            var result = await _dailyposm.ExecuteQueryAsyc<DailyPOSMModel>("SELECT * FROM Examples");
            return result;
        }

        public async Task<DailyPOSMModel> SaveAsync(DailyPOSMModel model)
        {
            var example = model.ToMap<DailyPOSMModel, DailyPOSM>();
            var result = await _dailyposm.CreateOrUpdateAsync(example);
            return result.ToMap<DailyPOSM, DailyPOSMModel>();
        }

        public async Task<DailyPOSMModel> UpdateAsync(DailyPOSMModel model)
        {
            var example = model.ToMap<DailyPOSMModel, DailyPOSM>();
            var result = await _dailyposm.UpdateAsync(example);
            return result.ToMap<DailyPOSM, DailyPOSMModel>();
        }

        public async Task<IEnumerable<DropdownOptions> > GetDropdownValueAsync()
        {
            var userId = AppIdentity.AppUser.UserId;

            userId = 1;

            SqlParameter param = new SqlParameter("@id", userId);
          
            var result = await _dailyposm.ExecuteQueryAsyc<DropdownOptions>("exec [dbo].[GetCMActivityId] @id", param);

           
            
            return result;
        }
    }
}
