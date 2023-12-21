using System.ComponentModel.DataAnnotations;
namespace ExtradosStore.Common.CustomValidationAtributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class Base64ImageAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var base64String = value as string;

            if (string.IsNullOrEmpty(base64String))
            {

                return false;
            }

            try
            {

                byte[] imageBytes = Convert.FromBase64String(base64String);



                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }

}
