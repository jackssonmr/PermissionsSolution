using System;
using System.Threading.Tasks;
using Permissions.Infrastructure.Data.Contexts;
using Permissions.Infrastructure.Repositories;

namespace Permissions.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IPermissionRepository _permissionRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IPermissionRepository PermissionRepository
        {
            get
            {
                return _permissionRepository ??= new PermissionRepository(_context);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar excepciones o realizar algún registro de errores
                throw ex;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

/*
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Permissions = new Repository<Permission>(_context);
        PermissionTypes = new Repository<PermissionType>(_context);
    }

    public IRepository<Permission> Permissions { get; }
    public IRepository<PermissionType> PermissionTypes { get; }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
*/