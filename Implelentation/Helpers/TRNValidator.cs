using System.Text.RegularExpressions;

namespace aqay_apis.Helpers
{
    public class TRNValidator
    {
        public static bool ValidateTaxRegistrationNumber(string trn)
        {
            // Validate length and format
            if (trn.Length != 9)
            {
                return false;
            }
            foreach (var c in trn)
            {
                if(c> 57 || c < 48)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
