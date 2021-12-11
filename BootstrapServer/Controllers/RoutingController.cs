using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BootstrapServer.Controllers
{

    [Route("/")]
    [ApiController]
    [AllowAnonymous]
    public class RoutingController : ControllerBase
    {
        private readonly ILogger<RoutingController> logger;
        private readonly BootstrapRoutingService routingService;

        public RoutingController(ILoggerFactory logger, BootstrapRoutingService routingService)
        {
            this.logger = logger.CreateLogger<RoutingController>();
            this.routingService = routingService;
        }

        [HttpGet("check")]
        public ActionResult Get()
        {
            var result = routingService.Process(HttpContext.Request.Query);
            logger.LogDebug($"Most suitable host: {result}");

            return Ok(result);
        }
    }
}
