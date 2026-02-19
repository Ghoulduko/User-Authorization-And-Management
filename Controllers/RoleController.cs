using AuthorizationIntegration.Application.Services;
using AuthorizationIntegration.Core.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationIntegration.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("AddRole")]
    public IActionResult AddRole(string roleName)
    {
        var didRoleAdd = _roleService.AddRole(roleName);
        if (!didRoleAdd)
        {
            return BadRequest("Failed to add role");
        }
        return Ok("Role added successfully");
    }

    [HttpGet("GetAllRoles")]
    public IActionResult GetAllRoles()
    {
        var allRoles = _roleService.GetAllRoles();
        return Ok(allRoles);
    }

    [HttpDelete("DeleteRole")]
    public IActionResult DeleteRole(int id)
    {
        var didRoleDelete = _roleService.DeleteRole(id);
        if (!didRoleDelete)
        {
            return BadRequest("Failed to delete role");
        }
        return Ok("Role deleted successfully");
    }
}
