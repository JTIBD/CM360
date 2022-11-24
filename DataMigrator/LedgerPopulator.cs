using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Domain.SPWisePOSMLedgers;
using FMAplication.Domain.Transaction;
using FMAplication.Enumerations;
using FMAplication.Repositories;
using System.Linq;
using FMAplication.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DataMigrator
{
    public interface ILedgerPopulator
    {
        Task RepairAllLedger();
    }

    public class LedgerPopulator : ILedgerPopulator
    {
        private readonly IRepository<DailyTask> _dailyTask;
        private readonly IRepository<Transaction> _transaction;
        private readonly IRepository<SalesPointStock> _spStock;
        private readonly IRepository<SalesPointReceivedTransfer> _spReceivedTransfers;
        private readonly IRepository<SalesPointReceivedTransferItem> _spReceivedTransferItems;
        private readonly IRepository<SPWisePOSMLedger> _spWisePosmLedger;

        public LedgerPopulator(IRepository<DailyTask> dailyTask ,
            IRepository<Transaction> transaction,
            IRepository<SalesPointStock> spStock,
             IRepository<SalesPointReceivedTransfer> spReceivedTransfers,
            IRepository<SalesPointReceivedTransferItem> spReceivedTransferItems, IRepository<SPWisePOSMLedger> spWisePosmLedger)
        {
            _dailyTask = dailyTask;
            _transaction = transaction;
            _spStock = spStock;
            _spReceivedTransfers = spReceivedTransfers;
            _spReceivedTransferItems = spReceivedTransferItems;
            _spWisePosmLedger = spWisePosmLedger;
        }


        private async Task<DateTime> GetFirstMissingDate()
        {
            var ledgerDates = await _spWisePosmLedger.GetAllActive().Select(x => x.Date).Distinct().ToListAsync();
            var firstPosmReceiveDate = await
                _transaction.GetAllActive().Where(x => x.TransactionType == TransactionType.Receive).OrderBy(x => x.TransactionDate).Select(x => x.TransactionDate).FirstOrDefaultAsync();

            var firstSpReceiveData = await _spReceivedTransfers.GetAllActive()
                .OrderBy(x => x.TransactionDate).Select(x => x.TransactionDate)
                .FirstOrDefaultAsync();

            var firstPosmAssignDate = await _dailyTask.GetAllActive().Where(x => x.DailyPosmTasks.Any())
                .OrderBy(x => x.DateTime).Select(x => x.DateTime).FirstOrDefaultAsync();

            var firstLedgerRecordDate = DateTime.UtcNow;

            if (firstPosmReceiveDate != default(DateTime).ToUniversalTime() &&
                firstLedgerRecordDate > firstPosmReceiveDate) firstLedgerRecordDate = firstPosmReceiveDate.BangladeshDateInUtc();

            if (firstSpReceiveData != default(DateTime).ToUniversalTime() &&
                firstLedgerRecordDate > firstSpReceiveData) firstLedgerRecordDate = firstSpReceiveData.BangladeshDateInUtc();

            if (firstPosmAssignDate != default(DateTime).ToUniversalTime() &&
                firstLedgerRecordDate > firstPosmAssignDate) firstLedgerRecordDate = firstPosmAssignDate.BangladeshDateInUtc();


            for (var date = firstLedgerRecordDate; date < DateTime.UtcNow.BangladeshDateInUtc(); date = date.AddDays(1))
            {
                if (!ledgerDates.Contains(date)) return date;
            }

            return DateTime.UtcNow.BangladeshDateInUtc();

        }

        public async Task RepairAllLedger()
        {
            var firstMissingDate = await GetFirstMissingDate();
            var previousDate = firstMissingDate.AddDays(-1);

            if (firstMissingDate == DateTime.UtcNow.BangladeshDateInUtc()) return;

            var posmReceivedTransactions = _transaction.GetAllActive().Where(x =>
                x.TransactionType == TransactionType.Receive && x.TransactionDate >= previousDate).
                Include(x => x.WDistributionRecieveTransactions).ToList();

            var spReceives = await _spReceivedTransfers.GetAllActive().Where(x =>
                x.TransactionDate >= previousDate).ToListAsync();
            spReceives = spReceives.Where(x => x.TransactionDate >= previousDate).ToList();

            var spReceiveIds = spReceives.Select(x => x.Id).ToList();

            var spReceivedItems = _spReceivedTransferItems.GetAllActive()
                .Where(x => spReceiveIds.Contains(x.TransferId)).Include(x => x.POSMProduct).ToList();

            spReceives.ForEach(x => x.Items = spReceivedItems.Where(item => item.TransferId == x.Id).ToList());

            var posmTasks = _dailyTask.GetAllActive().Where(x => x.DateTime >= previousDate).Include(x => x.DailyPosmTasks)
                .ThenInclude(x => x.DailyPosmTaskItems).ThenInclude(x => x.PosmProduct).ToList();


            var previousLedgers = await _spWisePosmLedger.GetAllActive().Where(x => x.Date == previousDate).ToListAsync();

            if (!previousLedgers.Any())
            {
                var spStoks = await _spStock.GetAllActive().Include(x => x.POSMProduct).ToListAsync();
                foreach (var stock in spStoks)
                {
                    var ledger = new SPWisePOSMLedger()
                    {
                        Date = previousDate,
                        SalesPointId = stock.SalesPointId,
                        PosmProductId = stock.POSMProductId,
                    };
                    previousLedgers.Add(ledger);
                }

            }

            try
            {
                await _spWisePosmLedger.DeleteAsync(x => x.Date > firstMissingDate);
            }
            catch (Exception)
            {
                Console.WriteLine($"Error occured during deleting all records after first missing date.");
            }


            for (var date = firstMissingDate; date <= DateTime.UtcNow.BangladeshDateInUtc(); date = date.AddDays(1))
            {
                Console.WriteLine($"Going to insert data for date: {date}");
                var repairedLedgers = new List<SPWisePOSMLedger>();

                var newLedgers = new List<SPWisePOSMLedger>();

                foreach (var ledger in previousLedgers)
                {
                    var newLedger = new SPWisePOSMLedger()
                    {
                        Date = date,
                        OpeningStock = ledger.ClosingStock,
                        SalesPointId = ledger.SalesPointId,
                        PosmProductId = ledger.PosmProductId
                    };

                    var posmReceived = posmReceivedTransactions
                        .Where(x => x.SalesPointId == newLedger.SalesPointId && date <= x.TransactionDate && x.TransactionDate < date.AddDays(1)).SelectMany(x => x.WDistributionRecieveTransactions)
                        .Where(x => x.POSMProductId == newLedger.PosmProductId).Sum(x => x.RecievedQuantity);

                    var spReceived = spReceives.Where(x => x.ToSalesPointId == newLedger.SalesPointId && date <= x.TransactionDate && x.TransactionDate < date.AddDays(1))
                        .SelectMany(x => x.Items).Where(x => x.POSMProductId == newLedger.PosmProductId)
                        .Sum(x => x.ReceivedQuantity);

                    newLedger.ReceivedStock = posmReceived + spReceived;

                    newLedger.ExecutedStock = posmTasks
                        .Where(x => x.SalesPointId == newLedger.SalesPointId && date <= x.DateTime && x.DateTime < date.AddDays(1))
                        .SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                        .Where(x => x.PosmProductId == newLedger.PosmProductId).Sum(x => x.Quantity);

                    newLedger.ClosingStock = newLedger.OpeningStock + newLedger.ReceivedStock - newLedger.ExecutedStock;

                    newLedgers.Add(newLedger);
                    repairedLedgers.Add(newLedger);
                    if (repairedLedgers.Count > 3000)
                    {
                        await _spWisePosmLedger.CreateListAsync(repairedLedgers);
                        repairedLedgers = new List<SPWisePOSMLedger>();
                    }
                }

                previousLedgers = newLedgers;
                if(repairedLedgers.Any())
                    await _spWisePosmLedger.CreateListAsync(repairedLedgers);
            }
        }
    }
}
