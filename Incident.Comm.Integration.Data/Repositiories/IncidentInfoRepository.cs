using Incident.Comm.Integration.Data.Interfaces;
using Incident.Comm.Integration.Data.Models;
using System.Linq;

namespace Incident.Comm.Integration.Data.Repositiories
{
    public class IncidentInfoRepository : Repository<IncidentInfo, IncidentCommDbContext>, IIncidentInfoRepository
    {
        public IncidentInfoRepository(IncidentCommDbContext context)
           : base(context)
        {
        }

        public IncidentInfo GetIncident(string incidentReference)
        {
            return Db.Set<IncidentInfo>()
             .FirstOrDefault(o => o.IncidentRef == incidentReference);
        }
    }
}
