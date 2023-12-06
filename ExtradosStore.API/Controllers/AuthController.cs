using ExtradosStore.Common.CustomExceptions.UserExceptions;
using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {

            _authService = authService;
        }
        // registrarse como usuario - rol=(user)

        [HttpPost("signup")]

        public async Task<IActionResult> SignUp([FromBody] RegisterUserRequest createUserRequest)
        {

            try
            {
                var user = await _authService.SignUpService(createUserRequest);

                return Ok("Succes");
            }
            catch (PhoneNumberAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("user role not found");
            }
            catch (EmailAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error register a new user {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }


        }

        [HttpPost("signin")]

        public async Task<IActionResult> signIn([FromBody] LoginUserRequest loginRequest)
        {

            try
            {
                var user = await _authService.SignInService(loginRequest);
                if (user == null) return NotFound("user not found");
                return Ok(user);
            }
            catch (IncorrectPasswordException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized("incorrect pasword");
            }
            catch (DisabledUserException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized(ex.Message);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error register a new user {Ex.Message} {Ex.StackTrace}");

                return StatusCode(500, "server error:");
            }


        }
    }
}
