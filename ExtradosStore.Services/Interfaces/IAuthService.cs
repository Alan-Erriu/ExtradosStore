using ExtradosStore.Common.CustomRequest.AuthRequest;

namespace ExtradosStore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> SignUpService(RegisterUserRequest createUserRequest);
    }
}