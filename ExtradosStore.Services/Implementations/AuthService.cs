
using ExtradosStore.Common.CustomExceptions;
using ExtradosStore.Common.CustomRequest.AuthRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthDAO _atuthDAO;
        private readonly IRoleDAO _roleDAO;
        private readonly IHasherService _hasherService;
        public AuthService(IAuthDAO atuthDAO, IHasherService hasherService, IRoleDAO roleDAO)
        {
            _hasherService = hasherService;
            _atuthDAO = atuthDAO;
            _roleDAO = roleDAO;
        }



        public async Task<int> SignUpService(RegisterUserRequest createUserRequest)
        {
            try
            {


                var emailInDatabase = (await _atuthDAO.DataGetEmailUser(createUserRequest.user_email))?.ToLower();
                var emailInRequest = createUserRequest.user_email.ToLower();

                if (emailInDatabase != null && emailInDatabase == emailInRequest) throw new EmailAlreadyExistsException();

                createUserRequest.user_password_hash = _hasherService.HashPasswordUser(createUserRequest.user_password_hash);

                var roleList = await _roleDAO.DataGetRoles();

                int userIdRole = roleList.FirstOrDefault(role => role.role_name == "user")?.role_id ?? 0;

                if (userIdRole == 0) throw new KeyNotFoundException("role *user* not found in data base");

                long dateOfBirthNewUserEpoch = new DateTimeOffset(createUserRequest.user_date_of_birth).ToUnixTimeMilliseconds();

                var Newuser = new User
                {

                    user_name = createUserRequest.user_name,
                    user_lastname = createUserRequest.user_lastname,
                    user_email = createUserRequest.user_email,
                    user_password_hash = createUserRequest.user_password_hash,
                    user_roleid = userIdRole,
                    user_created_at = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    date_of_birth = dateOfBirthNewUserEpoch,
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
