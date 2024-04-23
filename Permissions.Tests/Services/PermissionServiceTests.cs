using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Permissions.API.Services;
using Permissions.API.DTOs;
using Permissions.Infrastructure.Repositories;
using Permissions.Infrastructure.Models;

namespace Permissions.UnitTests.Services
{
    public class PermissionServiceTests
    {
        [Fact]
        public async Task GetAllPermissionsAsync_Returns_PermissionDTOs()
        {
            // Arrange
            var mockRepository = new Mock<IPermissionRepository>();
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, EmployeeForename = "John", EmployeeSurname = "Doe", PermissionDate = DateTime.Now.Date },
                new Permission { Id = 2, EmployeeForename = "Jane", EmployeeSurname = "Doe", PermissionDate = DateTime.Now.Date }
            };
            mockRepository.Setup(repo => repo.GetAllPermissionsAsync()).ReturnsAsync(permissions);
            var service = new PermissionService(mockRepository.Object);

            // Act
            var result = await service.GetAllPermissionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            foreach (var permissionDTO in result)
            {
                Assert.IsType<PermissionDTO>(permissionDTO);
            }
        }

        [Fact]
        public async Task GetPermissionByIdAsync_Returns_Null_For_NonExistent_Permission()
        {
            // Arrange
            var mockRepository = new Mock<IPermissionRepository>();
            mockRepository.Setup(repo => repo.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync((Permission)null);
            var service = new PermissionService(mockRepository.Object);

            // Act
            var result = await service.GetPermissionByIdAsync(123);

            // Assert
            Assert.Null(result);
        }

        // You can add more unit tests for other methods of PermissionService
    }
}
