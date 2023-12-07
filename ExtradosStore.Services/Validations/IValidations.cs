using ExtradosStore.Entities.DTOs.JWTDTOs;

namespace ExtradosStore.Services.Validations
{
    public interface IValidations
    {
        bool ValidationClaimsUser(ClaimsTokenUserDTO userClaims);
    }
}