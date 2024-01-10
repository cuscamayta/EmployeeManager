using Goodleap.Employee.Api.Business.DomainEvents.Publishers;
using Goodleap.Employee.Api.Configuration;
using Goodleap.Employee.Core.EmployeePermissions;
using Goodleap.Employee.Core.Models;
using Goodleap.Employee.Core.Permissions;
using Goodleap.Employee.Core.Units;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Goodleap.Employee.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<EmployeeDbContext>(options =>
                options.UseSqlServer(configuration["ConnectionStrings:EmployeeDb"]));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IEmployeePermissionRepository, EmployeePermissionRepository>();
            services.AddTransient<IPublishService, PublishService>();

            services.AddMediatR(typeof(Program).Assembly);
            services.AddElasticSearch(configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
