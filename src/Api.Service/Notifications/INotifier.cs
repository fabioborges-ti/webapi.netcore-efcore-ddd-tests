using System.Collections.Generic;
using FluentValidation.Results;

namespace Api.Service.Notifications
{
    public interface INotifier
    {
        void SetNotification(ValidationResult validations); 
        void SetNotification(Notification notification);
        bool HasNotification();
        List<Notification> GetNotification();
    }
}
