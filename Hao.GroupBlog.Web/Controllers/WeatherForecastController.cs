using Hao.GroupBlog.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hao.GroupBlog.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Get([FromServices] IConfiguration cfg)
        {
            var name = cfg[CfgConsts.PLATFORM_MY_APP_NAME];
            return Ok(new { name });
        }
    }
}