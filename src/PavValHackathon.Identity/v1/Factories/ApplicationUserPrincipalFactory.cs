using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PavValHackathon.Identity.Domain;

namespace PavValHackathon.Identity.v1.Factories
{
    public class ApplicationUserPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserPrincipalFactory(
            UserManager<ApplicationUser> userManager, 
            IOptions<IdentityOptions> optionsAccessor) 
            : base(userManager, optionsAccessor)
        {
        }
    }
}