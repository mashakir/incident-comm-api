using Incident.Comm.Integration.Api.Dto;
using Incident.Comm.Integration.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Incident.Comm.Integration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _incidentService; 

        public IncidentController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        /// <summary>
        /// Returns incidents that overlap a given lat/long and optionaly a website issue category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetIncidents")]
        [Produces(typeof(IEnumerable<IncidentDto>))]
        public async Task<IActionResult> GetIncidents()
        {
            var incidents = await _incidentService.GetIncidents();

            return new OkObjectResult(incidents);
        }
    }
}
