using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incident.Comm.Integration.Api.Controllers
{
    [Route("/")]
    public class AlwaysOnController : ControllerBase
    {
        /// <summary>
        /// Called by Azure always on;
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
