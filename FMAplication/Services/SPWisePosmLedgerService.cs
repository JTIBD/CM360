using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.DailyTasks;
using FMAplication.Domain.Sales;
using FMAplication.Domain.SPWisePOSMLedgers;
using FMAplication.Domain.Transaction;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.SPWisePOSMLedgers;
using FMAplication.Repositories;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace FMAplication.Services
{
    public class SPWisePosmLedgerService : ISPWisePosmLedgerService
    {
        private readonly IRepository<SalesPointStock> _salesPointStock;
        private readonly IRepository<SPWisePOSMLedger> _spWisePosmLedger;
        private readonly IRepository<Transaction> _transaction;
        private readonly IRepository<SalesPointReceivedTransfer> _spReceivedTransfers;
        private readonly IRepository<SalesPointReceivedTransferItem> _spReceivedTransferItems;
        private readonly IRepository<DailyTask> _dailyTask;

        public SPWisePosmLedgerService(IRepository<SalesPointStock> salesPointStock, 
            IRepository<SPWisePOSMLedger> spWisePosmLedger, IRepository<Transaction> transaction, IRepository<SalesPointReceivedTransfer> spReceivedTransfers, IRepository<SalesPointReceivedTransferItem> spReceivedTransferItems, IRepository<DailyTask> dailyTask)
        {
            _salesPointStock = salesPointStock;
            _spWisePosmLedger = spWisePosmLedger;
            _transaction = transaction;
            _spReceivedTransfers = spReceivedTransfers;
            _spReceivedTransferItems = spReceivedTransferItems;
            _dailyTask = dailyTask;
        }


        private async Task<List<SPWisePOSMLedger>> GetLedgers(DateTime date, int salesPointId, List<int> posmIds)
        {
            var ledgers = await _spWisePosmLedger.GetAll().Where(x =>
                posmIds.Contains(x.PosmProductId) && x.SalesPointId == salesPointId &&
                x.Date == date).ToListAsync();
            return ledgers;
        }

        public async Task SPWisePOSMLedgerExecutedStock(List<DailyPosmTaskItems> posmTaskItemses, int salesPointId)
        {
            var todaysDate = DateTime.UtcNow.BangladeshDateInUtc();
            List<SPWisePOSMLedger> spWisePosmLedgers = new List<SPWisePOSMLedger>();

            var ledgers = await GetLedgers(todaysDate, salesPointId, posmTaskItemses.Select(x => x.PosmProductId).ToList());
            if (!ledgers.Any())
            {
                try
                {
                    await InsertMissingLedgers(todaysDate,todaysDate.AddDays(-3));
                    ledgers = await GetLedgers(todaysDate, salesPointId, posmTaskItemses.Select(x => x.PosmProductId).ToList());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            foreach (var item in posmTaskItemses)
            {
                var ledger = ledgers.Find(x => x.PosmProductId == item.PosmProductId);

                if (ledger != null)
                {
                    ledger.ExecutedStock += item.Quantity;
                    ledger.ClosingStock = ledger.ClosingStock - ledger.ExecutedStock;
                    spWisePosmLedgers.Add(ledger);
                }
                else
                {
                    Console.WriteLine("Ledger not found, when insert execution");
                }

            }
            await _spWisePosmLedger.UpdateListAsync(spWisePosmLedgers);
        }

        public async Task SPWisePOSMLedgerReceivedStock(List<ReceivedPOSM> receivedPOSMs, int salesPointId)
        {
            var todaysDate = DateTime.UtcNow.BangladeshDateInUtc();
            var posmIds = receivedPOSMs.Select(x => x.POSMProductId).ToList();

            List<SPWisePOSMLedger> spWisePosmLedgers = new List<SPWisePOSMLedger>();
            var ledgers = await GetLedgers(todaysDate, salesPointId, posmIds);

            if (!ledgers.Any())
            {
                try
                {
                    await InsertMissingLedgers(todaysDate,todaysDate.AddDays(-3));
                    ledgers = await GetLedgers(todaysDate, salesPointId, posmIds);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            foreach (var ledger in ledgers)
            {
                var item = receivedPOSMs.First(x => x.POSMProductId == ledger.PosmProductId);
                ledger.ReceivedStock += item.Quantity;
                ledger.ClosingStock += item.Quantity;
                spWisePosmLedgers.Add(ledger);
            }
            await _spWisePosmLedger.UpdateListAsync(spWisePosmLedgers);
        }

        public async Task InsertMissingLedgers(DateTime date, DateTime? thresholdDate=null)
        {

            if(thresholdDate != null && thresholdDate > date) return;

            var previousDate = date.AddDays(-1);

            var previousLedgers = await _spWisePosmLedger.GetAllActive().Where(x => x.Date == previousDate).ToListAsync();

            if (!previousLedgers.Any())
            {
                await InsertMissingLedgers(previousDate, thresholdDate);
                previousLedgers = await _spWisePosmLedger.GetAllActive().Where(x => x.Date == previousDate).ToListAsync();
                if(!previousLedgers.Any()) return;
            }

            var posmReceivedTransactions = _transaction.GetAllActive().Where(x =>
                    x.TransactionType == TransactionType.Receive && date <= x.TransactionDate && x.TransactionDate < date.AddDays(1)).
                Include(x => x.WDistributionRecieveTransactions).ToList();

            var spReceives = await _spReceivedTransfers.GetAllActive().Where(x => date <= x.TransactionDate && x.TransactionDate < date.AddDays(1)).ToListAsync();

            var spReceiveIds = spReceives.Select(x => x.Id).ToList();

            var spReceivedItems = _spReceivedTransferItems.GetAllActive()
                .Where(x => spReceiveIds.Contains(x.TransferId)).Include(x => x.POSMProduct).ToList();

            spReceives.ForEach(x => x.Items = spReceivedItems.Where(item => item.TransferId == x.Id).ToList());

            var posmTasks = _dailyTask.GetAllActive().Where(x => x.IsSubmitted && x.DailyPosmTasks.Any() && date <= x.DateTime && x.DateTime < date.AddDays(1)).Include(x => x.DailyPosmTasks)
                .ThenInclude(x => x.DailyPosmTaskItems).ThenInclude(x => x.PosmProduct).ToList();

            var records = new List<SPWisePOSMLedger>();

            foreach (var ledger in previousLedgers)
            {
                var record = new SPWisePOSMLedger()
                {
                    Date = date,
                    SalesPointId = ledger.SalesPointId,
                    PosmProductId = ledger.PosmProductId,
                    OpeningStock = ledger.ClosingStock,
                    PosmProduct = ledger.PosmProduct,
                };

                var posmReceived = posmReceivedTransactions
                    .Where(x => x.SalesPointId == record.SalesPointId).SelectMany(x => x.WDistributionRecieveTransactions)
                    .Where(x => x.POSMProductId == record.PosmProductId).Sum(x => x.RecievedQuantity);

                var spReceived = spReceives.Where(x => x.ToSalesPointId == record.SalesPointId)
                    .SelectMany(x => x.Items).Where(x => x.POSMProductId == record.PosmProductId)
                    .Sum(x => x.ReceivedQuantity);

                record.ReceivedStock = posmReceived + spReceived;

                record.ExecutedStock = posmTasks
                    .Where(x => x.SalesPointId == record.SalesPointId)
                    .SelectMany(x => x.DailyPosmTasks).SelectMany(x => x.DailyPosmTaskItems)
                    .Where(x => x.PosmProductId == record.PosmProductId).Sum(x => x.Quantity);

                record.ClosingStock = record.OpeningStock + record.ReceivedStock - record.ExecutedStock;
                records.Add(record);
            }

            await _spWisePosmLedger.CreateListAsync(records);
            _spWisePosmLedger.DetachList(records);
        }
    }

    public interface ISPWisePosmLedgerService
    {
        Task SPWisePOSMLedgerExecutedStock(List<DailyPosmTaskItems> posmTaskItemses, int salesPointId);
        Task SPWisePOSMLedgerReceivedStock(List<ReceivedPOSM> receivedPOSMs, int salesPointId);
        Task InsertMissingLedgers(DateTime date, DateTime? thresholdDate = null);
    }
}
