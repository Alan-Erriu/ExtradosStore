using ExtradosStore.Common.CustomExceptions.UserExceptions;
using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.AuthDTOs;
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



        public async Task<AuthLoginUserDTO> SignInService(LoginUserRequest loginRequest)
        {
            try
            {
                var userInDB = await _atuthDAO.DataSignIn(loginRequest);
                if (!_hasherService.VerifyPassword(loginRequest.user_password, userInDB.user_password_hash)) throw new IncorrectPasswordException();
                if (!userInDB.user_status) throw new DisabledUserException();
                var roleName = await _roleDAO.DataGetNameRoleById(userInDB.user_roleid);
                var tokenCreated = _jWTService.CreateToken(userInDB.user_id.ToString(), userInDB.user_name, userInDB.user_email, roleName);
                string refreshToken = _jWTService.CreateRefreshToken();
                //antes de guardar el reresh token en la bd, siempre se borra el anterior, de forma que no se repita mas de un registro por usuaior en la bd
                await _jWTService.DeleteRefreshTokenExpiredFromBd(userInDB.user_id);
                var credencialUser = await _jWTService.SaveHistoryRefreshToken(userInDB.user_id, tokenCreated, refreshToken);
                return userInDB;
            }
            catch
            {

                throw;
            }
        }



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
    }
}
