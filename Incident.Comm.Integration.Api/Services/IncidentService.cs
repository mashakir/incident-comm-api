using AutoMapper;
using Incident.Comm.Integration.Api.Dto;
using Incident.Comm.Integration.Data.Interfaces;
using Incident.Comm.Integration.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Incident.Comm.Integration.Api.Services
{
    public interface IIncidentService
    { 
        Task<IEnumerable<IncidentDto>> GetIncidents();
    }

    public class IncidentService : IIncidentService
    {
        private readonly IIncidentInfoRepository _incidentInfoRepository;
        private IMapper _mapper;
        public IncidentService(IIncidentInfoRepository incidentInfoRepository, IMapper mapper)
        {
            _incidentInfoRepository = incidentInfoRepository;
            _mapper = mapper;
        }

        public IncidentDto GetIncident(string incidentReference)
        {
            var incident = _incidentInfoRepository.GetIncident(incidentReference);
            return _mapper.Map<IncidentInfo, IncidentDto>(incident);
        }

        public async Task<IEnumerable<IncidentDto>> GetIncidents()
        {
            var incidents = _incidentInfoRepository.GetAll().AsEnumerable();
            return await Task.Run(() => _mapper.Map<IEnumerable<IncidentInfo>, IEnumerable<IncidentDto>>(incidents));
        }
    }
}
