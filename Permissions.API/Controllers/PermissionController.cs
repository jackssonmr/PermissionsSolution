using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Permissions.API.DTOs;
using Permissions.API.Services;
using Permissions.Infrastructure.Services;

namespace Permissions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IElasticsearchService _elasticsearchService;
        private readonly IKafkaProducerService _producerService;

        public PermissionController(
            IPermissionService permissionService, IElasticsearchService elasticsearchService, IKafkaProducerService producerService)
        {
            _permissionService = permissionService;
            _elasticsearchService = elasticsearchService;
            _producerService = producerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDTO>>> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDTO>> GetPermissionById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null) {
                return NotFound();
            }
            _elasticsearchService.LogOperation("GetPermissionById", permission);
            _producerService.ProducePermissionMessageAsync(permission, "GET");
            return Ok(permission);
        }

        [HttpPost]
        public async Task<ActionResult> AddPermission([FromBody] PermissionDTO permissionDTO)
        {
            await _permissionService.AddPermissionAsync(permissionDTO);
            _elasticsearchService.LogOperation("AddPermission", permissionDTO);
            _producerService.ProducePermissionMessageAsync(permissionDTO, "ADD");
            return CreatedAtAction(nameof(GetPermissionById), new { id = permissionDTO.Id }, permissionDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePermission(int id, [FromBody] PermissionDTO permissionDTO)
        {
            await _permissionService.UpdatePermissionAsync(id, permissionDTO);
            _elasticsearchService.LogOperation("UpdatePermission", permissionDTO);
            _producerService.ProducePermissionMessageAsync(permissionDTO, "MODIFY");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePermission(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null) {
                return NotFound();
            }
            _elasticsearchService.LogOperation("DeletePermission", permission);
            _producerService.ProducePermissionMessageAsync(permission, "DELETE");
            
            await _permissionService.DeletePermissionAsync(id);
            return NoContent();
        }
    }
}
