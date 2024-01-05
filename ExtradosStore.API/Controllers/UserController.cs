using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {

        private IUserService _userService;
        private IJWTService _jwtService;

        public UserController(IUserService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }


        [HttpPut("disable/{userId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DisableUser(int userId)
        {


            var rowsAffected = await _userService.DisableUserService(userId);

            await _jwtService.DeleteRefreshTokenExpiredFromBd(userId);
            return Ok("now user is disable");




        }
        [HttpGet("getusers")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> getUsersAdmin()
        {

            var listUser = await _userService.GetUsersService();
            return Ok(listUser);
        }

        [HttpPut("enable/{userId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> EnableleUser(int userId)
        {

            var rowsAffected = await _userService.EnableUserService(userId);
            return Ok("now user is enable");
        }

        [HttpPut("upgrade/{userId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> UpgradeRoleFromUserToAdmin(int userId)
        {


            var rowsAffected = await _userService.UpgradeRoleFromUserToAdminService(userId);

            return Ok("now user is admin");

        }

    }
}
