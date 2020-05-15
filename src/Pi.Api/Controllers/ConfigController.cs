using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Pi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ConfigController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (_config.GetValue<bool>("ConfigController:Enabled"))
            {
                return new JsonResult(_config.AsEnumerable());
            }
            else
            {
                return NotFound();
            }
        }
    }
}