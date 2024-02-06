using JobPortal.Library.Security;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace JobPortal.Library.Utility
{
    /// <summary>
    /// Custom string utility methods.
    /// </summary>
    /// 
    public static class StringTool
    {
        /// <summary>
        /// Get a substring of the first N characters.
        /// </summary>
        public static string Truncate(string source, int length)
        {
            return Truncate(source, length, string.Empty);
        }

        /// <summary>
        /// Get a substring of the first N characters and replaces extra characters with replaceExtraWith.
        /// </summary>
        public static string Truncate(string source, int length, string replaceExtraWith)
        {
            if (source.Length > length)
                source = source.Substring(0, length) + replaceExtraWith;
            return source;
        }

        public static string RemoveSpecialCharacters(string str, string replaceWith)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, "[^a-zA-Z0-9_]+", replaceWith, System.Text.RegularExpressions.RegexOptions.Compiled);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return RemoveSpecialCharacters(str, "_");
        }

        public static string AddSpacesToSentence(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static string ToSentenseCase(this string text)
        {
            // start by converting entire string to lower case
            var lowerCase = text.ToLower();
            // matches the first sentence of a string, as well as subsequent sentences
            var r = new System.Text.RegularExpressions.Regex(@"(^[a-z])|\.\s+(.)", System.Text.RegularExpressions.RegexOptions.ExplicitCapture);
            // MatchEvaluator delegate defines replacement of setence starts to uppercase
            var result = r.Replace(lowerCase, s => s.Value.ToUpper());

            return result;
        }

        public static string ToTitleCase(this string text)
        {
            return new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(text.ToLower());
        }

        public static string RemoveHtmlTags(string htmlText)
        {
            Regex rx = new Regex("<[^>]*>");

            return rx.Replace(htmlText, "").Trim();
        }
        
        public static string RemoveEncodedHtmlTags(string htmlText)
        {
            Regex rx = new Regex("&lt;[^&gt;]*&gt;");

            return rx.Replace(htmlText, "").Trim();
        }

        /// <summary>
        /// Truncate job detail on job search page.
        /// </summary>
        /// <param name="htmlText">Job detail text</param>
        /// <param name="length">Number of characters to show</param>
        /// <returns></returns>
        public static string TruncateJobDetail(string htmlText, int length )
        {
            string nonHtmlText = RemoveHtmlTags(htmlText).Replace("&nbsp;", "").Trim();
            nonHtmlText = nonHtmlText.Replace("&#160;", "").Trim();
            nonHtmlText = RemoveEncodedHtmlTags(nonHtmlText).Trim();

            if (nonHtmlText.Length <= length)
                return nonHtmlText;
            else
            {
                string jobDetail = Truncate(nonHtmlText, length);

                return string.Format("{0}...", jobDetail);
            }

        }

        public static string GenerateEncryptedExpireDate(DateTime lastChangedPasswordDate)
        {
            return Crypto.EncryptText(lastChangedPasswordDate.ToFileTime().ToString());
        }
    }
}
