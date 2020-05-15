using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pi.Api.Model;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Pi.Api.Processors
{
    public class AciProcessor : IProcessor
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public AciProcessor(IConfiguration config, ILogger<AciProcessor> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<Job> SubmitJob(Job job)
        {
            var azure = Connect();

            var baseDefinition = azure.ContainerGroups.Define(job.Id)
                    .WithRegion(_config["Azure:Region"])
                    .WithExistingResourceGroup(_config["Azure:ResourceGroup"]);

            var osDefinition = _config["Pi:Docker:OSPlatform"] == nameof(OSPlatform.Windows) ?
                               baseDefinition.WithWindows() : 
                               baseDefinition.WithLinux();

            await osDefinition
                    .WithPublicImageRegistryOnly()
                    .WithoutVolume()
                    .DefineContainerInstance("pi")
                        .WithImage(_config["Pi:Docker:Image"])
                        .WithoutPorts()
                        .WithCpuCoreCount(4)
                        .WithMemorySizeInGB(16)
                        .WithStartingCommandLine($"/app/Pi.Runtime.NetFx -dp {job.DecimalPlaces}")
                        .Attach()
                    .WithRestartPolicy(ContainerGroupRestartPolicy.OnFailure)
                    .CreateAsync();

            job.ProcessingId = job.Id;
            return job;
        }

        private IAzure Connect()
        {
            // requires Service Principal creds
            // generate with `az ad sp create-for-rbac`
            // for other auth options see: https://github.com/Azure/azure-libraries-for-net/blob/master/AUTH.md

            var creds = new AzureCredentialsFactory().FromServicePrincipal(
                _config["Azure:ServicePrincipal:ClientId"],
                _config["Azure:ServicePrincipal:ClientSecret"],
                _config["Azure:ServicePrincipal:TenantId"],
                AzureEnvironment.AzureGlobalCloud);

            return Azure.Authenticate(creds).WithSubscription(_config["Azure:ServicePrincipal:SubscriptionId"]);
        }
    }
}
