using System.Reflection;
using GodelTech.Microservices.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PavValHackathon.Identity.Domain;
using PavValHackathon.Identity.Utils;
using PavValHackathon.Identity.v1.Factories;

namespace PavValHackathon.Identity.Host
{
    public class IdentityServerInitializer : MicroserviceInitializerBase
    {
        public IdentityServerInitializer(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIdentityServer();
            app.UseAuthentication();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserPrincipalFactory>();
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            var connectionString = Configuration.GetMasterConnectionString();
            
            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseSqlServer(connectionString, o => o.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = dbContextOptionsBuilder => dbContextOptionsBuilder.UseSqlServer(connectionString, o => o.MigrationsAssembly(migrationsAssembly));
                })
                .AddDeveloperSigningCredential();
        }
    }
}