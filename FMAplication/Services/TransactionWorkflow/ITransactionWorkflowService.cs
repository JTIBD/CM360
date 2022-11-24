using System.Collections.Generic;
using System.Threading.Tasks;
using FMAplication.Domain.Transaction;
using FMAplication.Enumerations;
using FMAplication.Models.TransactionWorkflow;

namespace FMAplication.Services.TransactionWorkflow
{
    public interface ITransactionWorkflowService
    {
        Task<bool> CreateTransactionWorkflow(Transaction transaction);
        Task<bool> SendNotification(Transaction transaction);
        Task AcceptWorkflow(int userId, int transactionWorkflowId, Transaction transaction);
        Task RejectWorkflow(int userId, int transactionWorkflowId, Transaction transaction);

        Task<bool> IsAllAccepted(Transaction transaction);
        Task<List<TransactionWorkflowModel>> GetTransactionWorkFlows(List<int> transactionIds);
        Task<bool> CheckValidWorkflowSetup(Transaction transaction);
    }
}