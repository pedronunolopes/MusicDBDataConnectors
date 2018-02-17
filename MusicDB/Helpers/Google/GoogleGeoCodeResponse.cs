using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Helpers.Google
{
    /// <summary>
    /// Googles maps response to geocoding query
    /// </summary>
    public class GoogleGeoCodeResponse
    {

        public string status { get; set; }
        public results[] results { get; set; }

        public address_component GetAddressComponent(string name)
        {
            return GetAddressComponent(0, name);
        }

        /// <summary>
        /// Gets an address component by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public address_component GetAddressComponent(int index, string name)
        {
            if (results != null && results.Length > 0)
            {
                foreach(address_component item in results[index].address_components)
                {
                    foreach (string itemName in item.types)
                    {
                        if (itemName == name)
                            return item;
                    }
                }
            }

            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class results
    {
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string[] types { get; set; }
        public address_component[] address_components { get; set; }
    }

    public class geometry
    {
        public string location_type { get; set; }
        public location location { get; set; }
    }

    public class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class address_component
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }
}
