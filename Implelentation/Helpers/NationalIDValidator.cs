using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class NationalIDValidator
{
    // Constants for governorates
    private static readonly Dictionary<string, string> Governorates = new Dictionary<string, string>
    {
        {"01", "Cairo"},
        {"02", "Alexandria"},
        {"03", "Port Said"},
        {"04", "Suez"},
        {"11", "Damietta"},
        {"12", "Dakahlia"},
        {"13", "Ash Sharqia"},
        {"14", "Kaliobeya"},
        {"15", "Kafr El - Sheikh"},
        {"16", "Gharbia"},
        {"17", "Monoufia"},
        {"18", "El Beheira"},
        {"19", "Ismailia"},
        {"21", "Giza"},
        {"22", "Beni Suef"},
        {"23", "Fayoum"},
        {"24", "El Menia"},
        {"25", "Assiut"},
        {"26", "Sohag"},
        {"27", "Qena"},
        {"28", "Aswan"},
        {"29", "Luxor"},
        {"31", "Red Sea"},
        {"32", "New Valley"},
        {"33", "Matrouh"},
        {"34", "North Sinai"},
        {"35", "South Sinai"},
        {"88", "Foreign"}
    };

    // Validate and extract information from an Egyptian National ID
    public static bool ValidateEGYNationalID(string id)
    {
        if (string.IsNullOrEmpty(id) || id.Length != 14 || !Regex.IsMatch(id, @"^\d{14}$"))
            return false;

        try
        {
            // Extract components from the ID
            string centuryCode = id.Substring(0, 1);
            string birthDateStr = id.Substring(1, 6);
            string governorateCode = id.Substring(7, 2);
            string sequence = id.Substring(9, 4);
            int genderCode = int.Parse(id.Substring(12, 1));

            // Validate century code and birthdate
            int century = ExtractBirthCentury(int.Parse(centuryCode));
            DateTime birthDate = ConvertBirthdate(birthDateStr, century);
            if (!ValidateBirthDate(birthDate, century))
                return false;

            // Validate governorate code
            if (!Governorates.ContainsKey(governorateCode))
                return false;

            // Validate gender code
            if (genderCode < 1 || genderCode > 9)
                return false;

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // Extract information from an Egyptian National ID
    public static string ExtractFormattedInfo(string id)
    {
        try
        {
            // Extract components from the ID
            string centuryCode = id.Substring(0, 1);
            string birthDateStr = id.Substring(1, 6);
            string governorateCode = id.Substring(7, 2);
            string sequence = id.Substring(9, 4);
            int genderCode = int.Parse(id.Substring(12, 1));

            // Extract and validate birth century and date
            int century = ExtractBirthCentury(int.Parse(centuryCode));
            DateTime birthDate = ConvertBirthdate(birthDateStr, century);
            if (!ValidateBirthDate(birthDate, century))
                throw new ArgumentException("Invalid birth date or century.");

            // Extract governorate name
            string governorate = Governorates[governorateCode];

            // Determine gender
            string gender = (genderCode % 2 == 0) ? "Female" : "Male";

            // Format information into a string
            string formattedInfo = $"Birth Century: {century}\n" +
                                   $"Birth Date: {birthDate:yyyy-MM-dd}\n" +
                                   $"Governorate: {governorate}\n" +
                                   $"Sequence: {sequence}\n" +
                                   $"Gender: {gender}";

            return formattedInfo;
        }
        catch (Exception ex)
        {
            return $"Invalid Egyptian National ID: {ex.Message}";
        }
    }


    // Helper method to extract birth century from century code
    private static int ExtractBirthCentury(int centuryCode)
    {
        if (centuryCode < 1 || centuryCode > 4)
            throw new ArgumentException("Invalid century code.");

        // Calculate century based on current year
        int currentYear = DateTime.Now.Year;
        int currentCentury = (currentYear / 100) + 1;

        return centuryCode + 18; // Adjusting to the actual century range
    }

    // Helper method to convert birthdate string to DateTime object
    private static DateTime ConvertBirthdate(string birthDateStr, int century)
    {
        int year = (century * 100) - 100 + int.Parse(birthDateStr.Substring(0, 2));
        int month = int.Parse(birthDateStr.Substring(2, 2));
        int day = int.Parse(birthDateStr.Substring(4, 2));

        return new DateTime(year, month, day);
    }

    // Helper method to validate birthdate against century range
    private static bool ValidateBirthDate(DateTime birthDate, int century)
    {
        // Validate birth date against the calculated century range
        DateTime minBirthDate = new DateTime((century * 100) - 100, 1, 1);
        DateTime maxBirthDate = new DateTime((century * 100) - 1, 12, 31);

        return birthDate >= minBirthDate && birthDate <= maxBirthDate;
    }
}
