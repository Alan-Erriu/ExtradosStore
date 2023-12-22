using ExtradosStore.Common.CustomExceptions.JWTExceptions;
using ExtradosStore.Common.CustomExceptions.UserExceptions;
using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Entities.DTOs.JWTDTOs;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExtradosStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly IJWTService _jwtService;
        public AuthController(IAuthService authService, IJWTService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }
        // registrarse como usuario - rol=(user)

        [HttpPost("signup")]

        public async Task<IActionResult> SignUp([FromBody] RegisterUserRequest createUserRequest)
        {

            try
            {
                var rowsAffected = await _authService.SignUpService(createUserRequest);


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

                return StatusCode(500, "Something went wrong. Please contact support.");
            }


        }

        [HttpPost("signin")]

        public async Task<IActionResult> signIn([FromBody] LoginUserRequest loginRequest)
        {

            try
            {
                var userTokens = await _authService.SignInService(loginRequest);

                return Ok(userTokens);
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("user not found");
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

                return StatusCode(500, "Something went wrong. Please contact support.");
            }


        }



        [HttpPost("gettoken")]
        public async Task<IActionResult> GetRefreshToken([FromBody] AccesAndRefreshTokenDTO acessAndRefreshTokenRequest)
        {
            try
            {


                var newTokes = await _authService.GenerateRereshTokensService(acessAndRefreshTokenRequest);

                return Ok(newTokes);


            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
            //refresh token distinto al de la db
            catch (InvalidRefreshTokenException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
            //refresh token caducado en db
            catch (ExpiredRefreshTokenException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
            //falta algun claim en el access token
            catch (MissingClaimsException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {

                //CompareRefreshTokens() lanza este error cuando el token, no es un token o no tiene un formato jwt valido 
                if (ex.Message.Contains("IDX12723"))
                {
                    Console.WriteLine(ex.Message);
                    return Unauthorized();
                }

                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Something went wrong. Please contact support.");
            }
        }
    }
}
