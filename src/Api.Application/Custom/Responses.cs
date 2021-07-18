using Api.Service.Notifications;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Api.Application.Custom
{
    [ExcludeFromCodeCoverage]
    public static class Responses
    {
        public static object GetErrors(List<Notification> notifications)
        {
            return new
            {
                success = false,
                error = notifications.Select(n => n.Message)
            };
        }

        public static object GetErrors(string message)
        {
            return new
            {
                success = false,
                error = message
            };
        }
    }
}
