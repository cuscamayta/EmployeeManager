using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goodleap.Employee.Tests.Configuration
{
    public static class ConfigurationManager
    {
        /// <summary>
        /// Load configuration.
        /// </summary>
        /// <param name="environmentName">Environment name.</param>
        /// <returns>Configuration.</returns>
        public static IConfiguration Get(string environmentName)
        {
            var basePath = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
