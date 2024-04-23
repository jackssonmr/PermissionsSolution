using System;
using System.Threading.Tasks;
using Permissions.Infrastructure.Repositories;

namespace Permissions.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPermissionRepository PermissionRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
