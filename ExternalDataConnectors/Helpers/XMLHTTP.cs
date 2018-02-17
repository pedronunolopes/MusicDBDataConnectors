using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;

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
        public async Task<XmlDocument> DoGetRequestAsync(string strUrl, Dictionary<string, string> parameteres)
        {
            XmlDocument xmlDoc = null;

            try
            {
                if(parameteres == null || parameteres.Count > 0)
                {
                    strUrl = strUrl + "?";

                    foreach (string parameter in parameteres.Keys)
                    {
                        strUrl = strUrl + parameter + "=" + parameteres[parameter];
                    }
                }

                var responseString = await client.GetStringAsync(strUrl);

                xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(responseString);

            }
            catch (Exception)
            {

                throw;
            }
            
            return xmlDoc;

        }
    }
}