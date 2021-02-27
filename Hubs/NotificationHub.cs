using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FYP_WebApp.Common_Logic;
using Microsoft.AspNet.Identity;

namespace FYP_WebApp.Hubs
{
    public class NotificationHub : Hub
    {
        public static List<KeyValuePair<string, string>> Recipients = new List<KeyValuePair<string, string>>();

        public static void Notify(MessageType messageType, List<string> recipients, string message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            bool selector(KeyValuePair<string, string> s) => recipients.Contains(s.Key);
            var connectionIds = Recipients.Where(selector).Select(y => y.Value).ToList();
            //hubContext.Clients.All.DisplayNotification(message);

            if (messageType == MessageType.Urgent)
            {
                message = $"<a href='/Message/Details/'>" + message + "</a>";
                hubContext.Clients.Clients(connectionIds).DisplayUrgentNotification(message);
            }

            if (messageType == MessageType.CheckIn)
            {
                hubContext.Clients.Clients(connectionIds).DisplayCheckInNotification(message);
            }

            if (messageType == MessageType.Routine)
            {
                hubContext.Clients.Clients(connectionIds).DisplayRoutineNotification(message);
            }
        }

        public override Task OnConnected()
        {
            var userId = Context.User.Identity.GetUserId();
            //var userId = "4a611902-1d78-4e38-a6ae-a6f7b6dd2545";
            Recipients.Add(new KeyValuePair<string, string>(userId, Context.ConnectionId));
            return base.OnConnected();
        }
    }
}