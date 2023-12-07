using ExtradosStore.Entities.DTOs.RoleDTOs;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [Authorize(Roles = "admin")]
        [HttpPost("create")]

        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO roleRequest)
        {
            try
            {
                CreateRoleDTO roleCreated = await _roleService.CreateRoleService(roleRequest);
                return Ok("succes");
            }
            catch (DuplicateNameException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("The name role is already in use");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error{Ex.Message}");
                Console.WriteLine($"Stack Trace: {Ex.StackTrace}");
                return StatusCode(500, "server error:");
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet("getroles")]

        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var rolesDB = await _roleService.GetRolesService();
                return Ok(rolesDB);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error:{Ex.Message}");
                Console.WriteLine($"Stack Trace: {Ex.StackTrace}");
                return StatusCode(500, "server error:");
            }
        }
    }
}
