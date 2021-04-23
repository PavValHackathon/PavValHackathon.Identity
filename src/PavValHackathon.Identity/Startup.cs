using System.Collections.Generic;
using Autofac;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.HealthChecks;
using GodelTech.Microservices.Core.Mvc;
using GodelTech.Microservices.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PavValHackathon.Identity.Host;
using PavValHackathon.Identity.Modules;

namespace PavValHackathon.Identity
{
    public class Startup : MicroserviceStartup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<IdentityServerModule>();
        }
        
        protected override IEnumerable<IMicroserviceInitializer> CreateInitializers()
        {
            yield return new DeveloperExceptionPageInitializer(Configuration);
            yield return new HttpsInitializer(Configuration);

            yield return new GenericInitializer((app, env) => app.UseRouting());
            
            yield return new IdentityServerInitializer(Configuration);
            yield return new DataBaseInitializer(Configuration);
            yield return new SwaggerInitializer(Configuration);
            yield return new ApiInitializer(Configuration);

            yield return new HealthCheckInitializer(Configuration);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            base.ConfigureServices(services);
        }
    }
}
