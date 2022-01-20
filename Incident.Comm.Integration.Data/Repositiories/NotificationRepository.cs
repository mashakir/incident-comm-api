using Incident.Comm.Integration.Data.Interfaces;
using Incident.Comm.Integration.Data.Models;

namespace Incident.Comm.Integration.Data.Repositiories
{
    public class NotificationRepository : Repository<Notification, IncidentCommDbContext>, INotificationRepository
    {
        public NotificationRepository(IncidentCommDbContext context)
           : base(context)
        {
        } 
    }
}
