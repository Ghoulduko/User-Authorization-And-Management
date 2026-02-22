using AuthorizationIntegration.Application.Dtos;
using AuthorizationIntegration.Application.Services;
using AuthorizationIntegration.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationIntegration.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly PermissionService _permissionService;

    public PermissionController(PermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet("GetAllPermissions")]
    public IActionResult GetAll(int roleId)
    {
        var userRole = User.FindFirst("RoleId")?.Value;
        if (userRole == null)
        {
            return BadRequest("User role not found.");
        }
        if (userRole == "1" || userRole == "2" || userRole == "5")
        {
            return StatusCode(403, "You do not have permission to access this resource.");
        }

        var permissions = _permissionService.GetAllPermissionsOfRole(roleId);
        return Ok(permissions);
    }

    [HttpPost("AttachPermissionToRole")]
    public IActionResult AttachPermissionToRole(ActionEnum actionName, int roleId, DateTime expiration)
    {
        var userRole = User.FindFirst("RoleId")?.Value;
        if (userRole == null)
        {
            return BadRequest("User role not found.");
        }
        if (userRole == "1" || userRole == "2" || userRole == "5")
        {
            return StatusCode(403, "You do not have permission to access this resource.");
        }

        var permission = new PermissionAttachmentDto
        {
            Action = actionName,
            RoleId = roleId,
            Expiration = expiration
        };

        _permissionService.AttachPermissionToRole(permission);
        return Ok("Permission attached successfully.");
    }

    [HttpDelete("DetachPermissionFromRole")]
    public IActionResult DetachPermissionFromRole(ActionEnum actionName, int roleId)
    {
        var userRole = User.FindFirst("RoleId")?.Value;
        if (userRole == null)
        {
            return BadRequest("User role not found.");
        }
        if (userRole == "1" || userRole == "2" || userRole == "5")
        {
            return BadRequest("You do not have permission to access this resource.");
        }

        var permission = new PermissionDetachmentDto
        {
            Action = actionName,
            RoleId = roleId,
        };

        _permissionService.DetachPermissionFromRole(permission);
        return Ok("Permission detached successfully.");
    }
}
