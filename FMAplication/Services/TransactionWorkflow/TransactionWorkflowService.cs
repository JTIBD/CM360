using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Transaction;
using FMAplication.Domain.TransactionWorkFlows;
using FMAplication.Domain.Users;
using FMAplication.Repositories;
using FMAplication.Domain.WorkFlows;
using FMAplication.Enumerations;
using FMAplication.Exceptions;
using FMAplication.Models.TransactionWorkflow;
using FMAplication.Services.Common.Interfaces;
using FMAplication.Services.Notification;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Rest;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace FMAplication.Services.TransactionWorkflow
{
    public class TransactionWorkflowService : ITransactionWorkflowService
    {
        private readonly IRepository<Domain.TransactionWorkFlows.TransactionWorkflow> _transactionWorkflow;
        
        private readonly IRepository<WorkFlowType> _workflowType;
        private readonly IRepository<WorkFlow> _workflow;
        private readonly IRepository<WorkFlowConfiguration> _workflowConfig;
        private readonly IRepository<TransactionNotification> _transactionNotification;
        private readonly INotificationHubService _notificationHubService;
        private readonly IRepository<UserRoleMapping> _userRoles;
        private readonly IRepository<UserInfo> _userInfo;
        private readonly IRepository<SalesPointNodeMapping> _salesPointNode;
        private readonly IRepository<UserTerritoryMapping> _userTerritory;
        private readonly ICommonService _common;


        public TransactionWorkflowService(IRepository<Domain.TransactionWorkFlows.TransactionWorkflow> transactionWorkflow,  
             IRepository<WorkFlowType> workflowType,
             IRepository<WorkFlow> workflow, IRepository<WorkFlowConfiguration> workflowConfig, 
             IRepository<TransactionNotification> transactionNotification,
             INotificationHubService notificationHubService, 
             IRepository<UserRoleMapping> userRoles,
             IRepository<UserInfo> userInfo,
             IRepository<SalesPointNodeMapping> salesPointNode, 
             IRepository<UserTerritoryMapping> userTerritory, 
             ICommonService common)
        {
            _transactionWorkflow = transactionWorkflow;
            _workflowType = workflowType;
            _workflow = workflow;
            _workflowConfig = workflowConfig;
            _transactionNotification = transactionNotification;
            _notificationHubService = notificationHubService;
            _userRoles = userRoles;
            _userInfo = userInfo;
            _salesPointNode = salesPointNode;
            _userTerritory = userTerritory;
            _common = common;
        }
        public async Task<bool> CreateTransactionWorkflow(Transaction transaction)
        {
            var workflow = await GetWorkflow(transaction.TransactionType);
            if (workflow == null) return false;

            var workflowConfigs = await GetWorkflowConfigs(transaction.TransactionType);
            if (workflowConfigs.Count == 0) return false;

            var existingTransactionWorkflow = await _transactionWorkflow.FindAsync(x => x.TransactionId == transaction.Id 
                                                                                       && x.WorkFlowId == workflow.Id);

            if (existingTransactionWorkflow != null) return false;


            var configIds = workflowConfigs.Select(x => x.Id).ToList();
            var isExistingTransactionLogs = await _transactionWorkflow.FindAllAsync(x => x.TransactionId == transaction.Id &&
                                                                             configIds.Contains(x.WorkflowConfigurationId));

            if (isExistingTransactionLogs.Count > 0) return false;

            var transactionWorkflows = new List<Domain.TransactionWorkFlows.TransactionWorkflow>();
            foreach (var workFlowConfiguration in workflowConfigs)
            {
                var transactionWorkflow = new Domain.TransactionWorkFlows.TransactionWorkflow
                {
                    TransactionId = transaction.Id, 
                    TransactionType = transaction.TransactionType,
                    WorkFlowId = workflow.Id, 
                    WorkflowConfigurationId = workFlowConfiguration.Id, 
                    RoleId = workFlowConfiguration.RoleId, 
                    UserId = workFlowConfiguration.UserId, 
                    Sequence = workFlowConfiguration.Sequence, 
                    TWStatus = TWStatus.Pending, 

                };
                transactionWorkflows.Add(transactionWorkflow);
            }
            await _transactionWorkflow.CreateListAsync(transactionWorkflows);

            return true;
        }
        public async Task<bool> SendNotification(Transaction transaction)
        {
            
            var workflow = await GetWorkflow(transaction.TransactionType);
            if (workflow == null) return false;

            var configs = await GetWorkflowConfigs(transaction.TransactionType);
            if (configs.Count == 0) return false;

            var configIds = configs.Select(x => x.Id).ToList();

            var transactionWorkflows = await _transactionWorkflow.FindAll(x => x.TransactionId == transaction.Id && 
                                                                              x.WorkFlowId == workflow.Id && configIds.Contains(x.WorkflowConfigurationId))
                                                                              .ToListAsync();


            if (transactionWorkflows.Count == 0) return false;

            var selectedTw = transactionWorkflows.OrderBy(x => x.Sequence)
                .FirstOrDefault(x => x.TWStatus == TWStatus.Pending);

            if (selectedTw == null) return false;

            await NotificationToUser(workflow, selectedTw, transaction);
            return true;
        }
        public async Task AcceptWorkflow(int userId, int transactionWorkflowId, Transaction transaction)
        {
            var workflow = await GetWorkflow(transaction.TransactionType);
            if (workflow == null) throw new AppException("Workflow not found");

            var configs = await GetWorkflowConfigs(transaction.TransactionType);
            if (configs.Count == 0) throw new AppException("Workflow config not found");

            var transactionWorkflows = await _transactionWorkflow.FindAllAsync(x => x.TransactionId == transaction.Id);

            if (transactionWorkflows.Any(x => x.TWStatus == TWStatus.Rejected))
                throw new AppException("workflow already rejected");

            if (transactionWorkflows.Count(x => x.TWStatus == TWStatus.Accepted) == workflow.WorkflowStep)
                throw new AppException("All workflow already accepted");



            var transactionWorkFlow =  transactionWorkflows.FirstOrDefault(x =>  x.TransactionId == transaction.Id
                                                                                 && x.Id == transactionWorkflowId);
            if (transactionWorkFlow == null) throw new AppException("Transaction workflow not found");
            if (transactionWorkFlow.TWStatus == TWStatus.Accepted) throw new AppException("Notification already accepted");
            
            //TODO: need to check valid user 


            transactionWorkFlow.TWStatus = TWStatus.Accepted;
            transactionWorkFlow.SubmittedById = userId;
            await _transactionWorkflow.UpdateAsync(transactionWorkFlow);
        }
        public async Task RejectWorkflow(int userId, int transactionWorkflowId, Transaction transaction)
        {
            var workflow = await GetWorkflow(transaction.TransactionType);
            if (workflow == null) throw new AppException("Workflow not found");

            var configs = await GetWorkflowConfigs(transaction.TransactionType);
            if (configs.Count == 0) throw new AppException("Workflow config not found");

            var transactionWorkflows = await _transactionWorkflow.FindAllAsync(x => x.TransactionId == transaction.Id);

            if (transactionWorkflows.Any(x => x.TWStatus == TWStatus.Rejected))
                throw new AppException("workflow already rejected");

            if (transactionWorkflows.Count(x => x.TWStatus == TWStatus.Accepted) == workflow.WorkflowStep)
                throw new AppException("All workflow already accepted");


            var transactionWorkFlow = transactionWorkflows.FirstOrDefault(x => x.TransactionId == transaction.Id && x.Id == transactionWorkflowId);
            if (transactionWorkFlow == null) throw new AppException("Transaction workflow not found");

            //TODO: need to check valid user 
            transactionWorkFlow.SubmittedById = userId;
            transactionWorkFlow.TWStatus = TWStatus.Rejected;
            await _transactionWorkflow.UpdateAsync(transactionWorkFlow);
        }
        public async Task<bool> IsAllAccepted(Transaction transaction)
        {
            var workflow = await GetWorkflow(transaction.TransactionType);
            var count = await _transactionWorkflow.CountFuncAsync(x =>
                x.TransactionId == transaction.Id && x.TWStatus == TWStatus.Accepted);

            return workflow.WorkflowStep == count;
        }
        public async Task<List<TransactionWorkflowModel>> GetTransactionWorkFlows(List<int> transactionIds)
        {
            if (transactionIds.Count == 0) return new List<TransactionWorkflowModel>();

            var transactionWorkflows = await _transactionWorkflow.Where(x => 
                                                          transactionIds.Contains(x.TransactionId) && (x.TWStatus == TWStatus.Pending || x.TWStatus == TWStatus.Rejected))
                                                          .Include(x=>x.User)
                                                          .Include(r => r.Role).ToListAsync();


            return transactionWorkflows.MapToModel();
        }

        public async Task<bool> CheckValidWorkflowSetup(Transaction transaction)
        {
            var workflow = await GetWorkflow(transaction.TransactionType);
            if (workflow == null) throw new AppException("Failed to confirm, No workflow found");

            var configs = await GetWorkflowConfigs(transaction.TransactionType);
            if (configs.Count == 0)
                throw new AppException("Failed to confirm, No workflow config found");


            if (workflow.WorkflowConfigType == WorkflowConfigType.Role)
            {
                var firstWorkflow = configs.Find(x => x.WorkFlowId == workflow.Id && x.Sequence == 1);
                if (firstWorkflow.RoleId != null && transaction.SalesPointId != 0)
                {
                    var userIds = await GetRoleAndSalespointNodeWiseUserIds(firstWorkflow.RoleId.Value, transaction.SalesPointId);
                    if (!userIds.Any())
                        throw new AppException("Failed to confirm, Users not found for first Approver role");
                }
            }
            return true;
        }

        #region private methods
        private async Task NotificationToUser(WorkFlow workflow, Domain.TransactionWorkFlows.TransactionWorkflow selectedTw, Transaction transaction)
        {
            if (workflow.WorkflowConfigType == WorkflowConfigType.User && selectedTw.UserId != null)
            {
                await SaveTransactionNotification(selectedTw);
                await _notificationHubService.SendNotificationToUser(selectedTw.UserId.Value);
            }
            else if (workflow.WorkflowConfigType == WorkflowConfigType.Role && selectedTw.RoleId != null && 
                     transaction.TransactionType == TransactionType.SalesPointStockAdjustment 
                     || transaction.TransactionType == TransactionType.SP_Transfer)
            {
                if (transaction.SalesPointId != 0)
                {
                    var userIds = await GetRoleAndSalespointNodeWiseUserIds(selectedTw.RoleId.Value, transaction.SalesPointId);
                    if (userIds.Count > 0)
                    {
                        await SaveTransactionNotifications(selectedTw, userIds);
                        await _notificationHubService.SendNotificationToUsers(userIds);
                    }
                    else
                        throw new AppException("Failed to sent notifications");

                }
            }
        }
        private async Task SaveTransactionNotifications(Domain.TransactionWorkFlows.TransactionWorkflow selectedTw, List<int> userIds)
        {
            var list = new List<TransactionNotification>();
            foreach (var userId in userIds)
            {
                var notification = new TransactionNotification
                {
                    TransactionId = selectedTw.TransactionId,
                    TransactionType = selectedTw.TransactionType,
                    UserId = userId,
                    TransactionWorkFlowId = selectedTw.Id
                };

                list.Add(notification);
            }

            await _transactionNotification.CreateListAsync(list);
        }
        private async Task SaveTransactionNotification(Domain.TransactionWorkFlows.TransactionWorkflow selectedTw)
        {
            var notification = new TransactionNotification
            {
                TransactionId = selectedTw.TransactionId,
                UserId = selectedTw.UserId.Value,
                TransactionWorkFlowId = selectedTw.Id,
                TransactionType = selectedTw.TransactionType
            };
            await _transactionNotification.CreateAsync(notification);
        }
        private async Task<List<WorkFlowConfiguration>> GetWorkflowConfigs(TransactionType transactionType)
        {
            var getWorkFlowType = GetworkFlowType(transactionType);
            var workflowConfigs = new List<WorkFlowConfiguration>();
            var workflowType = await _workflowType.FindAsync(x => x.WorkflowTypeName.ToLower() == getWorkFlowType.ToString().ToLower());
            if (workflowType == null) return workflowConfigs;

            var workflow = await _workflow.FindAsync(x => x.WorkflowType == workflowType.Id);
            if (workflow == null) return workflowConfigs;

            workflowConfigs = await _workflowConfig.FindAll(x => x.WorkFlowId == workflow.Id).ToListAsync();
            return workflowConfigs;
        }
        private WorkFlowTypes GetworkFlowType(TransactionType transactionType)
        {
            if (transactionType == TransactionType.StockAdjustment)
                return WorkFlowTypes.CWStockAdjustment;
            else if (transactionType == TransactionType.SalesPointStockAdjustment)
                return WorkFlowTypes.SPStockAdjustment;
            else if (transactionType == TransactionType.SP_Transfer)
                return WorkFlowTypes.SPToSpTransfer;
            else 
                throw new AppException("Failed to identity workflow type");
        }
        private async Task<WorkFlow> GetWorkflow(TransactionType transactionType)
        {
            var getWorkFlowType = GetworkFlowType(transactionType);
            var workflowType = await _workflowType.FindAsync(x => x.WorkflowTypeName.ToLower() == getWorkFlowType.ToString().ToLower());
            if (workflowType == null) return null;

            var workflow = await _workflow.FindAsync(x => x.WorkflowType == workflowType.Id);
            return workflow;
        }
        private async Task<List<int>> GetRoleAndSalespointNodeWiseUserIds(int roleId, int salesPointId)
        {
            var nodeIds = new List<int>();
            var userIdsByRole = await _userRoles.FindAll(x => x.RoleId == roleId).Select(x=>x.UserInfoId).ToListAsync();
            var node = await _salesPointNode.FindAsync(x => x.SalesPointId == salesPointId);
            if (node == null) throw new AppException("Node not found");

            nodeIds.Add(node.NodeId);
            var nodeList = _common.GetParentNodeIds(nodeIds);
            nodeList.Add(node.NodeId);

            var userIdsBySalespoint = _userTerritory.FindAll(x => nodeList.Contains(x.NodeId)).Select(x => x.UserInfoId).ToList();
            var userIds = userIdsByRole.Intersect(userIdsBySalespoint).ToList();
            var activeUserIds = _userInfo.GetAllActive().Where(x => userIds.Contains(x.Id)).Select(x => x.Id).ToList();
            return activeUserIds;
        }
        #endregion

    }
}
