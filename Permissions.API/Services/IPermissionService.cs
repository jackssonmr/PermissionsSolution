using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Permissions.API.DTOs;

namespace Permissions.API.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync();
        Task<PermissionDTO> GetPermissionByIdAsync(int id);
        Task AddPermissionAsync(PermissionDTO permissionDTO);
        Task UpdatePermissionAsync(int id, PermissionDTO permissionDTO);
        Task DeletePermissionAsync(int id);
    }
}
