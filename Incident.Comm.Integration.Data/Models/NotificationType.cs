namespace Incident.Comm.Integration.Data.Models
{
    public class NotificationType: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
    }
}
