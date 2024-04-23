using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Permissions.Infrastructure.Data.Contexts;
using Permissions.Infrastructure.Models;

namespace Permissions.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<Permission> GetPermissionByIdAsync(int id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByEmployeeAsync(string forename, string surname)
        {
            return await _context.Permissions
                .Where(p => p.EmployeeForename == forename && p.EmployeeSurname == surname)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByTypeAsync(int typeId)
        {
            return await _context.Permissions
                .Where(p => p.PermissionTypeId == typeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByDateAsync(DateTime date)
        {
            return await _context.Permissions
                .Where(p => p.PermissionDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task AddPermissionAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            _context.Entry(permission).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
