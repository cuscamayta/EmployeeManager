﻿using Goodleap.Employee.Api.DTOs;
using MediatR;

namespace Goodleap.Employee.Api.Business.Queries.EmployeePermissions;

public record GetAllEmployeePermissionQuery : IRequest<List<UpdatePermissionDto>>;
