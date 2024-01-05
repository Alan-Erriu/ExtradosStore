using ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions;
using ExtradosStore.Entities.DTOs.JWTDTOs;

namespace ExtradosStore.Services.Validations
{
    public class Validations : IValidations
    {

        public bool ValidationClaimsUser(ClaimsTokenUserDTO userClaims)
        {
            if (userClaims.user_id == 0) throw new UnauthorizedException("user id not foun in claims token");
            if (string.IsNullOrEmpty(userClaims.user_name)) throw new UnauthorizedException("user name not foun in claims token");
            if (string.IsNullOrEmpty(userClaims.user_email)) throw new UnauthorizedException("user email not foun in claims token");
            if (userClaims.user_roleid == 0) throw new UnauthorizedException("user rol id not foun in claims token");
            return true;

        }
    }
}
