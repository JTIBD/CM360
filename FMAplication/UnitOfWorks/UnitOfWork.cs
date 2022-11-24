using System;
using System.Threading.Tasks;
using FMAplication.Domain;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.WareHouse;
using FMAplication.Repositories;
using FMAplication.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace FMAplication.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _DBtransaction = null;
        private bool _disposed = false;

        #region Private properties

        private IRepository<Transaction> _transaction { get; set; }
        private IRepository<StockAdjustmentItems> _stockAdjust { get; set; }
        private IRepository<WareHouse> _warehouse { get; set; }

        #endregion

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _DBtransaction = _context.Database.BeginTransaction();
        }

        public void Save()
        {

            try
            {
                _context.SaveChanges();
                _DBtransaction?.Commit();
            }
            catch
            {
                _DBtransaction?.Rollback();
                throw;
            }
        }



        public IRepository<Transaction> Transaction => _transaction ??= new Repository<Transaction>(_context);
        public IRepository<StockAdjustmentItems> StockAdjust => _stockAdjust ??= new Repository<StockAdjustmentItems>(_context);
        public IRepository<WareHouse> Warehouse => _warehouse ??= new Repository<WareHouse>(_context);

        public async Task SaveAsync()
        {

            try
            {
                await _context.SaveChangesAsync();
                if (_DBtransaction != null) await _DBtransaction?.CommitAsync();
            }
            catch
            {
                if (_DBtransaction != null) await _DBtransaction?.RollbackAsync();
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _DBtransaction?.Dispose();
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
