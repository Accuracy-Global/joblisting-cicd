
namespace JobPortal.Library.Utility
{
    public static class NumberFormat
    {
        static string[] _words =
        {
            "zero",
	        "first",
	        "second",
	        "third",
	        "fourth",
	        "fifth",
	        "sixth",
	        "seventh",
	        "eighth",
	        "ninth",
	        "tenth",
            "eleventh",
            "twelveth",
            "thirteenth",
            "fourteenth",
            "fifteenth",
            "sixteenth",
            "seventeenth",
            "eighteenth",
            "nineteenth",
            "twentieth",
            "twenty-first",
            "twenty-second",
            "twenty-third",
            "twenty-fourth",
            "twenty-fifth",
            "twenty-sixth",
            "twenty-seventh",
            "twenty-eighth",
            "twenty-ninth",
            "thirtieth"
        };

        private static string GetString(int value)
        {
            if (value >= 1 &&
                value <= 30)
            {
                return _words[value];
            }
            return value.ToString();
        }

        public static string NumberAbbreviations(int number)
        {
            return GetString(number);
            
        }
    }
}
