using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pi.Api.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.Api.Processors
{
    public class DockerProcessor : IProcessor
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public DockerProcessor(IConfiguration config, ILogger<AciProcessor> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<Job> SubmitJob(Job job)
        {
                       
            var parameters = new CreateContainerParameters()
            {
                Name = job.Id,
                Image = _config["Pi:Docker:Image"],
                Cmd = new[] 
                { 
                    "-dp", 
                    job.DecimalPlaces.ToString() 
                }
            };

            var client = Connect();
            var container = await client.Containers.CreateContainerAsync(parameters);
            await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());

            job.ProcessingId = container.ID;
            return job;
        }

        private DockerClient Connect()
        {
            return new DockerClientConfiguration(new Uri(_config["Pi:Processors:Docker:Api"])).CreateClient();
        }
    }
}
