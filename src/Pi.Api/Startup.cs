using k8s;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pi.Api.Processors;
using System;

namespace Pi.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            AddProcessors(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddProcessors(IServiceCollection services)
        {
            switch (Configuration["Pi:Processor"])
            {
                case nameof(AciProcessor):
                    services.AddSingleton<IProcessor, AciProcessor>();
                    break;
                case nameof(DockerProcessor):
                    services.AddSingleton<IProcessor, DockerProcessor>();
                    break;
                case nameof(KubernetesProcessor):
                    switch (Configuration["Pi:Processors:Kubernetes:Configuration"])
                    {
                        case "InCluster":
                            services.AddTransient(c => new Kubernetes(KubernetesClientConfiguration.InClusterConfig()));
                            break;
                        default:
                            services.AddTransient(c => new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile()));
                            break;
                    }
                    services.AddTransient<IProcessor, KubernetesProcessor>();
                    break;
                default:
                    throw new Exception($"Unknown processor type: {Configuration["Pi:Processor"]}");
            }
        }
    }
}
