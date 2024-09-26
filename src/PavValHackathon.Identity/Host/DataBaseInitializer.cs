using System.Reflection;
using GodelTech.Microservices.Core;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PavValHackathon.Identity.Data;
using PavValHackathon.Identity.Domain;
using PavValHackathon.Identity.Utils;

namespace PavValHackathon.Identity.Host
{
    public class DataBaseInitializer : MicroserviceInitializerBase
    {
        public DataBaseInitializer(IConfiguration configuration) 
            : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            string connectionString = Configuration.GetMasterConnectionString();
            
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var applicationServices = app.ApplicationServices;
            var serviceScopeFactory = applicationServices.GetService<IServiceScopeFactory>();

            using var scope = serviceScopeFactory!.CreateScope();

            var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            DataMigrationHelper.Migrate(persistedGrantDbContext);
            persistedGrantDbContext.SaveChanges();
            
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            DataMigrationHelper.Migrate(applicationDbContext);
            DataMigrationHelper.Migrate(applicationDbContext.Roles);
            applicationDbContext.SaveChanges();
            
            var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            DataMigrationHelper.Migrate(configurationDbContext);
            DataMigrationHelper.Migrate(configurationDbContext.Clients);
            DataMigrationHelper.Migrate(configurationDbContext.ApiScopes);
            DataMigrationHelper.Migrate(configurationDbContext.IdentityResources);
            configurationDbContext.SaveChanges();
        }
    }
}