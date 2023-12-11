using ExtradosStore.Configuration.JWTConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.JWTDTOs;
using ExtradosStore.Entities.Models;
using ExtradosStore.Services.Interfaces;
using ExtradosStore.Services.Validations;
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

        private IRoleDAO _roleDAO;

        private IValidations _validations;
        public JWTService(IOptions<JWTConfig> jwtConfig, IJWTDAO jwtDao, IRoleDAO roleDAO, IValidations validations)
        {
            _jwtConfig = jwtConfig.Value;
            _jwtDao = jwtDao;
            _roleDAO = roleDAO;
            _validations = validations;
        }

        //funciones JWT----------------------------------------------------------

        // crear refresh token-------------
        public string CreateRefreshToken()
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
        public async Task<string> CreateToken(ClaimsTokenUserDTO userClaims)
        {
            var roleName = await _roleDAO.DataGetNameRoleById(userClaims.user_roleid);
            var key = _jwtConfig.Secret;
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userClaims.user_id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, userClaims.user_name));
            claims.AddClaim(new Claim(ClaimTypes.Email, userClaims.user_email));
            claims.AddClaim(new Claim(ClaimTypes.Role, roleName));

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
        public async Task<AccesAndRefreshTokenDTO> SaveHistoryRefreshToken(int id_user, string token, string refreshToken)
        {
            var tokenHistory = new Token
            {
                token_userid = id_user,
                token_accesstoken = token,
                token_refreshToken = refreshToken,
                token_expiration_date_refreshtoken = DateTimeOffset.UtcNow.AddDays(4).ToUnixTimeMilliseconds()
            };
            AccesAndRefreshTokenDTO tokenSaved = await _jwtDao.DataInsertRefreshToken(tokenHistory);
            return tokenSaved;
        }

        //recibe un access token y retorna un nuevo access token y un refresh token
        public async Task<AccesAndRefreshTokenDTO> ReturnRefreshToken(string accessTokenRequest)
        {
            try
            {
                ClaimsTokenUserDTO user = await GetUserFromAccessToken(accessTokenRequest);
                var refreshTokenCreated = CreateRefreshToken();
                //crear un nuevo token de acceso a partir de los claims del token vencido
                var roleName = await _roleDAO.DataGetNameRoleById(user.user_roleid);
                var tokenCreated = await CreateToken(user);
                await SaveHistoryRefreshToken(user.user_id, tokenCreated, refreshTokenCreated);
                return new AccesAndRefreshTokenDTO { AccessToken = tokenCreated, refreshToken = refreshTokenCreated };
            }
            catch
            {

                throw;
            }

        }
        //buscar un refresh token por id_user y compararlo contra otro refresh token
        public async Task<bool> CompareRefreshTokens(int id_user, string refreshToken)
        {

            Token refreshTokenBd = await _jwtDao.DataSelectRefreshToken(id_user);

            if (refreshTokenBd.token_accesstoken == null) throw new ArgumentNullException("token not found in db, refreshTokenBd.refresh_Token_tokenhistory will be null");
            if (refreshTokenBd.token_refreshToken != refreshToken) return false;
            return true;
        }

        public async Task<int> DeleteRefreshTokenExpiredFromBd(int id_user)
        {
            var rowsAffected = await _jwtDao.DataDeleteRefreshTokenExpired(id_user);
            return rowsAffected;
        }


        //obtener el refresh token de la bd y checkear que no este vencido
        public async Task<bool> RefreshTokenIsActive(int id_user)
        {

            Token refreshTokenBd = await _jwtDao.DataSelectRefreshToken(id_user);
            if (refreshTokenBd.token_accesstoken == null) throw new ArgumentNullException("token not found in db, refreshTokenBd.expiration_date_tokenhistory will be null");
            long currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (refreshTokenBd.token_expiration_date_refreshtoken < currentEpochTime) return false;
            return true;
        }




        // obtener todos los datos del usuario por medio de los claims del token
        public async Task<ClaimsTokenUserDTO> GetUserFromAccessToken(string token)
        {
            var key = _jwtConfig.Secret;
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var user = new ClaimsTokenUserDTO
            {
                user_id = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                user_name = principal.FindFirst(ClaimTypes.Name)?.Value,
                user_email = principal.FindFirst(ClaimTypes.Email)?.Value,
                user_roleid = await _roleDAO.DataGetIdRoleByName(principal.FindFirst(ClaimTypes.Role)?.Value)
            };
            _validations.ValidationClaimsUser(user);
            return user;
        }


    }
}

