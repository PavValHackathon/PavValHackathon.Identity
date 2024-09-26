using Autofac;
using Microsoft.AspNetCore.Identity;
using PavValHackathon.Identity.Domain;
using PavValHackathon.Identity.v1.Factories;

namespace PavValHackathon.Identity.Modules
{
    public class IdentityServerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationUserPrincipalFactory>()
                .As<IUserClaimsPrincipalFactory<ApplicationUser>>();
        }
    }
}