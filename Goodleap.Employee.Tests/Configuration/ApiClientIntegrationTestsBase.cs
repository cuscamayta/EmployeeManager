using Goodleap.Employee.Api;
using Goodleap.Employee.Tests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Goodleap.Employee.Tests.Configuration
{
    public abstract class ApiClientIntegrationTestsBase : WebApplicationFactory<Startup>
    {
        protected ApiClientIntegrationTestsBase()
        {
            Client = CreateClient();

            var scope = Server.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            Configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            JsonSerializerOptions = new JsonSerializerOptions().Setup();
        }

        public IConfiguration Configuration { get; }

        public HttpClient Client { get; }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Client?.Dispose();
        }
    }
}
