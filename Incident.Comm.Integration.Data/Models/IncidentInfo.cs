using System.Collections.Generic;

namespace Incident.Comm.Integration.Data.Models
{
    public class IncidentInfo : Entity
    { 
        public int IceCallsCount { get; set; }
        public string IncidentRef { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int SearchCount { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}
