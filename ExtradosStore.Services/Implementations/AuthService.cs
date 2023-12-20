using ExtradosStore.Common.CustomExceptions.JWTExceptions;
using ExtradosStore.Common.CustomExceptions.UserExceptions;
using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.JWTDTOs;
using ExtradosStore.Entities.Models;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthDAO _atuthDAO;
        private readonly IRoleDAO _roleDAO;
        private readonly IHasherService _hasherService;
        private readonly IJWTService _jWTService;

        public AuthService(IAuthDAO atuthDAO, IHasherService hasherService, IRoleDAO roleDAO, IJWTService jWTService)
        {
            _hasherService = hasherService;
            _atuthDAO = atuthDAO;
            _roleDAO = roleDAO;
            _jWTService = jWTService;
        }

        //registrarse:  devuelve un int que representa las filas afectadas (dapper)
        public async Task<int> SignUpService(RegisterUserRequest createUserRequest)
        {
            try
            {
                var emailInDB = (await _atuthDAO.DataGetEmailUser(createUserRequest.user_email))?.ToLower();
                var emailInRequest = createUserRequest.user_email.ToLower();
                var phoneNumberInDB = (await _atuthDAO.DataGetPhoneNumberUser(createUserRequest.user_phone_number));
                var roleList = await _roleDAO.DataGetRoles();

                if (emailInDB != null && emailInDB == emailInRequest) throw new EmailAlreadyExistsException();

                if (phoneNumberInDB != null && phoneNumberInDB == createUserRequest.user_phone_number) throw new PhoneNumberAlreadyExistsException();

                int userIdRole = roleList.FirstOrDefault(role => role.role_name == "user")?.role_id ?? 0;
                if (userIdRole == 0) throw new KeyNotFoundException("role *user* not found in data base");

                createUserRequest.user_password_hash = _hasherService.HashPasswordUser(createUserRequest.user_password_hash);

                long dateOfBirthNewUserEpoch = new DateTimeOffset(createUserRequest.user_date_of_birth).ToUnixTimeMilliseconds();

                var Newuser = new User
                {
                    user_name = createUserRequest.user_name,
                    user_lastname = createUserRequest.user_lastname,
                    user_email = createUserRequest.user_email,
                    user_password_hash = createUserRequest.user_password_hash,
                    user_roleid = userIdRole,
                    user_created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    user_date_of_birth = dateOfBirthNewUserEpoch,
                    user_phone_number = createUserRequest.user_phone_number,
                };
                return await _atuthDAO.DataSignUp(Newuser);


            }
            catch
            {
                throw;
            }
        }


        //iniciar sesion: devuelve accesstoken y refresh token
        public async Task<AccesAndRefreshTokenDTO> SignInService(LoginUserRequest loginRequest)
        {
            try
            {
                var userInDB = await _atuthDAO.DataSignIn(loginRequest);
                if (userInDB == null) throw new UserNotFoundException();
                if (!_hasherService.VerifyPassword(loginRequest.user_password, userInDB.user_password_hash)) throw new IncorrectPasswordException();
                if (!userInDB.user_status) throw new DisabledUserException();
                var userClaims = new ClaimsTokenUserDTO { user_id = userInDB.user_id, user_name = userInDB.user_name, user_email = userInDB.user_email, user_roleid = userInDB.user_roleid };
                var tokenCreated = await _jWTService.CreateToken(userClaims);
                string refreshTokenCreated = _jWTService.CreateRefreshToken();
                //antes de guardar el reresh token en la bd, siempre se borra el anterior, de forma que no se repita mas de un registro por usuaior en la bd
                await _jWTService.DeleteRefreshTokenExpiredFromBd(userInDB.user_id);
                await _jWTService.SaveHistoryRefreshToken(userInDB.user_id, tokenCreated, refreshTokenCreated);

                return new AccesAndRefreshTokenDTO { AccessToken = tokenCreated, refreshToken = refreshTokenCreated };
            }
            catch
            {

                throw;
            }
        }
        // recibe un accessToken, genera un accessToken nuevo,
        // un nuevo refresh token, borra los tokes desactualizados de la db y guarda los nuevos en la db
        public async Task<AccesAndRefreshTokenDTO> GenerateRereshTokensService(AccesAndRefreshTokenDTO tokensRequest)
        {

            try
            {
                var refreshtokenRequest = tokensRequest.refreshToken;

                // extraer claims del access token
                ClaimsTokenUserDTO user = await _jWTService.GetUserFromAccessToken(tokensRequest.AccessToken);

                //validar que el Refreshtoken(el string) de la request sea el mismo que en la bd (en el campo token_refreshtoken)             
                if (!await _jWTService.CompareRefreshTokens(user.user_id, refreshtokenRequest)) throw new InvalidRefreshTokenException();

                //chequear que el token refresh no este vencido              
                if (!await _jWTService.RefreshTokenIsActive(user.user_id)) throw new ExpiredRefreshTokenException();

                //borra los tokens desactualizados en la db
                await _jWTService.DeleteRefreshTokenExpiredFromBd(user.user_id);

                var newTokens = new AccesAndRefreshTokenDTO
                {
                    AccessToken = await _jWTService.CreateToken(user),
                    refreshToken = _jWTService.CreateRefreshToken()
                };
                //guarda los nuevos tokens en la db
                await _jWTService.SaveHistoryRefreshToken(user.user_id, newTokens.AccessToken, newTokens.refreshToken);

                return newTokens;

            }
            catch
            {

                throw;
            }
        }

    }
}
