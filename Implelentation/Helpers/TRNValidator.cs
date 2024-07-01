using System.Text.RegularExpressions;

namespace aqay_apis.Helpers
{
    public class TRNValidator
    {
        public static bool ValidateTaxRegistrationNumber(string trn)
        {
            // Validate length and format
            if (trn.Length != 9 || !Regex.IsMatch(trn, @"^\d{9}$"))
            {
                return false;
            }

            // Validate the check digit
            if (!IsValidCheckDigit(trn))
            {
                return false;
            }

            return true;
        }

        private static bool IsValidCheckDigit(string trn)
        {
            // Assuming the check digit is the last digit
            // Implementing a common algorithm (Luhn Algorithm) for illustration
            int[] weights = { 3, 2, 7, 6, 5, 4, 3, 2, 1 }; // Hypothetical weights for TRN
            int sum = 0;

            for (int i = 0; i < 8; i++)
            {
                sum += int.Parse(trn[i].ToString()) * weights[i];
            }

            int checkDigit = sum % 11;
            if (checkDigit == 10)
            {
                checkDigit = 0;
            }

            return checkDigit == int.Parse(trn[8].ToString());
        }

    }
}
