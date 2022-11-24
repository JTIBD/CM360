using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.TransactionWorkFlows;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.Transaction;
using FMAplication.Models.TransactionNotifications;
using FMAplication.Repositories;
using FMAplication.Services.Users.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using X.PagedList;

namespace FMAplication.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<TransactionNotification> _transactionNotification;
        private readonly IRepository<Domain.TransactionWorkFlows.TransactionWorkflow> _transactionWorkflow;
        private readonly IUserInfoService _unInfoService;
        private readonly IRepository<Transaction> _transaction;
        private readonly IRepository<SalesPointTransfer> _salesPointTransfer;

        public NotificationService(IRepository<TransactionNotification> transactionNotification, 
            IRepository<Domain.TransactionWorkFlows.TransactionWorkflow> transactionWorkflow, 
            IUserInfoService unInfoService, 
            IRepository<Transaction> transaction, IRepository<SalesPointTransfer> salesPointTransfer)
        {
            _transactionNotification = transactionNotification;
            _transactionWorkflow = transactionWorkflow;
            _unInfoService = unInfoService;
            _transaction = transaction;
            _salesPointTransfer = salesPointTransfer;
        }
        public async Task<List<TransactionNotificationModel>> TransactionNotifications(int userId)
        {
            const int notificationTake = 8;
            var notifications = await _transactionNotification.FindAll(x => x.UserId == userId && !x.IsSeen).OrderByDescending(x=>x.CreatedTime).ToListAsync();

            if (notifications.Count < 9)
                notifications = await _transactionNotification.FindAllInclude(x => x.UserId == userId).OrderByDescending(x => x.CreatedTime)
                    .Take(notificationTake).ToListAsync();

            var transactionWorkFlowIds = notifications.Select(x => x.TransactionWorkFlowId).ToList();
            var transactionWorkflows = _transactionWorkflow.FindAll(x => transactionWorkFlowIds.Contains(x.Id)).ToList();

            var notificationList = notifications.MapToModel();
            foreach (var transactionNotification in notificationList)
            {
                transactionNotification.Transaction = await GetTransaction(transactionNotification);

                var selectedTransactionWorkflow = transactionWorkflows.FirstOrDefault(x => x.Id == transactionNotification.TransactionWorkFlowId);
                if (selectedTransactionWorkflow != null) transactionNotification.TwStatus = selectedTransactionWorkflow.TWStatus;

                if (selectedTransactionWorkflow?.SubmittedById != null)
                {
                    if (selectedTransactionWorkflow.SubmittedById == userId) transactionNotification.SubmittedBy = "You";

                    else
                    {
                        var user = await _unInfoService.GetUserAsync(selectedTransactionWorkflow.SubmittedById.Value);
                        if (user != null) transactionNotification.SubmittedBy = user.Name;
                    }
                }
            }
            return notificationList;
        }

        public async Task<List<TransactionNotificationModel>> GetAllTransactionNotifications(int userId)
        {
            
            var notifications = await _transactionNotification.FindAll(x => x.UserId == userId).OrderByDescending(x => x.CreatedTime).ToListAsync();

            var transactionWorkFlowIds = notifications.Select(x => x.TransactionWorkFlowId).ToList();
            var transactionWorkflows = _transactionWorkflow.FindAll(x => transactionWorkFlowIds.Contains(x.Id)).ToList();

            var notificationList = notifications.MapToModel();
            foreach (var transactionNotification in notificationList)
            {

                transactionNotification.Transaction = await  GetTransaction(transactionNotification);
                var selectedTransactionWorkflow = transactionWorkflows.FirstOrDefault(x => x.Id == transactionNotification.TransactionWorkFlowId);
                if (selectedTransactionWorkflow != null) transactionNotification.TwStatus = selectedTransactionWorkflow.TWStatus;

                if (selectedTransactionWorkflow?.SubmittedById != null)
                {
                    if (selectedTransactionWorkflow.SubmittedById == userId) transactionNotification.SubmittedBy = "You";

                    else
                    {
                        var user = await _unInfoService.GetUserAsync(selectedTransactionWorkflow.SubmittedById.Value);
                        if (user != null) transactionNotification.SubmittedBy = user.Name;
                    }
                }
            }
            return notificationList;
        }

        public async Task<TransactionNotificationModel> TransactionNotification(int userId, int transactionId)
        {
            var notification = await _transactionNotification.FindAsync(x => x.UserId == userId && x.TransactionId == transactionId);

            if (notification == null) return null;

            var transactionWorkflow = await _transactionWorkflow.FindAsync(x => x.Id == notification.TransactionWorkFlowId
                                                                         && x.TWStatus == TWStatus.Pending);

            if (transactionWorkflow == null) return null;
            var notificationModel = notification.MapToModel();    
            notificationModel.TwStatus = transactionWorkflow.TWStatus;
            return notificationModel;
        }
        public async Task<List<TransactionNotificationModel>> MarkAsRead(int userId)
        {
            var notifications = await _transactionNotification.FindAll(x => x.UserId == userId).ToListAsync();
            
            foreach (var transactionNotification in notifications)
            {
                transactionNotification.IsSeen = true;
            }

            await _transactionNotification.UpdateListAsync(notifications);
            return notifications.MapToModel();

        }

        #region private

        private async Task<TransactionModel> GetTransaction(TransactionNotificationModel notificationModel)
        {
            if (notificationModel.TransactionType == TransactionType.SP_Transfer)
            { 
                var salesPointTransfer = await _salesPointTransfer.FindAsync(x => x.Id == notificationModel.TransactionId);
                var result =  salesPointTransfer.ToMap<SalesPointTransfer, TransactionModel>();
                result.TransactionType = TransactionType.SP_Transfer;
                return result;
            }
            var transaction = await _transaction.FindAsync(x => x.Id == notificationModel.TransactionId);
           return transaction.ToMap<Transaction, TransactionModel>();
        }

        #endregion
    }
    public interface INotificationService
    {
        Task<List<TransactionNotificationModel>> TransactionNotifications(int userId);
        Task<TransactionNotificationModel> TransactionNotification(int userId, int transactionId);
        Task<List<TransactionNotificationModel>> MarkAsRead(int userId);
        Task<List<TransactionNotificationModel>> GetAllTransactionNotifications(int userId);
    }
}
