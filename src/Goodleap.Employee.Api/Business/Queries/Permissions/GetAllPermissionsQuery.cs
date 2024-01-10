using Goodleap.Employee.Core.Models;
using MediatR;

namespace Goodleap.Employee.Api.Business.Queries.Permissions;

public record GetAllPermissionsQuery : IRequest<List<Permission>>;
