using Goodleap.Employee.Api.Business.DomainEvents.Publishers;
using Goodleap.Employee.Core.Models;
using Goodleap.Employee.Tests.Publisher;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goodleap.Employee.Tests.Helpers
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection RemoveServices(this IServiceCollection services, params Type[] serviceTypes)
        {
            foreach (var descriptor in services
                .Where(descriptor => serviceTypes
                    .Any(serviceType => descriptor.ServiceType == serviceType))
                .ToList())
            {
                services.Remove(descriptor);
            }

            return services;
        }

        public static IServiceCollection AddTestDbContextServices(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<EmployeeDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
        }
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services
                    .AddHttpClient("test", httpClient =>
                    {
                        httpClient.BaseAddress = new Uri("http://teset.com");
                        httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "test");
                    });

            return services
                .AddScoped<IPublishService, PublishFakeService>();
        }

    }
}
