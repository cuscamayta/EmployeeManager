using System.Reflection;
using Goodleap.Employee.Api.Business.DomainEvents.Publishers;
using Goodleap.Employee.Api.Configuration;
using Goodleap.Employee.Api.Services.ElasticSearch;
using Goodleap.Employee.Core.EmployeePermissions;
using Goodleap.Employee.Core.Models;
using Goodleap.Employee.Core.Permissions;
using Goodleap.Employee.Core.Units;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

using Serilog.Sinks.Elasticsearch;

namespace Goodleap.Employee.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            var builder = WebApplication.CreateBuilder(args);


            builder.Host.UseSerilog();
            ConfigureLogging(builder, configuration, environment);

            
            var startup = new Startup();
            startup.ConfigureServices(builder.Services, configuration);
            var app = builder.Build();
            startup.Configure(app, app.Environment);
            app.Run();
        }

        private static void ConfigureLogging(WebApplicationBuilder builder, IConfigurationRoot configuration, string environment)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                NumberOfReplicas = 1,
                NumberOfShards = 2
            };
        }

    }
}