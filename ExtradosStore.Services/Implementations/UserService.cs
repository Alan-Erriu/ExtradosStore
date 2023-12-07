using ExtradosStore.Common.CustomExceptions.UserExceptions;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;
        private readonly IRoleDAO _roleDAO;


        public UserService(IUserDAO userDAO, IRoleDAO roleDAO)
        {
            _userDAO = userDAO;
            _roleDAO = roleDAO;

        }

        //deshabilitar usuario
        public async Task<int> DisableUserService(int idUser)
        {
            var user = await _userDAO.DataGetUserById(idUser);
            if (user == null) throw new UserNotFoundException();
            if (!user.user_status) throw new InvalidCastException("The user's status was already false in the database");
            // el numero 0 corresponde a false en sqlServer(bit)
            var rowsAffected = await _userDAO.DataUpdateStatusUser(idUser, 0);
            return rowsAffected;
        }


        //habilitar usuario
        public async Task<int> EnableUserService(int idUser)
        {

            var user = await _userDAO.DataGetUserById(idUser);
            if (user == null) throw new UserNotFoundException();
            if (user.user_status) throw new InvalidCastException("The user's status was already true in the database");
            // el numero 1 corresponde a true en sqlServer(bit)
            var rowsAffected = await _userDAO.DataUpdateStatusUser(idUser, 1);

            return rowsAffected;
        }

        public async Task<int> UpgradeRoleFromUserToAdminService(int userId)
        {
            var user = await _userDAO.DataGetUserById(userId);
            if (user == null) throw new UserNotFoundException();
            var roleList = await _roleDAO.DataGetRoles();
            int adminIdRole = roleList.FirstOrDefault(role => role.role_name == "admin")?.role_id ?? 0;
            if (adminIdRole == 0) throw new KeyNotFoundException("role *admin* not found in data base");
            if (adminIdRole == user.user_roleid) throw new InvalidCastException("The user's role was already admin in the database");
            return await _userDAO.DataUpgradeRoleFromUserToAdmin(userId, adminIdRole);

        }
    }
}
