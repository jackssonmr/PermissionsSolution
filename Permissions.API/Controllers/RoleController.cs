using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Permissions.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpPost("roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRole([FromBody] string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var role = new IdentityRole(roleName);
            await _roleManager.CreateAsync(role);
            return Ok($"Role '{roleName}' created successfully");
        }
        else
        {
            return BadRequest($"Role '{roleName}' already exists");
        }
    }
}