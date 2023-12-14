using ExtradosStore.Common.CustomExceptions.UserExceptions;
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
            try
            {

                var rowsAffected = await _userService.DisableUserService(userId);

                await _jwtService.DeleteRefreshTokenExpiredFromBd(userId);
                return Ok("now user is disable");
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("user not found");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("the user was already disabled");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error{Ex.Message}");
                Console.WriteLine($"Stack Trace: {Ex.StackTrace}");
                return StatusCode(500, "server error:");
            }
        }

        [HttpPut("enable/{userId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> EnableleUser(int userId)
        {
            try
            {

                var rowsAffected = await _userService.EnableUserService(userId);
                if (rowsAffected == 0) return StatusCode(500, "server error");
                return Ok("now user is enable");
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("user not found");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("the user was already enable");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error{Ex.Message}");
                Console.WriteLine($"Stack Trace: {Ex.StackTrace}");
                return StatusCode(500, "server error:");
            }
        }

        [HttpPut("upgrade/{userId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> UpgradeRoleFromUserToAdmin(int userId)
        {
            try
            {

                var rowsAffected = await _userService.UpgradeRoleFromUserToAdminService(userId);
                if (rowsAffected == 0) return StatusCode(500, "server error");
                return Ok("now user is admin");
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("user not found");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("role *admin* not found");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict("The user's role was already admin in the database");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error{Ex.Message}");
                Console.WriteLine($"Stack Trace: {Ex.StackTrace}");
                return StatusCode(500, "server error:");
            }
        }

    }
}
