using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Entities.DTOs.JWTDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> SignUpService(RegisterUserRequest createUserRequest);
        Task<AccesAndRefreshTokenDTO> SignInService(LoginUserRequest createUserRequest);

        Task<AccesAndRefreshTokenDTO> GenerateRereshTokensService(AccesAndRefreshTokenDTO tokensRequest);
    }
}