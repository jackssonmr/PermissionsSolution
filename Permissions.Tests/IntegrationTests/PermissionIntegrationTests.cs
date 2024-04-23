using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Permissions.API.DTOs;
using Newtonsoft.Json;

namespace Permissions.Tests.IntegrationTests
{
    public class PermissionIntegrationTests : IDisposable
    {
        private readonly HttpClient _client;

        public PermissionIntegrationTests()
        {
            // Configurar el cliente HTTP con la URL base de la API
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080") // Reemplazar con la URL correcta
            };
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [Fact]
        public async Task Can_Add_Permission()
        {
            // Arrange
            var permission = new PermissionDTO
            {
                EmployeeForename = "John",
                EmployeeSurname = "Doe",
                PermissionTypeId = 1,
                PermissionDate = DateTime.Now.Date
            };
            var content = new StringContent(JsonConvert.SerializeObject(permission), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/permissions", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Permission added successfully", responseContent);
        }

        [Fact]
        public async Task Can_Get_Permissions()
        {
            // Act
            var response = await _client.GetAsync("/api/permissions");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseContent);
        }
    }
}
