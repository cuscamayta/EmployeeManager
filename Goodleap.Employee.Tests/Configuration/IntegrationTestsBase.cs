using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Goodleap.Employee.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.TestHost;
using Goodleap.Employee.Tests.Helpers;

namespace Goodleap.Employee.Tests.Configuration
{
    public abstract class IntegrationTestsBase : ApiClientIntegrationTestsBase
    {
        private string _connectionString = string.Empty;

        protected IntegrationTestsBase()
            : base()
        {
            var scope = Server.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            DbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
            DbContext.Database.EnsureCreated();

        }

        public EmployeeDbContext DbContext { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = ConfigurationManager.Get(environment ?? "Development");

            var connectionString = configuration.GetConnectionString("EmployeeDb");
            var masterConnectionString = connectionString.Replace("Initial Catalog=Employees;", string.Empty);
            var databaseName = $"EmployeeDb_{Guid.NewGuid()}";
            _connectionString = $"{masterConnectionString};Initial Catalog={databaseName};TrustServerCertificate=True;";

            builder.ConfigureTestServices(services =>
                services
                    .RemoveServices(
                        typeof(DbContextOptions),
                        typeof(DbContextOptions<EmployeeDbContext>))
                    .AddBusinessServices()
                    .AddTestDbContextServices(_connectionString));
        }

        protected EmployeeDbContext CreateEmployeeDbContext()
        {
            return new EmployeeDbContext(
                new DbContextOptionsBuilder<EmployeeDbContext>().UseSqlServer(_connectionString).Options);
        }

        protected override void Dispose(bool disposing)
        {
            var employeeDbContext = CreateEmployeeDbContext();
            employeeDbContext.Database.EnsureDeleted();

            base.Dispose(disposing);

            employeeDbContext.Dispose();

        }
    }
}
