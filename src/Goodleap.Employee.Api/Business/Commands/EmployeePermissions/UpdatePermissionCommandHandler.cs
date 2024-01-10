﻿using Goodleap.Employee.Api.Business.DomainEvents.Publishers;
using Goodleap.Employee.Api.Business.Queries.EmployeePermissions;
using Goodleap.Employee.Api.DTOs;
using Goodleap.Employee.Core.Models;
using Goodleap.Employee.Core.Units;
using MediatR;

namespace Goodleap.Employee.Api.Business.Commands.EmployeePermissions
{ 
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, UpdatePermissionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishService _publishService;
        private readonly ILogger<UpdatePermissionCommandHandler> _logger;

        public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, IPublishService publishService, ILogger<UpdatePermissionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _publishService = publishService;
            _logger = logger;
        }

        public async Task<UpdatePermissionDto> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{this.GetType().Name} - Saving Information");

            var updatePermissions = request.UpdatePermissionDto;

            var activePermission = await _unitOfWork.employeePermissionRepository
                .GetEmployeePermissionByEmployeeIdAsync(updatePermissions.EmployeeId);

            var employeePermissionDelete = new List<EmployeePermission>();

            var permissionIdActives = activePermission.Select(permission => permission.PermissionId).ToList();

            var permissionCommon = updatePermissions.Permissions.Intersect(permissionIdActives).ToList();

            foreach(var permission in updatePermissions.Permissions)
            {
                if (!permissionCommon.Contains(permission))
                {
                    var employeePermission = new EmployeePermission
                    {
                        EmployeeId = updatePermissions.EmployeeId,
                        PermissionId = permission
                    };

                    _unitOfWork.employeePermissionRepository.AddEmployeePermissionAsync(employeePermission);
                }
            }

            foreach (var permission in activePermission)
            {
                if (!permissionCommon.Contains(permission.PermissionId))
                {
                    _unitOfWork.employeePermissionRepository.DeleteEmployeePermission(permission);
                }
            }
            
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"{this.GetType().Name} - Publishing Information to kafka");
            await _publishService.Publish<UpdatePermissionDto>("employee-permission", updatePermissions);

            return updatePermissions;
        }
    }
}