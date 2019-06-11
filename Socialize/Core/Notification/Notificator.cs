using Notifications.Wpf;
using UnifyMe.Core.Enums;
using System;

namespace UnifyMe.Notification
{
    public static class Notificator
    {
        public static void Services_SendNotify(ServicesEnums service = ServicesEnums.None, string body = "Hai un nuovo messaggio da leggere.")
        {
            if (service == ServicesEnums.None)
                throw new InvalidOperationException("Service is required.");

            // In a real app, these would be initialized with actual data
            string title = $"UnifyMe - {service.ToString()}";
            string content = body;
            NotificationManager notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = content,
                Type = NotificationType.Information,
            });
        }
    }
}
