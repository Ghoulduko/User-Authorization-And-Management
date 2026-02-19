using AuthorizationIntegration.Application.Dtos;
using AuthorizationIntegration.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationIntegration.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("RegisterUser")]
    public IActionResult RegisterUser(string username, string password, string email, string firstName, string lastName, string phoneNumber)
    {
        var didUserAdd = _userService.RegisterUser(username, password, email, firstName, lastName, phoneNumber);

        if (!didUserAdd)
        {
            return BadRequest("Failed to create user.");
        }

        return Ok("User created successfully.");
    }

    [HttpGet("GetAllUsers")]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("GetUserById")]
    public IActionResult GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        return Ok(user);
    }

    [HttpGet("GetUserByEmail")]
    public IActionResult GetUserByEmail(string email)
    {
        var user = _userService.GetUserByEmail(email);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        return Ok(user);
    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser(int id, UserUpdateDto user)
    {
        var didUserUpdate = _userService.UpdateUser(id, user);
        if (!didUserUpdate)
        {
            return BadRequest("Failed to update user.");
        }

        return Ok("User updated successfully.");
    }

    [HttpPatch("UpdateUserRole")]
    public IActionResult UpdateUserRole(int userId, int roleId)
    {
        var didUserRoleUpdate = _userService.UpdateUserRole(userId, roleId);
        if (!didUserRoleUpdate)
        {
            return BadRequest("Failed to update user role.");
        }
        return Ok("User role updated successfully.");
    }

    [HttpDelete("DeleteUser")]
    public IActionResult DeleteUser(int id)
    {
        var didUserDelete = _userService.DeleteUser(id);
        if (!didUserDelete)
        {
            return BadRequest("Failed to delete user.");
        }
        return Ok("User deleted successfully.");
    }
}
