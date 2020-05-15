using k8s;
using k8s.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pi.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pi.Api.Processors
{
    public class KubernetesProcessor : IProcessor
    {
        private readonly Kubernetes _kubernetes;
        private readonly string _namespace = "default";

        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public KubernetesProcessor(Kubernetes kubernetes, IConfiguration config, ILogger<AciProcessor> logger)
        {
            _kubernetes = kubernetes;
            _config = config;
            _logger = logger;
        }

        public async Task<Job> SubmitJob(Job job)
        {
            var resources = new V1ResourceRequirements
            {
                Requests = new Dictionary<string, ResourceQuantity>()
                {
                    { "cpu", new ResourceQuantity("1") },
                    { "memory", new ResourceQuantity($"1Gi")}
                }
            };

            var podSpec = new V1PodSpec
            {
                Containers = new[]
                {
                    new V1Container
                    {
                        Name = job.Id,
                        Image = _config["Pi:Docker:Image"],
                        Command = new []
                        {
                            "/app/Pi.Runtime.NetFx",
                            "-dp",
                            job.DecimalPlaces.ToString()
                        },
                        Resources = resources
                    }
                },
                RestartPolicy = "Never"
            };

            //for running in AKS with ACI integration:
            if (_config.GetValue<bool>("Pi:Processors:Kubernetes:UseAci"))
            {
                AddAciConfiguration(podSpec);
            }

            var jobSpec = new V1JobSpec()
            {
                Template = new V1PodTemplateSpec(spec: podSpec)
            };

            var jobMetadata = new V1ObjectMeta(name: job.Id)
            {
                Labels = new Dictionary<string, string>()
                {
                    { "com.pi", "1"}                    
                }
            };

            var k8sJob = new V1Job(metadata: jobMetadata, spec: jobSpec);
            
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("*** Generated YAML: ***");
                var yaml = Yaml.SaveToString(k8sJob);
                _logger.LogDebug(yaml);
                _logger.LogDebug("---");
            }

            await _kubernetes.CreateNamespacedJobAsync(k8sJob, _namespace);
            job.ProcessingId = k8sJob.Metadata.Name;
            return job;
        }

        private void AddAciConfiguration(V1PodSpec podSpec)
        {
            //bursting out to ACI is controlled by node selectors and tolerations:
            podSpec.NodeSelector = new Dictionary<string, string>
            {
                { "beta.kubernetes.io/os", _config["Pi:Docker:OSPlatform"].ToLower() },
                { "kubernetes.io/role",  "agent" },
                { "type", "virtual-kubelet" }
            };

            podSpec.Tolerations = new List<V1Toleration>
            {
                new V1Toleration
                {
                    Key = "virtual-kubelet.io/provider",
                    OperatorProperty = "Exists"
                },
                new V1Toleration
                {
                    Key = "azure.com/aci",
                    Effect = "NoSchedule"
                }
            };
        }
    }
}