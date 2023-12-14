using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class Minimum24HoursFromNowAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            DateTime minimumDateUtc = DateTime.UtcNow.AddHours(24);

            if (date < minimumDateUtc)
            {
                return new ValidationResult($"The date must be at least 24 hours from now. The minimum allowed date is {minimumDateUtc} (UTC).");
            }
        }

        return ValidationResult.Success;
    }
}

