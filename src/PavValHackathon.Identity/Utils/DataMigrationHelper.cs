using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PavValHackathon.Identity.Utils
{
    public static class DataMigrationHelper
    {
        public static void Migrate(DbSet<ApiScope> dbSet)
        {
            var apiScopeIds = dbSet.Select(p => p.Name).ToHashSet();
            var missingScopes = Config.ApiScopes.Where(p => !apiScopeIds.Contains(p.Name));

            foreach (var apiScope in missingScopes)
                dbSet.Add(apiScope.ToEntity());
        }
        
        public static void Migrate(DbSet<IdentityResource> dbSet)
        {
            var resourcesIds = dbSet.Select(p => p.Name).ToHashSet();
            var missingResources = Config.IdentityResources.Where(p => !resourcesIds.Contains(p.Name));

            foreach (var resource in missingResources)
                dbSet.Add(resource.ToEntity());
        }


        public static void Migrate(DbSet<Client> dbSet)
        {
            var clientIds = dbSet.Select(p => p.ClientId).ToHashSet();
            var missingClients = Config.Clients.Where(p => !clientIds.Contains(p.ClientId));
            
            foreach (var client in missingClients)
                dbSet.Add(client.ToEntity());
        }
        
        public static void Migrate(DbSet<IdentityRole> dbSet)
        {
            var roles = dbSet.Select(p => p.Name).ToHashSet();
            var missingRoles  = Config.Roles.All.Where(p => !roles.Contains(p));

            foreach (var missingRole in missingRoles)
            {
                var identityRole = new IdentityRole(missingRole) {NormalizedName = missingRole.ToUpper()};
                dbSet.Add(identityRole);
            }
        }

        public static void Migrate(DbContext context) => context.Database.Migrate();
    }
}