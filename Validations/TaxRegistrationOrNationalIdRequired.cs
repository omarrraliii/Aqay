using System.ComponentModel.DataAnnotations;
using aqay_apis.Models;// Ensure to import SignUpViewModel namespace

namespace aqay_apis.Validations
{
    public class TaxRegistrationOrNationalIdRequired : ValidationAttribute
    {
        public TaxRegistrationOrNationalIdRequired()
        {
            ErrorMessage = "Either Tax Registration Number or National ID is required.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (SignupMerchantModel)validationContext.ObjectInstance;

            // Check if either TaxRegistrationNumber or NationalId is provided
            if (string.IsNullOrEmpty(model.TaxRegistrationNumber) && string.IsNullOrEmpty(model.NationalId))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
