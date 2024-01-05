using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Entities.DTOs.JWTDTOs;
using ExtradosStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

            await _authService.SignUpService(createUserRequest);
            return Ok("Succes");

        }

        [HttpPost("signin")]

        public async Task<IActionResult> signIn([FromBody] LoginUserRequest loginRequest)
        {

            var userTokens = await _authService.SignInService(loginRequest);
            return Ok(userTokens);

        }



        [HttpPost("gettoken")]
        public async Task<IActionResult> GetRefreshToken([FromBody] AccesAndRefreshTokenDTO acessAndRefreshTokenRequest)
        {
            var newTokes = await _authService.GenerateRereshTokensService(acessAndRefreshTokenRequest);
            return Ok(newTokes);
        }
    }
}
