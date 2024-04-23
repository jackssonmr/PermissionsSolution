
using Permissions.API.DTOs;

namespace Permissions.API.Services
{
    public interface IElasticsearchService
    {
        void LogOperation(string operationName, PermissionDTO permissionDto);
    }
}
