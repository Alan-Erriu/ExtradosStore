using ExtradosStore.Entities.DTOs.JWTDTOs;

namespace ExtradosStore.Services.Implementations
{
    public interface IJWTService
    {
        Task<int> DeleteRefreshTokenExpiredFromBd(int id_user);
        Task<bool> RefreshTokenIsActive(int id_user);
        string CreateRefreshToken();
        string CreateToken(string id_user, string name_user, string mail_user, string role_user);

        Task<AccesAndRefreshTokenDTO> SaveHistoryRefreshToken(int id_user, string token, string refreshToken);
    }
}