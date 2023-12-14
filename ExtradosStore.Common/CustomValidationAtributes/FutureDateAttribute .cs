using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            // convertir la fecha en horario de cada pais ejemplo -03 (argentina) a UTC si no está en formato UTC
            if (date.Kind != DateTimeKind.Utc)
            {
                date = date.ToUniversalTime();
            }

            if (date < DateTime.UtcNow)
            {
                return new ValidationResult("La fecha debe ser superior a la actual");
            }
        }

        return ValidationResult.Success;
    }
}
