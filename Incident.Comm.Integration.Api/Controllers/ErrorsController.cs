using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Incident.Comm.Integration.Api.Controllers
{
    /// <summary>
    /// Controller containing actions to be executed by the exception handling middleware in the event of an error.
    /// </summary>
    [Route("[controller]")]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : Controller
    {
        private readonly ILogger<ErrorsController> _logger;

        /// <summary>
        /// Creates an instance of <see cref="ErrorsController"/>.
        /// </summary>
        /// <param name="logger"></param>
        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// The default action to be executed in the event of an exception.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error, "Unhandled exception.");
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
