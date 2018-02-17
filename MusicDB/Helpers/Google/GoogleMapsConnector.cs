using MusicDB.Entities.Helpers;
using MusicDB.ExternalData.Connectors.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Helpers.Google
{
    /// <summary>
    /// 
    /// </summary>
    public class GoogleMapsConnector
    {
        // base URL for google maps
        const string _baseURL = "https://maps.googleapis.com/maps/api/geocode/json";

        // google maps api key
        const string _apiKey = "AIzaSyC2AIeJYW_ULOWT0C59pXWA9j-L-ZoxvNc";

        // dictionary with the base parameters
        Dictionary<string, string> _baseParameters = null;

        public GoogleMapsConnector()
        {
            InitRequestParameters();
        }

        /// <summary>
        /// Initializes the base parameters array
        /// </summary>
        private void InitRequestParameters()
        {
            _baseParameters = new Dictionary<string, string>();

            _baseParameters.Add("key", _apiKey);

        }

        /// <summary>
        /// Gets the parameteres to get an artist
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetGeoLocationQueryParameteres(string address)
        {

            Dictionary<string, string> parameteres = new Dictionary<string, string>(_baseParameters);

            parameteres.Add("address", address);

            return parameteres;
        }

        /// <summary>
        /// Gets a location of an address from Google Maps
        /// </summary>
        /// <param name="strAddress"></param>
        /// <returns></returns>
        public Location GetLocation(string strAddress)
        {
            Location resultLocation = null;
            XmlHttpRequest xmlHttp = new XmlHttpRequest();
            Dictionary<string, string> parameters = GetGeoLocationQueryParameteres(strAddress);
            Task<string> strJSONResponse = null;

            try
            {
                // does the http request 
                strJSONResponse = xmlHttp.DoGetRequestAsync(_baseURL, parameters);

                GoogleGeoCodeResponse googleResponse = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(strJSONResponse.Result);

                if (googleResponse.status == "OK")
                {
                    if (googleResponse.results.Length == 1)
                    {
                        resultLocation = new Location();
                        resultLocation.City = (googleResponse.GetAddressComponent("locality") != null ?
                                                    googleResponse.GetAddressComponent("locality").long_name :
                                                    (
                                                        googleResponse.GetAddressComponent("postal_town") != null ?
                                                            googleResponse.GetAddressComponent("postal_town").long_name :
                                                            null)
                                                );

                        resultLocation.Latitude = decimal.Parse(googleResponse.results[0].geometry.location.lat.Replace(".", ","));
                        resultLocation.Longitude = decimal.Parse(googleResponse.results[0].geometry.location.lng.Replace(".", ","));
                        resultLocation.Address1 = googleResponse.results[0].formatted_address;
                        resultLocation.Country = googleResponse.GetAddressComponent("country").long_name;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }




            return resultLocation;
        }
    }
}
