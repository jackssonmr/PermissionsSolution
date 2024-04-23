using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Permissions.API.DTOs;
using Permissions.Infrastructure.Kafka;
using Permissions.Infrastructure.Models;
using Permissions.Infrastructure.Repositories;

namespace Permissions.API.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllPermissionsAsync();
            // Mapear las entidades Permission a DTOs PermissionDTO
            // Esto depende de cómo tengas configurado el mapeo en tu proyecto
            var permissionDTOs = MapPermissionsToDTOs(permissions);
            return permissionDTOs;
        }

        public async Task<PermissionDTO> GetPermissionByIdAsync(int id)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(id);
            if (permission == null)
            {
                // Manejar el caso en el que no se encuentre el permiso con el ID dado
                return null;
            }
            // Mapear la entidad Permission a un DTO PermissionDTO
            // Esto depende de cómo tengas configurado el mapeo en tu proyecto
            var permissionDTO = MapPermissionToDTO(permission);
            return permissionDTO;
        }

        public async Task AddPermissionAsync(PermissionDTO permissionDTO)
        {
            // Mapear el DTO PermissionDTO a una entidad Permission
            // Esto depende de cómo tengas configurado el mapeo en tu proyecto
            var permission = MapDTOToPermission(permissionDTO);
            // Llamar al método correspondiente en el repositorio para agregar el permiso
            await _permissionRepository.AddPermissionAsync(permission);
        }

        public async Task UpdatePermissionAsync(int id, PermissionDTO permissionDTO)
        {
            var existingPermission = await _permissionRepository.GetPermissionByIdAsync(id);
            if (existingPermission == null)
            {
                // Manejar el caso en el que no se encuentre el permiso con el ID dado
                return;
            }
            // Mapear el DTO PermissionDTO a la entidad Permission existente
            // Esto depende de cómo tengas configurado el mapeo en tu proyecto
            MapDTOToPermission(permissionDTO, existingPermission);
            // Llamar al método correspondiente en el repositorio para actualizar el permiso
            await _permissionRepository.UpdatePermissionAsync(existingPermission);
        }

        public async Task DeletePermissionAsync(int id)
        {
            // Llamar al método correspondiente en el repositorio para eliminar el permiso
            await _permissionRepository.DeletePermissionAsync(id);
        }

        // Otros métodos auxiliares
        private PermissionDTO MapPermissionToDTO(Permission permission)
        {
            if (permission == null) {
                return null;
            }
        
            var permissionDTO = new PermissionDTO
            {
                Id = permission.Id,
                EmployeeSurname = permission.EmployeeSurname,
                EmployeeForename = permission.EmployeeForename,
                PermissionDate = permission.PermissionDate,
                PermissionTypeId = permission.PermissionTypeId
            };
        
            return permissionDTO;
        }
        
        private IEnumerable<PermissionDTO> MapPermissionsToDTOs(IEnumerable<Permission> permissions)
        {
            List<PermissionDTO> permissionDTOs = new List<PermissionDTO>();
        
            foreach (var permission in permissions)
            {
                PermissionDTO permissionDTO = new PermissionDTO
                {
                    Id = permission.Id,
                    EmployeeSurname = permission.EmployeeSurname,
                    EmployeeForename = permission.EmployeeForename,
                    PermissionDate = permission.PermissionDate,
                    PermissionTypeId = permission.PermissionTypeId
                };
        
                permissionDTOs.Add(permissionDTO);
            }
        
            return permissionDTOs;
        }
        
        private Permission MapDTOToPermission(PermissionDTO permissionDTO)
        {
            return new Permission
            {
                Id = permissionDTO.Id, 
                EmployeeSurname = permissionDTO.EmployeeSurname,
                EmployeeForename = permissionDTO.EmployeeForename,
                PermissionDate = permissionDTO.PermissionDate,
                PermissionTypeId = permissionDTO.PermissionTypeId
            };
        }

        private Permission MapDTOToPermission(PermissionDTO permissionDTO, Permission existingPermission = null)
        {
            if (existingPermission == null) {
                existingPermission = new Permission();
            }

            existingPermission.EmployeeSurname = permissionDTO.EmployeeSurname;
            existingPermission.EmployeeForename = permissionDTO.EmployeeForename;
            existingPermission.PermissionDate = permissionDTO.PermissionDate;
            existingPermission.PermissionTypeId = permissionDTO.PermissionTypeId;
            
            return existingPermission;
        }

    }
}
