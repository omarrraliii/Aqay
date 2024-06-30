using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using aqay_apis.Models;

namespace aqay_apis.Validations
{
    public class ValidNationalId : ValidationAttribute
    {
        public ValidNationalId()
        {
            ErrorMessage = "Invalid National ID.";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var model = (SignupMerchantModel)validationContext.ObjectInstance;
            if (value == null && !string.IsNullOrEmpty(model.TaxRegistrationNumber))
            {

                return ValidationResult.Success;
            }
            else
            {
                const string EgyptianNationalIDPattern = @"[2-3]{1}[0-9]{2}[0-1]{1}[0-9]{1}[0-3]{1}[0-9]{7}";

                if (value is not string nationalId)
                {
                    return new ValidationResult(ErrorMessage);
                }


                // Check if the national ID matches the pattern
                else if (!Regex.IsMatch(nationalId, EgyptianNationalIDPattern))
                {
                    return new ValidationResult(ErrorMessage);
                }

                // Validation successful
                return ValidationResult.Success;
            }
        }
    }
}
