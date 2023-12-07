using ExtradosStore.Entities.DTOs.JWTDTOs;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IJWTDAO
    {
        Task<int> DataDeleteRefreshTokenExpired(int userId);
        Task<AccesAndRefreshTokenDTO> DataInsertRefreshToken(Token tokenRequest);
        Task<Token> DataSelectRefreshToken(int id_user);
    }
}