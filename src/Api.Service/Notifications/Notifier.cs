using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Service.Notifications
{
    public class Notifier : INotifier, IDisposable
    {
        public List<Notification> Notifications = new();

        public void SetNotification(ValidationResult validations)
        {
            foreach (var error in validations.Errors)
            {
                SetNotification(new Notification(error.ErrorMessage));
            }
        }

        public void SetNotification(Notification notification)
        {
            Notifications.Add(notification);
        }

        public bool HasNotification()
        {
            return Notifications.Any();
        }
        
        public List<Notification> GetNotification()
        {
            return Notifications;
        }

        public void Dispose()
        {
            Notifications.Clear();
        }
    }
}
