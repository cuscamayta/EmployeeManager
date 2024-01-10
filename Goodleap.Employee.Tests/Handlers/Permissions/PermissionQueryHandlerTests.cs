using Goodleap.Employee.Api.Business.Queries.Permissions;
using Goodleap.Employee.Core.Models;
using Goodleap.Employee.Core.Units;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goodleap.Employee.Tests.Handlers.Permissions
{
    public class PermissionQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task Handle_Returns_PermissionsList()
        {
            // Arrange
            var permissions = new List<Permission>
            {
                new Permission { Id = Guid.NewGuid(), Name = "Permission 1", TypeId = Guid.NewGuid() },
                new Permission { Id = Guid.NewGuid(), Name = "Permission 2", TypeId = Guid.NewGuid() }
            };

            _unitOfWorkMock
                .Setup(uow => uow.permissionRepository.GetAllPermissionsAsync())
                .ReturnsAsync(permissions);

            var handler = new GetAllPermissionsQueryHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(permissions.Count, result.Count);
        }

        [Fact]
        public async Task Handle_Returns_EmptyList_WhenNoPermissions()
        {
            // Arrange
            var emptyPermissions = new List<Permission>();
            _unitOfWorkMock
                .Setup(uow => uow.permissionRepository.GetAllPermissionsAsync())
                .ReturnsAsync(emptyPermissions);

            var handler = new GetAllPermissionsQueryHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_Returns_Null_WhenUnitOfWorkReturnsNull()
        {
            // Arrange
            _unitOfWorkMock
                .Setup(uow => uow.permissionRepository.GetAllPermissionsAsync())
                .ReturnsAsync((List<Permission>)null);

            var handler = new GetAllPermissionsQueryHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_Throws_Exception_WhenUnitOfWorkThrows()
        {
            // Arrange
            _unitOfWorkMock
                .Setup(uow => uow.permissionRepository.GetAllPermissionsAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var handler = new GetAllPermissionsQueryHandler(_unitOfWorkMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);
            });
        }
    }
}
