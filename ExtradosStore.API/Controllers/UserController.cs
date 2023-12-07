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

                await _userService.DisableUserService(userId);
                await _jwtService.DeleteRefreshTokenExpiredFromBd(userId);

                return Ok("user disable");
            }
            catch (UserNotFoundExceptioncs ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("user not found");
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

                var user = await _userService.DisableUserService(userId);
                if (user == 0) return StatusCode(500, "server error");
                return Ok("user disable");
            }
            catch (UserNotFoundExceptioncs ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("user not found");
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
