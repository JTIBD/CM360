using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Bases;
using FMAplication.Models.Bases;
using FMAplication.Models.Sales;
using FMAplication.Models.Transaction;
using FMAplication.Repositories;

namespace FMAplication.Services.Common.Interfaces
{
    public interface ICommonService
    {
        List<int> GetDescendantNodeIds(List<int> nodeIds);
        List<int> GetParentNodeIds(List<int> nodeIds);
        List<NodeHierarchy> GetNodeTreeByFmUser(int userId);
        List<int> GetNodeIdsByFmUser(int userId);
        List<SalesPoint> GetSalesPointsByFMUser(int userId);
        void ValidateDateRange(DateTime fromDate, DateTime toDate);
        Task<string> GetTransactionNumber(TransactionType type, string sourceEntityCode);
        Task<string> GetTransactionNumber(TransactionType type, string sourceEntityCode, string destinationEntityCode, int serial = -1);
        int GetTransactionCount(TransactionType type);
        Task<List<T>> GetExistingSetups<T>(IRepository<T> repository, List<BaseSetupModel> setups,
            bool checkViolation = true) where T  :BaseSetup;

        void InsertOutlets<T>(List<T> list) where T : IWithOutlet;
        void InsertSalesPoints<T>(List<T> list) where T : IWithSalesPoint;
        List<NodeModel> GetParentNodesBySalesPoint(List<int> salesPointIds);
        Task<int> GetPosmHoldingAmount(int posmProductId, int salespointId);

        void HandleConflictingCase<T>(DateTime fromDateTime, DateTime toDateTime, List<T> existingSetups,List<SalesPoint>sps) where T:BaseSetup;

        void HandleUserTypeValidation<T>(AssignedUserType userType, List<T> existingSetups,
            List<SalesPoint> salesPointsOfExistingSetups) where T : BaseSetup;

        void InsertAvalableSpQuantity(List<SalesPointStockModel> spStocks);
        void CheckSpAvailableQuantityViolationForSPAdjustment(int tranferId);
        void CheckSpTransferAvailableQuantityViolation(int transactionId);
        void InsertRoutesOptional<T>(List<T> list) where T : IWithRouteOptional;
        bool IsOverlapping(DateTime from1,DateTime to1, DateTime from2, DateTime to2);
    }
}
