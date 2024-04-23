using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Permissions.API.Controllers;
using Permissions.API.DTOs;
using Permissions.API.Services;
using Permissions.Infrastructure.Services;

namespace Permissions.UnitTests.Controllers
{
    public class PermissionControllerTests
    {
        [Fact]
        public async Task GetAllPermissions_Returns_OkObjectResult_With_PermissionDTOs()
        {
            // Arrange
            var mockPermissionService = new Mock<IPermissionService>();
            var mockElasticsearchService = new Mock<IElasticsearchService>();
            var mockKafkaProducerService = new Mock<IKafkaProducerService>();

            var permissions = new List<PermissionDTO>
            {
                new PermissionDTO { Id = 1, EmployeeForename = "John", EmployeeSurname = "Doe", PermissionTypeId = 1, PermissionDate = DateTime.Now.Date },
                new PermissionDTO { Id = 2, EmployeeForename = "Jane", EmployeeSurname = "Doe", PermissionTypeId = 2, PermissionDate = DateTime.Now.Date }
            };

            mockPermissionService.Setup(service => service.GetAllPermissionsAsync()).ReturnsAsync(permissions);

            var controller = new PermissionController(mockPermissionService.Object, mockElasticsearchService.Object, mockKafkaProducerService.Object);

            // Act
            var result = await controller.GetAllPermissions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PermissionDTO>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetPermissionById_Returns_OkObjectResult_With_PermissionDTO()
        {
            // Arrange
            var mockPermissionService = new Mock<IPermissionService>();
            var mockElasticsearchService = new Mock<IElasticsearchService>();
            var mockKafkaProducerService = new Mock<IKafkaProducerService>();

            var permission = new PermissionDTO { Id = 1, EmployeeForename = "John", EmployeeSurname = "Doe", PermissionTypeId = 1, PermissionDate = DateTime.Now.Date };

            mockPermissionService.Setup(service => service.GetPermissionByIdAsync(It.IsAny<int>())).ReturnsAsync(permission);

            var controller = new PermissionController(mockPermissionService.Object, mockElasticsearchService.Object, mockKafkaProducerService.Object);

            // Act
            var result = await controller.GetPermissionById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<PermissionDTO>(okResult.Value);
            Assert.Equal(permission.Id, model.Id);
        }

        // You can add more unit tests for other action methods of PermissionController
    }
}