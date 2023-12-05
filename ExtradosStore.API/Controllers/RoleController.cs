using ExtradosStore.Entities.DTOs.RoleDTOs;
using ExtradosStore.Services.Interfaces;
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

        [HttpPost("create")]

        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO roleRequest)
        {
            try
            {
                CreateRoleDTO roleCreated = await _roleService.CreateRoleService(roleRequest);
                return Ok(roleCreated);
            }
            catch (DuplicateNameException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("The name role is already in use");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error in controller  {Ex.Message}");
                Console.WriteLine($"Stack Trace: {Ex.StackTrace}");
                return StatusCode(500, "server error:");
            }
        }
    }
}
