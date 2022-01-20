using Incident.Comm.Integration.Data.Models;

namespace Incident.Comm.Integration.Data.Interfaces
{
    public interface IIncidentInfoRepository : IRepository<IncidentInfo>
    {
        IncidentInfo GetIncident(string incidentReference);
    }
}
