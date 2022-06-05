using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Relax.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TechController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public TechController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        [HttpGet]
        [Route("version")]
        public async Task<IActionResult> GetVersionAsync(CancellationToken cancellationToken)
        {
            var v = typeof(TechController).Assembly.GetName().Version;
            return Ok(v.ToString());
        }

        [HttpGet]
        [Route("selfTest")]
        public async Task<IActionResult> SelfTestAsync(CancellationToken cancellationToken)
        {
            foreach (var healthCheck in _serviceProvider.GetServices<IHealthCheck>())
            {
                var result = await healthCheck.CheckHealthAsync(new HealthCheckContext(), cancellationToken);
                if (result.Status != HealthStatus.Healthy)
                    return Problem(result.Exception.GetBaseException().Message, null, (int)HttpStatusCode.InternalServerError);
            }

            return Ok("Success");
        }
    }
}
