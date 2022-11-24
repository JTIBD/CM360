using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Controllers.Common;
using FMAplication.Models.Notifications;
using FMAplication.Services;

namespace FMAplication.Controllers.Notifications
{
    [ApiController]
    //    [JwtAuthorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotification()
        {
            var appIdentity = AppIdentity.AppUser;
            var result = await _notificationService.TransactionNotifications(appIdentity.UserId);
            var notification = new NotificationsViewModel
            {
                TransactionNotifications = result,
                Count = result.Count(x=>!x.IsSeen)  
            };
            return Ok(notification);
        }
        [HttpGet("AllNotifications")]
        public async Task<IActionResult> AllNotifications()
        {
            var appIdentity = AppIdentity.AppUser;
            var result = await _notificationService.GetAllTransactionNotifications(appIdentity.UserId);
            return Ok(result);
        }

        [HttpGet("MarkRead")]
        public async Task<IActionResult> MarkRead()
        {
            
            var appIdentity = AppIdentity.AppUser;
            var result = await _notificationService.MarkAsRead(appIdentity.UserId);
            return Ok(result);
            
        }


    }
}
