using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Entities.DTOs.AuthDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> SignUpService(RegisterUserRequest createUserRequest);
        Task<AuthLoginUserDTO> SignInService(LoginUserRequest createUserRequest);
    }
}