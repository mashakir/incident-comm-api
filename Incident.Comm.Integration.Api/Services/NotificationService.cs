using Incident.Comm.Integration.Data.Interfaces;

namespace Incident.Comm.Integration.Api.Services
{
    public interface INotificationService
    {
    }
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
    }
}
