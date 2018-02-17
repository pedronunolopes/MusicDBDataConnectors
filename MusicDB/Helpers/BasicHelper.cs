using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MusicDB.ExternalData.Helpers
{
    public static class BasicHelper
    {
        public static string ToTitleCase(string stringToConvert)
        {
            //Get the culture property of the thread.
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            //Create TextInfo object.
            TextInfo textInfo = cultureInfo.TextInfo;

            return textInfo.ToTitleCase(stringToConvert);
        }

        /// <summary>
        /// Get URL FileName
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string GetURLFilename(string strUrl)
        {
            string filename = null;
            Uri uri = new Uri(strUrl);

            filename = System.IO.Path.GetFileName(uri.LocalPath);

            return filename;
        }

        /// <summary>
        /// Get URL FileName
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string GetURLFilenameExtension(string strUrl)
        {
            string extension = null;
            Uri uri = new Uri(strUrl);

            extension = Path.GetExtension( System.IO.Path.GetFileName(uri.LocalPath));

            return extension;
        }

        // erase html tags from a string
        public static string StripHtml(string target)
        {
            //Regular expression for html tags
            Regex StripHTMLExpression = new Regex("<a.*?</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            return StripHTMLExpression.Replace(target, string.Empty);
        }

        // erase html tags from a string
        public static string StripNonAlphaNumeric(string target)
        {
            //Regular expression for html tags
            Regex StripExpression = new Regex("[^a-zA-Z0-9-]", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            return StripExpression.Replace(target, string.Empty);
        }
    }
}
