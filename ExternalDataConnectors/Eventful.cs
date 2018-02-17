using MusicDB.Entities;
using MusicDB.Entities.Metadata;
using MusicDB.ExternalData.Connectors.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace MusicDB.ExternalData.Connectors.Eventful
{
    public class DataConnector : IDataConnector
    {
        const string _performersDataBaseURL = "http://api.eventful.com/rest/performers/search";
        const string _eventsDataBaseURL = "http://api.eventful.com/rest/events/search";

        // API Key to access LastFM Database
        const string _apiKey = "TkPfP5w3hS5PpVsm";

        private string _artistKey = null;

        // dictionary with the base parameters
        Dictionary<string, string> _baseParameters = null;

        public DataConnector()
        {
            InitRequestParameters();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistName"></param>
        public Artist GetArtistByName(string strArtistName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistName"></param>
        public List<Event> GetEventsByArtist(Artist artist)
        {
            List<Event> eventsList = null;

            // first lets get the artis id from Eventful
            InitArtistKey(artist);

            if (_artistKey != null)
            {
                // loads the artists events
                eventsList = LoadEvents(artist);
            }

            return eventsList;
        }


        /// <summary>
        /// Loads up the artists events
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        private List<Event> LoadEvents(Artist artist)
        {
            int currentPage = 1;
            int totalPages = 1;
            XmlHttpRequest xmlHttp = new XmlHttpRequest();
            Dictionary<string, string> parameters = GetEventsInfoParameteres(artist.Name);
            XmlDocument xmlDoc = null;
            Task<string> strXmlStr = null;
            List<Event> eventList = new List<Event>();

            try
            {
                while (currentPage <= totalPages)
                {
                    // updates the page current parameter 
                    parameters["page_number"] = currentPage.ToString();

                    // does the http request 
                    strXmlStr = xmlHttp.DoGetRequestAsync(_eventsDataBaseURL, parameters);

                    // loads up de text into the XmlDocument
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(strXmlStr.Result);

                    // parses the XML Doc 
                    totalPages = ParseEventsXml(xmlDoc, ref eventList);

                    currentPage++;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return eventList;

        }

        /// <summary>
        /// Parses the events list XML
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="eventList"></param>
        /// <returns></returns>
        private int ParseEventsXml(XmlDocument xmlDoc, ref List<Event> eventList)
        {
            string elementValue = null;
            XmlNode node = null;
            int totalPages = 0;

            if (eventList == null)
                eventList = new List<Event>();

            // iterates over the events nodes of the selected artist
            foreach (XmlNode eventNode in xmlDoc.SelectNodes("//search/events/event[performers/performer/id='" + _artistKey +"']"))
            {
                Event eventItem = new Event();

                node = eventNode.SelectSingleNode("title");
                elementValue = (node != null ? node.InnerText : null);
                eventItem.Name = elementValue;

                node = eventNode.SelectSingleNode("description");
                elementValue = (node != null ? node.InnerText : null);
                eventItem.Description = elementValue;

                node = eventNode.SelectSingleNode("start_time");
                elementValue = (node != null ? node.InnerText : null);
                if (elementValue != null)
                    eventItem.Date = DateTime.Parse(elementValue);

                node = eventNode.SelectSingleNode("region_abbr");
                elementValue = (node != null ? node.InnerText : null);
                if (!string.IsNullOrEmpty(elementValue) && elementValue.Length > 3)
                    eventItem.City = elementValue;
                else
                {
                    node = eventNode.SelectSingleNode("city_name");
                    elementValue = (node != null ? node.InnerText : null);
                    eventItem.City = elementValue;
                }

                // Parses the Venue XML
                eventItem.Venue = ParseEventVenue(eventNode);

                // assumes all events are concerts
                eventItem.Type = new EventType(EventType.EventTypeEnum.Concert);

                // adds the new created event
                eventList.Add(eventItem);
            }

            node = xmlDoc.SelectSingleNode("//search/page_count");
            if (node != null)
                totalPages = int.Parse(node.InnerText);

            return totalPages;
        }

        /// <summary>
        /// Parses the venue XML contained in the event xml
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Venue ParseEventVenue(XmlNode eventXml)
        {
            Venue venue = new Venue();
            string elementValue = null;

            XmlNode node = null;

            node = eventXml.SelectSingleNode("venue_name");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null)
                venue.Name = elementValue;

            node = eventXml.SelectSingleNode("venue_address");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null)
                venue.Address1 = elementValue;

            node = eventXml.SelectSingleNode("city_name");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null)
                venue.City = elementValue;

            node = eventXml.SelectSingleNode("postal_code");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null)
                venue.Address2 = elementValue;

            node = eventXml.SelectSingleNode("country_name");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null)
                venue.Country = new Country(elementValue);

            node = eventXml.SelectSingleNode("latitude");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null)
                venue.Latitude = decimal.Parse(elementValue.Replace(".", ","));

            node = eventXml.SelectSingleNode("longitude");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null)
                venue.Longitude = decimal.Parse(elementValue.Replace(".", ","));

            return venue;
        }

        /// <summary>
        /// Initializes de Artist Id Before making the requests
        /// </summary>
        private void InitArtistKey(Artist artist)
        {
            XmlHttpRequest xmlHttp = new XmlHttpRequest();
            Dictionary<string, string> parameters = GetArtistInfoParameteres(artist.Name);
            XmlDocument xmlDoc = null;
            Task<string> strXmlStr = null;
            string externalArtistId = null;

            try
            {
                // does the http request 
                strXmlStr = xmlHttp.DoGetRequestAsync(_performersDataBaseURL, parameters);

                // loads up de text into the XmlDocument
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strXmlStr.Result);

                // parses the XML Doc 
                externalArtistId = ParseArtistXml(xmlDoc, artist);

                _artistKey = externalArtistId;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Parses the artist search XML in order to get the artist id
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        private string ParseArtistXml(XmlDocument xmlDoc, Artist artist)
        {
            string artistId = null;
            string elementValue = null;
            XmlNode node = null;

            node = xmlDoc.SelectSingleNode("//search/performers/performer[name='"+ artist.Name +"']");

            if (node != null)
            {
                node = node.SelectSingleNode("id");
                elementValue = (node != null ? node.InnerText : null);
                artistId = elementValue;
            }

            return artistId;

        }


        /// <summary>
        /// Gets the parameteres to get an artist
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetEventsInfoParameteres(string artistName)
        {

            Dictionary<string, string> parameteres = new Dictionary<string, string>(_baseParameters);

            parameteres.Add("keywords", artistName.ToLower());
            parameteres.Add("date", "");
            parameteres.Add("pagesize", "250");
            parameteres.Add("page_number", "1");

            return parameteres;
        }

        /// <summary>
        /// Gets the parameteres to get an artist
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetArtistInfoParameteres(string artistName)
        {

            Dictionary<string, string> parameteres = new Dictionary<string, string>(_baseParameters);

            parameteres.Add("keywords", artistName.ToLower());

            return parameteres;
        }

        /// <summary>
        /// initializes the parameteres used across all types of requests to LastFM database
        /// </summary>
        private void InitRequestParameters()
        {
            _baseParameters = new Dictionary<string, string>();

            _baseParameters.Add("app_key", _apiKey);

        }

    }
}
