using ExtradosStore.Entities.DTOs.JWTDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IJWTService
    {
        Task<int> DeleteRefreshTokenExpiredFromBd(int id_user);
        string CreateRefreshToken();
        Task<string> CreateToken(ClaimsTokenUserDTO user);
        Task<ClaimsTokenUserDTO> GetUserFromAccessToken(string token);
        Task<bool> RefreshTokenIsActive(int id_user);
        Task<bool> CompareRefreshTokens(int id_user, string refreshToken);

        Task<AccesAndRefreshTokenDTO> SaveHistoryRefreshToken(int id_user, string token, string refreshToken);
    }
}