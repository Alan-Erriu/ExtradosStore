using ExtradosStore.Common.CustomExceptions.JWTExceptions;
using ExtradosStore.Entities.DTOs.JWTDTOs;

namespace ExtradosStore.Services.Validations
{
    public class Validations : IValidations
    {

        public bool ValidationClaimsUser(ClaimsTokenUserDTO userClaims)
        {
            if (userClaims.user_id == 0) throw new MissingClaimsException("user id not foun in claims token");
            if (string.IsNullOrEmpty(userClaims.user_name)) throw new MissingClaimsException("user name not foun in claims token");
            if (string.IsNullOrEmpty(userClaims.user_email)) throw new MissingClaimsException("user email not foun in claims token");
            if (userClaims.user_roleid == 0) throw new MissingClaimsException("user rol id not foun in claims token");
            return true;

        }
    }
}
