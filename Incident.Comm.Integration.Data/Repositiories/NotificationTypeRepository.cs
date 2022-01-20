using Incident.Comm.Integration.Data.Interfaces;
using Incident.Comm.Integration.Data.Models;

namespace Incident.Comm.Integration.Data.Repositiories
{
    public class NotificationTypeRepository : Repository<NotificationType, IncidentCommDbContext>, INotificationTypeRepository
    {
        public NotificationTypeRepository(IncidentCommDbContext context)
           : base(context)
        {
        }
    }
}
