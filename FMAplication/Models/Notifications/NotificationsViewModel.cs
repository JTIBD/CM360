using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Models.TransactionNotifications;

namespace FMAplication.Models.Notifications
{
    public class NotificationsViewModel
    {
        public int Count { get; set; }
        public IList<TransactionNotificationModel> TransactionNotifications { get; set; }  
    }
}
