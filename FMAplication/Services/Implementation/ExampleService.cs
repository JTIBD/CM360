﻿using FMAplication.Domain.Examples;
using FMAplication.Extensions;
using FMAplication.Models.Examples;
using FMAplication.Repositories;
using FMAplication.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace FMAplication.Services.Implementation
{
    public class ExampleService : IExampleService
    {
        private readonly IRepository<Example> _example;

        public ExampleService(IRepository<Example> example)
        {
            _example = example;
        }


        public async Task<ExampleModel> CreateAsync(ExampleModel model)
        {
            var example = model.ToMap<ExampleModel, Example>();
            var result = await _example.CreateAsync(example);
            return result.ToMap<Example, ExampleModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _example.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsCodeExistAsync(string code, int id)
        {
            var result = id <= 0
                ? await _example.IsExistAsync(s => s.Code == code)
                : await _example.IsExistAsync(s => s.Code == code && s.Id != id);

            return result;
        }
        public async Task<ExampleModel> GetExampleAsync(int id)
        {
            var result = await _example.FindAsync(s => s.Id == id);
            return result.ToMap<Example, ExampleModel>();
        }

        public async Task<IEnumerable<ExampleModel>> GetExamplesAsync()
        {
            var result = await _example.GetAllAsync();
            return result.ToMap<Example, ExampleModel>();
        }

        public async Task<IPagedList<ExampleModel>> GetPagedExamplesAsync(int pageNumber, int pageSize)
        {
            var result = await _example.GetAll().OrderBy(s => s.Id).ToPagedListAsync(pageNumber, pageSize);
            return result.ToMap<Example, ExampleModel>();

        }

        public async Task<IEnumerable<ExampleModel>> GetQueryExamplesAsync()
        {
            var result = await _example.ExecuteQueryAsyc<ExampleModel>("SELECT * FROM Examples");
            return result;
        }

        public async Task<ExampleModel> SaveAsync(ExampleModel model)
        {
            var example = model.ToMap<ExampleModel, Example>();
            var result = await _example.CreateOrUpdateAsync(example);
            return result.ToMap<Example, ExampleModel>();
        }

        public async Task<ExampleModel> UpdateAsync(ExampleModel model)
        {
            var example = model.ToMap<ExampleModel, Example>();
            var result = await _example.UpdateAsync(example);
            return result.ToMap<Example, ExampleModel>();
        }


    }
}
