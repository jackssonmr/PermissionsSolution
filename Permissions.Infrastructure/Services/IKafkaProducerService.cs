using Permissions.API.DTOs;

namespace Permissions.Infrastructure.Services
{
    public interface IKafkaProducerService
    {
        Task ProducePermissionMessageAsync(PermissionDTO permissionDTO, string operation);
    }
}
