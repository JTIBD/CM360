using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Models.Common
{
    public class EnumList
    {
        public string[] ActionType => Enum.GetNames(typeof(ActionType));
        public string[] ApprovalStatus => Enum.GetNames(typeof(ApprovalStatus));
        public string[] AssignedUserType => Enum.GetNames(typeof(AssignedUserType));
        public string[] AvCommunicationCampaignType => Enum.GetNames(typeof(AvCommunicationCampaignType));
        public string[] CMUserType => Enum.GetNames(typeof(CMUserType));
        public string[] DailyAuditCheckStatus => Enum.GetNames(typeof(DailyAuditCheckStatus));
        public string[] FileUploadCode => Enum.GetNames(typeof(FileUploadCode));
        public string[] ModeOfApproval => Enum.GetNames(typeof(ModeOfApproval));
        public string[] NotificationStatus => Enum.GetNames(typeof(NotificationStatus));
        public string[] OrgRoleDesignation => Enum.GetNames(typeof(OrgRoleDesignation));
        public string[] POSMActionType => Enum.GetNames(typeof(POSMActionType));
        public string[] POSMProductType => Enum.GetNames(typeof(POSMProductType));
        public string[] POSMStatus => Enum.GetNames(typeof(POSMStatus));
        public string[] PosmTaskAssignStatus => Enum.GetNames(typeof(PosmTaskAssignStatus));
        public string[] PosmWorkType => Enum.GetNames(typeof(PosmWorkType));
        public string[] ReceivedStatus => Enum.GetNames(typeof(ReceivedStatus));
        public string[] RejectedStatus => Enum.GetNames(typeof(RejectedStatus));
        public string[] ReceivedSTaskInCompleteReasontatus => Enum.GetNames(typeof(TaskInCompleteReason));
        public string[] Status => Enum.GetNames(typeof(Status));
        public string[] StatusCode => Enum.GetNames(typeof(StatusCode));
        public string[] TaskStatus => Enum.GetNames(typeof(Enumerations.TaskStatus));
        public string[] TransactionStatus => Enum.GetNames(typeof(TransactionStatus));
        public string[] TransactionType => Enum.GetNames(typeof(TransactionType));
        public string[] WareHouseType => Enum.GetNames(typeof(WareHouseType));
        public string[] WFStatus => Enum.GetNames(typeof(WFStatus));
        public string[] WorkflowStatus => Enum.GetNames(typeof(WorkflowStatus));
        public string[] WorkflowType => Enum.GetNames(typeof(WorkflowType));
    }
}
