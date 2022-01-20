using System;

namespace Incident.Comm.Integration.Api.Dto
{
    public class IncidentDto
    {
        public string Severity { get; set; }
        public string NotificationName { get; set; }
        public Guid NotificationTypeId { get; set; }
        public string Description { get; set; } 
        public int IceCallsCount { get; set; }
        public Guid IncidentRef { get; set; }
        public string IncidentDescription { get; set; }
        public string IncidentLocation { get; set; }
        public int SearchCount { get; set; }
    }
}
