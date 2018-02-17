using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicDB.ExternalData.Connectors.Helpers
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
    }
}
