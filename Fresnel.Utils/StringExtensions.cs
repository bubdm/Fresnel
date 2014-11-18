using System;
using System.Globalization;


namespace Envivo.Fresnel.Utils
{

    /// <summary>
    /// Some String helper functions
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static class StringExtensions
    {

        public static string PILCROW = ((char)182).ToString();

        /// <summary>
        /// Returns a display-friendly name
        /// </summary>
        /// <param name="name">The string to be converted into a friendly representation</param>
        /// <returns>The converted string</returns>
        /// <remarks>E.g. "send_notificationEmail" becomes "Send Notification Email"</remarks>
        public static string CreateFriendlyName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            // First we break apart words by inserting spaces before uppercase letters:
            var regexp = new System.Text.RegularExpressions.Regex("[A-Z][a-z]");
            var newName = regexp.Replace(name, " $&");

            // Then we insert spaces between lowercase letters and uppercase letters:
            regexp = new System.Text.RegularExpressions.Regex("([a-z])([A-Z])");
            newName = regexp.Replace(newName, "$1 $2");

            // Then we insert spaces between numbers and letters:
            regexp = new System.Text.RegularExpressions.Regex("([A-Za-z])([0-9])");
            newName = regexp.Replace(newName, "$1 $2");
            regexp = new System.Text.RegularExpressions.Regex("([0-9])([A-Za-z])");
            newName = regexp.Replace(newName, "$1 $2");

            // Finally we can tidy up and capitalise:
            newName = newName.Replace("_", " ");
            newName = newName.Replace("  ", " ");
            newName = newName.Trim();
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newName);
        }

        /// <summary>
        /// Returns a string that is compressed to fit the given size.
        /// If the original string is already with the size limit, it will be returned unaltered.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maximumLength"></param>
        
        
        public static string CompressToFit(this string value, int maximumLength)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var result = value.Trim();

            if (value.Length > maximumLength)
            {
                // Remove all vowels, spaces, and underscore chars:
                string[] ignoredChars = { "a", "e", "i", "o", "u", ".", " ", "_" };
                foreach (var ignoredChar in ignoredChars)
                {
                    result = result.Replace(ignoredChar, string.Empty);
                }

                if (result.Length > maximumLength)
                {
                    throw new ArgumentException(string.Format("The value '{0}' exceeds the limit of {1} chars", result, maximumLength));
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a compressed version of the given string value.
        /// All vowels and duplicate characters are removed.
        /// </summary>
        /// <param name="value"></param>
        
        
        public static string Compress(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var result = value.Trim();

            // Remove all vowels and duplicate chars:
            char[] ignoredChars = { 'a', 'e', 'i', 'o', 'u', '.', ' ', '_' };
            foreach (var ignoredChar in ignoredChars)
            {
                result = result.Replace(ignoredChar, char.MinValue);
            }

            return result;
        }

        /// <summary>
        /// Replaces 'newline' characters so that the value can be displayed in a single line
        /// </summary>
        /// <param name="value"></param>
        
        public static string ConvertToSingleLine(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace(Environment.NewLine, PILCROW);
        }


        /// <summary>
        /// Replaces PILCROW characters with newlines, so that the value is displayed over multiple lines
        /// </summary>
        /// <param name="value"></param>
        
        public static string ConvertToMultiLine(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace(PILCROW, Environment.NewLine);
        }

        /// <summary>
        /// Calculates the Levenshtein distance between the two given strings
        /// </summary>
        /// <param name="stringA"></param>
        /// <param name="stringB"></param>
        
        public static int CalculateDistanceFrom(this string stringA, string stringB)
        {
            var lengthA = stringA.Length;
            var lengthB = stringB.Length;

            var matrix = new int[lengthA + 1, lengthB + 1];
            var cost = 0;

            // Step 1
            if (lengthA == 0)
                return lengthB;
            if (lengthB == 0)
                return lengthA;

            // Step 2
            for (var i = 0; i <= lengthA; matrix[i, 0] = i++)
                ;
            for (var j = 0; j <= lengthB; matrix[0, j] = j++)
                ;

            // Step 3
            for (var i = 1; i <= lengthA; i++)
            {
                //Step 4
                for (var j = 1; j <= lengthB; j++)
                {
                    // Step 5
                    cost = (stringB.Substring(j - 1, 1) == stringA.Substring(i - 1, 1) ? 0 : 1);

                    // Step 6
                    matrix[i, j] = System.Math.Min(System.Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
                }
            }

            // Step 7
            return matrix[lengthA, lengthB];
        }

        /// <summary>
        /// Returns TRUE if the given strings are different. Uses ordinal comparison for high-speed operation.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        
        public static bool IsNotSameAs(this string stringA, string stringB)
        {
            return (string.Equals(stringA, stringB, StringComparison.Ordinal)) == false;
        }

        public static bool IsNotSameAs(this string stringA, string stringB, bool ignoreCase)
        {
            return (string.Equals(stringA, stringB, StringComparison.OrdinalIgnoreCase)) == false;
        }

        public static bool AreDifferent(this string stringA, string stringB)
        {
            return (string.Equals(stringA, stringB, StringComparison.Ordinal)) == false;
        }

        /// <summary>
        /// Returns TRUE if the given strings are the same. Uses ordinal comparison for high-speed operation.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        
        public static bool IsSameAs(this string stringA, string stringB)
        {
            return (string.Equals(stringA, stringB, StringComparison.Ordinal));
        }

        public static bool IsSameAs(this string stringA, string stringB, bool ignoreCase)
        {
            return (string.Equals(stringA, stringB, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns TRUE if the string has a value
        /// </summary>
        /// <param name="value"></param>
        
        public static bool IsNotEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) == false;
        }

        /// <summary>
        /// Returns TRUE if the string is null or empty
        /// </summary>
        /// <param name="value"></param>
        
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsRichText(this string value)
        {
            return value.StartsWith("{\\rtf", StringComparison.OrdinalIgnoreCase);
        }
    }

}
