using System;

namespace Incident.Comm.Integration.Api.Dto
{
    public class NotificationDto
    {
        public string Severity { get; set; }
        public string Name { get; set; } 
        public Guid NotificationTypeId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPublished { get; set; }
        public int IceCallsCount { get; set; }
        public Guid IncidentRef { get; set; }
        public string IncidentDescription { get; set; }
        public string IncidentLocation { get; set; }
        public int SearchCount { get; set; }
    }
}
