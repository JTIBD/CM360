using System;
using System.Threading.Tasks;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Repositories;

namespace FMAplication.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        new void Dispose();
        Task SaveAsync();
        void Save();

        IRepository<Transaction> Transaction { get; }
        IRepository<StockAdjustmentItems> StockAdjust { get; }
        IRepository<WareHouse> Warehouse { get; }
    }
}
