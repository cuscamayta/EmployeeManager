using FluentAssertions;
using Goodleap.Employee.Api.Business.Queries.Permissions;
using Goodleap.Employee.Api.Controllers;
using Goodleap.Employee.Core.Models;
using Goodleap.Employee.Tests.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Goodleap.Employee.Tests.Controllers
{
    public class PermissionControllerIntegrationTests : IntegrationTestsBase
    {
        [Fact]
        public async Task GetAllPermission_Returns_OK()
        {
            var response = await Client.GetAsync($"permission/all");
            var jsonResult = await response.Content.ReadAsStringAsync();

            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }

}
