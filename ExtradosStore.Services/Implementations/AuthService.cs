﻿using ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions;
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


        public async Task<int> SignUpService(RegisterUserRequest createUserRequest)
        {

            var emailInDB = (await _atuthDAO.DataGetEmailUser(createUserRequest.user_email))?.ToLower();
            var emailInRequest = createUserRequest.user_email.ToLower();
            var phoneNumberInDB = (await _atuthDAO.DataGetPhoneNumberUser(createUserRequest.user_phone_number));
            var roleList = await _roleDAO.DataGetRoles();

            if (emailInDB != null && emailInDB == emailInRequest) throw new ConflictException("The email is already in use");

            if (phoneNumberInDB != null && phoneNumberInDB == createUserRequest.user_phone_number) throw new ConflictException("The phone number is already in use");

            int userIdRole = roleList.FirstOrDefault(role => role.role_name == "user")?.role_id ?? 0;
            if (userIdRole == 0) throw new NotFoundException("role *user* not found in data base");

            createUserRequest.user_password_hash = _hasherService.HashPasswordUser(createUserRequest.user_password_hash);



            var Newuser = new User
            {
                user_name = createUserRequest.user_name,
                user_lastname = createUserRequest.user_lastname,
                user_email = createUserRequest.user_email,
                user_password_hash = createUserRequest.user_password_hash,
                user_roleid = userIdRole,
                user_created_at = DateTime.UtcNow,
                user_date_of_birth = createUserRequest.user_date_of_birth.ToUniversalTime(),
                user_phone_number = createUserRequest.user_phone_number,
            };
            return await _atuthDAO.DataSignUp(Newuser);



        }


        //iniciar sesion: devuelve accesstoken y refresh token
        public async Task<AccesAndRefreshTokenDTO> SignInService(LoginUserRequest loginRequest)
        {

            var userInDB = await _atuthDAO.DataSignIn(loginRequest);
            if (userInDB == null) throw new NotFoundException("User Not Found");
            if (!_hasherService.VerifyPassword(loginRequest.user_password, userInDB.user_password_hash)) throw new UnauthorizedException("incorrect password");
            if (!userInDB.user_status) throw new UnauthorizedException("The user is disabled");
            var userClaims = new ClaimsTokenUserDTO { user_id = userInDB.user_id, user_name = userInDB.user_name, user_email = userInDB.user_email, user_roleid = userInDB.user_roleid };
            var tokenCreated = await _jWTService.CreateToken(userClaims);
            string refreshTokenCreated = _jWTService.CreateRefreshToken();
            //antes de guardar el reresh token en la bd, siempre se borra el anterior, de forma que no se repita mas de un registro por usuaior en la bd
            await _jWTService.DeleteRefreshTokenExpiredFromBd(userInDB.user_id);
            await _jWTService.SaveHistoryRefreshToken(userInDB.user_id, tokenCreated, refreshTokenCreated);

            return new AccesAndRefreshTokenDTO { AccessToken = tokenCreated, refreshToken = refreshTokenCreated };

        }
        // recibe un accessToken, genera un accessToken nuevo,
        // un nuevo refresh token, borra los tokes desactualizados de la db y guarda los nuevos en la db
        public async Task<AccesAndRefreshTokenDTO> GenerateRereshTokensService(AccesAndRefreshTokenDTO tokensRequest)
        {


            var refreshtokenRequest = tokensRequest.refreshToken;

            // extraer claims del access token
            ClaimsTokenUserDTO user = await _jWTService.GetUserFromAccessToken(tokensRequest.AccessToken);

            //validar que el Refreshtoken(el string) de la request sea el mismo que en la bd (en el campo token_refreshtoken)             
            if (!await _jWTService.CompareRefreshTokens(user.user_id, refreshtokenRequest)) throw new UnauthorizedException();

            //chequear que el token refresh no este vencido              
            if (!await _jWTService.RefreshTokenIsActive(user.user_id)) throw new UnauthorizedException();

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

    }
}
