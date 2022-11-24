using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace FMAplication.Hubs
{
    public class NotificationHub : Hub
    {
        
        static List<NotificationConnection> _notificationConnections = new List<NotificationConnection>();

        
        public async Task Connect(string userId)
        {
            int loggedinUserId = Convert.ToInt16(userId);
            var connectionId = Context.ConnectionId;
            if (_notificationConnections.All(p => p.ConnectionId != connectionId && p.UserId != loggedinUserId))
                _notificationConnections.Add(new NotificationConnection { UserId = loggedinUserId, ConnectionId = connectionId });
            await Clients.Client(connectionId).SendAsync("MyConnectionId", connectionId);
        }

        public override async Task OnConnectedAsync()
        {   
            var connectionId = Context.ConnectionId;
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var item = _notificationConnections.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
            if (item != null)
                _notificationConnections.Remove(item);
            

            await base.OnDisconnectedAsync(ex);
        }

        public async Task OnMessage(string message)
        {
            await Clients.All.SendAsync("OnMessage", message);
        }
        public async Task OnSendNotificationToUser(int userId)
        {
            var connectedUser = _notificationConnections.FirstOrDefault(x => x.UserId == userId);
            if (connectedUser != null)
               await Clients.Client(connectedUser.ConnectionId).SendAsync("OnSendNotificationToUser", userId);
        }

        public async Task OnSendNotificationToUsers(List<int> userIds)
        {
            var connectedUsers = _notificationConnections.Where(x => userIds.Contains(x.UserId));
            foreach (var connectedUser in connectedUsers)
            {
                await Clients.Client(connectedUser.ConnectionId).SendAsync("OnSendNotificationToUser", connectedUser.UserId);
            }
        }
    }

    public class NotificationConnection
    {
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
    }
}
