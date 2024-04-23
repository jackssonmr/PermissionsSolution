using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Permissions.Infrastructure.Models;

namespace Permissions.Infrastructure.Repositories
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task<Permission> GetPermissionByIdAsync(int id);
        Task<IEnumerable<Permission>> GetPermissionsByEmployeeAsync(string forename, string surname);
        Task<IEnumerable<Permission>> GetPermissionsByTypeAsync(int typeId);
        Task<IEnumerable<Permission>> GetPermissionsByDateAsync(DateTime date);
        Task AddPermissionAsync(Permission permission);
        Task UpdatePermissionAsync(Permission permission);
        Task DeletePermissionAsync(int id);
        Task SaveChangesAsync();
    }
}
