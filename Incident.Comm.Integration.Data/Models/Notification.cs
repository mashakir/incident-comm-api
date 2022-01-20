using System;

namespace Incident.Comm.Integration.Data.Models
{
    public class Notification: Entity
    {
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPublished { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
