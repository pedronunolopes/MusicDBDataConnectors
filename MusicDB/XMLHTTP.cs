using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace MusicDB.ExternalData.Connectors.Helpers
{
    public class XmlHttpRequest
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Do a http get request
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="parameteres"></param>
        /// <returns></returns>
        public async Task<string> DoGetRequestAsync(string strUrl, Dictionary<string, string> parameteres)
        {
            try
            {
                if(parameteres == null || parameteres.Count > 0)
                {
                    bool firstParam = true;
                    strUrl = strUrl + "?";

                    foreach (string parameter in parameteres.Keys)
                    {
                        strUrl = strUrl + (firstParam?"":"&") + parameter + "=" + Uri.EscapeDataString(parameteres[parameter]);
                        firstParam = false;
                    }
                }

                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(strUrl).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;

                        // by calling .Result you are synchronously reading the result
                        string responseString = responseContent.ReadAsStringAsync().Result;

                        return responseString;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            
            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public Bitmap GetImage(string imageUrl)
        {

            using (WebClient client = new WebClient())
            {
                using (Stream stream = client.OpenRead(imageUrl))
                {
                    Bitmap bitmap;
                    bitmap = new Bitmap(stream);

                    stream.Flush();
                    stream.Close();
                    client.Dispose();

                    return bitmap;
                }
            }

        }
    }
}