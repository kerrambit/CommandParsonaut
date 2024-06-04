using System.Globalization;
using System.Text.RegularExpressions;

namespace CommandParsonaut
{
    /// <summary>
    /// Offers basic functionality for string input parsing.
    /// </summary>
    public static class InputParser
    {
        private readonly static char[] DELIMITERS = { ' ', '\t' };

        public static string[] SplitInput(in string input, bool preserveTextInDoubleQuotes = false)
        {
            if (preserveTextInDoubleQuotes)
            {
                return SplitInputWithPreservedTextInDoubleQuotes(input);
            }

            string[] tokens = input.Split(DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
            return tokens;
        }

        private static string[] SplitInputWithPreservedTextInDoubleQuotes(in string input)
        {
            List<string> tokens = new List<string>();

            string pattern = "\"([^\"]*)\"|\\S+";
            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                if (match.Groups[1].Success)
                {
                    tokens.Add(match.Groups[1].Value);
                }
                else
                {
                    tokens.Add(match.Value);
                }
            }

            return tokens.ToArray();
        }

        public static bool ParseInteger(in string token, out int result)
        {
            if (!int.TryParse(token, out result))
            {
                return false;
            }

            return true;
        }

        public static bool ParseIntegerInRange(in string token, int min, int max, out int result)
        {
            if (!int.TryParse(token, out result))
            {
                return false;
            }

            return result >= min && result <= max;
        }

        public static bool ParseDouble(in string token, out double result)
        {
            if (!double.TryParse(token, out result))
            {
                return false;
            }

            return true;
        }

        public static bool ParseDoubleInRange(in string token, double min, double max, out double result)
        {
            if (!double.TryParse(token, out result))
            {
                return false;
            }

            return result >= min && result <= max;
        }

        public static bool ParsePositiveInteger(in string token, out int result)
        {
            if (ParseInteger(token, out result))
            {
                return result > 0;
            }

            return false;
        }

        public static bool ParseDateTime(in string token, in string format, out DateTime result)
        {
            if (DateTime.TryParseExact(token, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return true;
            }

            return false;
        }
    }
}
