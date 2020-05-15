using Microsoft.AspNetCore.Mvc;
using Pi.Api.Model;
using Pi.Api.Processors;
using System.Threading.Tasks;

namespace Pi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {

        private readonly IProcessor _processor;

        public JobsController(IProcessor processor)
        {
            _processor = processor;
        }     

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Job job)
        {
            var newJob = await _processor.SubmitJob(job);
            return Created("", newJob);
        }
    }
}
