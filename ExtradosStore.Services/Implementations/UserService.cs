using ExtradosStore.Common.CustomExceptions.UserExceptions;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;


        public UserService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        //deshabilitar usuario
        public async Task<int> DisableUserService(int idUser)
        {
            var user = _userDAO.DataGetUserById(idUser);
            if (user == null) throw new UserNotFoundExceptioncs();
            // el numero 0 corresponde a false en sqlServer(bit)
            var rowsAffected = await _userDAO.DataUpdateStatusUser(idUser, 0);
            // borrar token, buscar publicaciones y deshabilitarlas
            return rowsAffected;
        }


        //habilitar usuario
        public async Task<int> EnableUserService(int idUser)
        {

            var user = _userDAO.DataGetUserById(idUser);
            if (user == null) throw new UserNotFoundExceptioncs();
            // el numero 0 corresponde a false en sqlServer(bit)
            var rowsAffected = await _userDAO.DataUpdateStatusUser(idUser, 1);

            return rowsAffected;
        }
    }
}
