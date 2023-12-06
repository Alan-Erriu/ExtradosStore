
using ExtradosStore.Configuration.JWTConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.JWTDTOs;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExtradosStore.Services.Implementations
{
    public class JWTService : IJWTService
    {
        private JWTConfig _jwtConfig;

        private IJWTDAO _jwtDao;
        public JWTService(IOptions<JWTConfig> jwtConfig, IJWTDAO jwtDao)
        {
            _jwtConfig = jwtConfig.Value;
            _jwtDao = jwtDao;
        }

        //funciones JWT----------------------------------------------------------

        // crear refresh token-------------
        private string CreateRefreshToken()
        {
            var byteArray = new byte[64];
            string refreshToken = "";
            using (var mg = RandomNumberGenerator.Create())
            {
                mg.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }

        //crear token jwt
        private string CreateToken(string id_user, string name_user, string mail_user, string role_user)
        {

            var key = _jwtConfig.Secret;
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, id_user));
            claims.AddClaim(new Claim(ClaimTypes.Name, name_user));
            claims.AddClaim(new Claim(ClaimTypes.Email, mail_user));
            claims.AddClaim(new Claim(ClaimTypes.Role, role_user));

            var credentialsToken = new SigningCredentials(
               new SymmetricSecurityKey(keyBytes),
               SecurityAlgorithms.HmacSha256Signature
               );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = credentialsToken,
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string createdToken = tokenHandler.WriteToken(tokenConfig);

            return createdToken;
        }

        //insertar en la tabla token, el refresh token, el user_id, fecha de expiracion de refreshToken y el Accesstoken
        private async Task<AccesAndRefreshTokenDTO> SaveHistoryRefreshToken(int id_user, string token, string refreshToken)
        {
            var tokenHistory = new Token
            {
                token_iduser = id_user,
                token_accesstoken = token,
                token_refreshToken = refreshToken,
                token_expiration_date_refreshtoken = DateTimeOffset.UtcNow.AddDays(2).ToUnixTimeMilliseconds()
            };
            AccesAndRefreshTokenDTO tokenSaved = await _jwtDao.DataInsertRefreshToken(tokenHistory);
            return tokenSaved;
        }

        //recibe un access token y retorna un nuevo access token y un refresh token
        //public async Task<TokensDTO> ReturnRefreshToken(RefreshTokenRequest tokenRequest)
        //{
        //    try
        //    {
        //        var user = GetUserFromExpiredToken(tokenRequest.expiredToken);
        //        var refreshTokenCreated = CreateRefreshToken();
        //        //crear un nuevo token de acceso a partir de los claims del token vencido
        //        var tokenCreated = CreateToken(user.id_user.ToString(), user.name_user, user.mail_user, user.role_user);
        //        await SaveHistoryRefreshToken(user.id_user, tokenCreated, refreshTokenCreated);
        //        return new TokensDTO { token = tokenCreated, refreshToken = refreshTokenCreated, msg = "ok" };
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new TokensDTO { msg = "server error" };
        //    }

        //}
        //buscar un refresh token por id_user y compararlo contra otro refresh token
        //public async Task<bool> CompareRefreshTokens(int id_user, string refreshToken)
        //{

        //    var refreshTokenBd = await _authRepository.DataSelectRefreshToken(id_user);

        //    if (refreshTokenBd.msg == "token not found") throw new ArgumentNullException("token not found in db, refreshTokenBd.refresh_Token_tokenhistory will be null");
        //    if (refreshTokenBd.refresh_Token_tokenhistory != refreshToken) return false;
        //    return true;
        //}

        public async Task<int> DeleteRefreshTokenExpiredFromBd(int id_user)
        {
            var rowsAffected = await _jwtDao.DataDeleteRefreshTokenExpired(id_user);
            return rowsAffected;
        }


        //obtener el refresh token de la bd y checkear que no este vencido
        public async Task<bool> RefreshTokenIsActive(int id_user)
        {

            AccesAndRefreshTokenDTO refreshTokenBd = await _jwtDao.DataSelectRefreshToken(id_user);
            if (refreshTokenBd.msg == "token not found") throw new ArgumentNullException("token not found in db, refreshTokenBd.expiration_date_tokenhistory will be null");
            long currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (refreshTokenBd. < currentEpochTime) return false;
            return true;
        }




        // obtener todos los datos del usuario por medio de los claims del token
        //public CreateUserWithRoleDTO GetUserFromExpiredToken(string token)
        //{
        //    var key = _jwtConfig.Secret;
        //    var keyBytes = Encoding.ASCII.GetBytes(key);

        //    var tokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        ValidateLifetime = false,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        //        ClockSkew = TimeSpan.Zero
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        //    JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
        //    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        throw new SecurityTokenException("Invalid token");
        //    }

        //    var user = new CreateUserWithRoleDTO
        //    {
        //        id_user = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value),
        //        name_user = principal.FindFirst(ClaimTypes.Name)?.Value,
        //        mail_user = principal.FindFirst(ClaimTypes.Email)?.Value,
        //        role_user = principal.FindFirst(ClaimTypes.Role)?.Value
        //    };


        //    return user;
        //}


    }
}
