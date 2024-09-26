using System;
using Microsoft.Extensions.Configuration;

namespace PavValHackathon.Identity.Utils
{
    public static class ConfigurationExtensions
    {
        public static string GetMasterConnectionString(this IConfiguration configuration)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}