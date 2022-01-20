using Incident.Comm.Integration.Data.Interfaces;

namespace Incident.Comm.Integration.Api.Services
{
    public interface INotificationTypeService
    {
    }
    public class NotificationTypeService : INotificationTypeService
    {
        private readonly INotificationTypeRepository _notificationTypeRepository;
        public NotificationTypeService(INotificationTypeRepository notificationTypeRepository)
        {
            _notificationTypeRepository = notificationTypeRepository;
        }
    }
}
