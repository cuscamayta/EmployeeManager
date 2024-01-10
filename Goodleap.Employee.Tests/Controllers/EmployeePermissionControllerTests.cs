using FluentAssertions;
using Goodleap.Employee.Api.DTOs;
using Goodleap.Employee.Tests.Configuration;
using Goodleap.Employee.Tests.Helpers;
using System.Net;

namespace Goodleap.Employee.Tests.Controllers
{
    public class EmployeePermissionControllerTests : IntegrationTestsBase
    {

        [Fact]
        public async Task GetAllEmployeePermission_ReturnsSuccessStatusCode()
        {
            var response = await Client.GetAsync($"EmployeePermission/all");
            var jsonResult = await response.Content.ReadAsStringAsync();

            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateEmployeePermission_ReturnsSuccessStatusCode()
        {
            var permissionType = new Core.Models.PermissionType()
            {
                Id = new Guid(),
                Name = "Write"
            };
            DbContext.PermissionTypes.Add(permissionType);

            var permission = new Core.Models.Permission()
            {
                Id = new Guid(),
                Name = "Permission1",
                TypeId = permissionType.Id
            };

            DbContext.Permissions.Add(permission);

            var employee = new Core.Models.Employee()
            {
                Id = new Guid(),
                Name = "Jhon",
                LastName = "Deer"
            };

            DbContext.Employees.Add(employee);

            DbContext.SaveChanges();


            var updateDto = new UpdatePermissionDto()
            {
                EmployeeId = employee.Id,
                Permissions = new List<Guid> { permission.Id }
            }; // Create a valid UpdatePermissionDto for testing
            var response = await Client.PatchAsync("EmployeePermission/update", ContentHelper.GetStringContent(updateDto));
            var jsonResult = await response.Content.ReadAsStringAsync();

            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateEmployeePermission_WithInvalidData_ReturnsBadRequest()
        {           
            var updateDto = new UpdatePermissionDto(); // Create a valid UpdatePermissionDto for testing
            var response = await Client.PatchAsync("EmployeePermission/update", ContentHelper.GetStringContent(updateDto));
            var jsonResult = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAllEmployeePermission_ReturnsExpectedMediaType()
        {
            var permissionType = new Core.Models.PermissionType()
            {
                Id = new Guid(),
                Name = "Write"
            };
            DbContext.PermissionTypes.Add(permissionType);

            var permission = new Core.Models.Permission()
            {
                Id = new Guid(),
                Name = "Permission1",
                TypeId = permissionType.Id
            };

            DbContext.Permissions.Add(permission);

            var employee = new Core.Models.Employee()
            {
                Id = new Guid(),
                Name = "Jhon",
                LastName = "Deer"
            };

            DbContext.Employees.Add(employee);

            DbContext.SaveChanges();
            var updateDto = new UpdatePermissionDto()
            {
                EmployeeId = employee.Id,
                Permissions = new List<Guid> { permission.Id }
            }; // Create a valid UpdatePermissionDto for testing
            var response = await Client.PatchAsync("EmployeePermission/update", ContentHelper.GetStringContent(updateDto));

            // Assert
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }
    }
}
