using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace FMAplication.Services.Notification
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly HubConnection _hubConnection;
        private readonly IOptions<ApplicationConfig> applicationConfiguration;

        public NotificationHubService(IOptions<ApplicationConfig> applicationConfiguration)
        {
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{applicationConfiguration.Value.Url}/NotificationHub")
                    .Build();

                InitConnection();
                this.applicationConfiguration = applicationConfiguration;
            }
            catch (Exception ex)
            {
                
            }
           
            
        }

      
        private async void InitConnection()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception e)
            {
                
            }
           
        }

        public async Task SendNotification(string message)
        {
            await _hubConnection.InvokeAsync("OnMessage", message);
        }

        public async Task SendNotificationToUser(int userId)
        {
            await _hubConnection.InvokeAsync("OnSendNotificationToUser", userId);
        }

        public async Task SendNotificationToUsers(List<int> userIds)
        {
            await _hubConnection.InvokeAsync("OnSendNotificationToUsers", userIds);
        }
    }

    public interface INotificationHubService
    {
        Task SendNotification(string message);
        Task SendNotificationToUser(int userId);
        Task SendNotificationToUsers(List<int> userIds);
    }

    public class ApplicationConfig
    {
        public string Url { get; set; }
    }
}

