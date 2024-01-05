using ExtradosStore.Entities.DTOs.RoleDTOs;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            CreateRoleDTO roleCreated = await _roleService.CreateRoleService(roleRequest);
            return Ok("succes");
        }
        [Authorize(Roles = "admin")]
        [HttpGet("getroles")]

        public async Task<IActionResult> GetRoles()
        {
            var rolesDB = await _roleService.GetRolesService();
            return Ok(rolesDB);
        }
    }
}
