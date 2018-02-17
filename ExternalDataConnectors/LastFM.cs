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

namespace MusicDB.ExternalData.Connectors.LastFM
{
    public class DataConnector : IDataConnector
    {
        const string _baseURL = "https://ws.audioscrobbler.com/2.0/";

        // API Key to access LastFM Database
        const string _apiKey = "59129daa5bccd7f1604b7e0337780a01";

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
        public List<Event> GetEventsByArtist(Artist artist)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistName"></param>
        public Artist GetArtistByName(string strArtistName)
        {
            Artist resultArtist = null;
            XmlHttpRequest xmlHttp = new XmlHttpRequest();
            Dictionary<string, string> parameters = GetArtistInfoParameteres(strArtistName.ToLower());
            XmlDocument xmlDoc = null;
            Task<string> strXmlStr = null;

            try
            {
                // does the http request 
                strXmlStr = xmlHttp.DoGetRequestAsync(_baseURL, parameters);

                // loads up de text into the XmlDocument
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strXmlStr.Result);

                // parses the XML Doc into the artist structure
                resultArtist = ParseArtistXml(xmlDoc);
            }
            catch (Exception ex) {

                throw ex;
            }


            return resultArtist;
        }

        /// <summary>
        /// Parses LastFM Xml into an Artist Object
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private Artist ParseArtistXml(XmlDocument artistXml)
        {
            string elementValue = null;
            XmlNode node = null;
            Artist artistResult = new Artist();

            node = artistXml.SelectSingleNode("//artist/name");
            elementValue = (node != null ? node.InnerText : null);
            artistResult.Name = elementValue;

            node = artistXml.SelectSingleNode("//artist/image[@size='extralarge']");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null && !string.IsNullOrEmpty(elementValue))
            {
                artistResult.Logo = new Media(artistResult, new MediaType(MediaType.MediaEnum.Logo), elementValue);
                artistResult.Logo.FileName = MusicDB.ExternalData.Helpers.BasicHelper.StripNonAlphaNumeric(artistResult.Name) + ".png";
            }

            node = artistXml.SelectSingleNode("//artist/bio/summary");
            elementValue = (node != null ? MusicDB.ExternalData.Helpers.BasicHelper.StripHtml(node.InnerText) : null);
            artistResult.Description = elementValue;

            node = artistXml.SelectSingleNode("//artist/bio/content");
            elementValue = (node != null ? MusicDB.ExternalData.Helpers.BasicHelper.StripHtml(node.InnerText) : null);
            artistResult.History = elementValue;

            foreach(XmlNode nodeData in artistXml.SelectNodes("//artist/similar/artist"))
            {
                Artist similarArtist = new Artist();

                node = nodeData.SelectSingleNode("name");
                elementValue = (node != null ? node.InnerText : null);
                similarArtist.Name = elementValue;

                node = nodeData.SelectSingleNode("image[@size='extralarge']");
                elementValue = (node != null ? node.InnerText : null);
                if (elementValue != null && !string.IsNullOrEmpty(elementValue))
                {
                    similarArtist.Logo = new Media(similarArtist, new MediaType(MediaType.MediaEnum.Logo), elementValue);
                    similarArtist.Logo.FileName = MusicDB.ExternalData.Helpers.BasicHelper.StripNonAlphaNumeric(similarArtist.Name) + ".png";
                }

                artistResult.RelatedArtists.Add(similarArtist.Name, similarArtist);
            }

            foreach (XmlNode nodeData in artistXml.SelectNodes("//artist/tags/tag"))
            {
                MusicStyle musicStyle = null;

                node = nodeData.SelectSingleNode("name");
                elementValue = (node != null ? node.InnerText : null);
                musicStyle = new MusicStyle(elementValue);

                artistResult.MusicStyles.Add(musicStyle.Name, musicStyle);
            }

            // loads from last FM the artist's albums
            LoadArtistAlbums(artistResult);

            return artistResult;
        }

        /// <summary>
        /// Loads the artist's albums from lastFM
        /// </summary>
        /// <param name="artistResult"></param>
        private void LoadArtistAlbums(Artist artist)
        {

            XmlHttpRequest xmlHttp = new XmlHttpRequest();
            Dictionary<string, string> parameters = GetArtistAlbumsInfoParameteres(artist);
            XmlDocument xmlDoc = null;
            Task<string> strXmlStr = null;

            try
            {
                // does the http request 
                strXmlStr = xmlHttp.DoGetRequestAsync(_baseURL, parameters);

                // loads up de text into the XmlDocument
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strXmlStr.Result);

                // parses the XML Doc into the artist structure
                ParseArtistAlbumsXml(artist, xmlDoc);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        private void ParseArtistAlbumsXml(Artist artist, XmlDocument xmlDoc)
        {
            string elementValue = null;
            XmlNodeList albumNodes = null;
            XmlNode node = null;

            albumNodes = xmlDoc.SelectNodes("//lfm/topalbums/album/name");

            // loads albums from XML
            foreach(XmlNode albumNode in albumNodes)
            {
                Album album = new Album();
                elementValue = (albumNode != null ? albumNode.InnerText : null);
                album.Name = elementValue;

                node = albumNode.SelectSingleNode("../image[@size='extralarge']");
                elementValue = (node != null ? node.InnerText : null);
                if (elementValue != null && !string.IsNullOrEmpty(elementValue))
                {
                    album.Logo = new Media(album, new MediaType(MediaType.MediaEnum.AlbumCover) ,elementValue);
                    album.Logo.FileName = MusicDB.ExternalData.Helpers.BasicHelper.StripNonAlphaNumeric(album.Name)+".png";
                }


                artist.Albums.Add(album.Name, album);

            }

            // loads album info from last fm
            foreach(Album album in artist.Albums.Values)
            {
                LoadAlbumInfo(artist, album);
            }

        }

        /// <summary>
        /// Loads the artist's albums from lastFM
        /// </summary>
        /// <param name="artistResult"></param>
        private void LoadAlbumInfo(Artist artist, Album album)
        {

            XmlHttpRequest xmlHttp = new XmlHttpRequest();
            Dictionary<string, string> parameters = GetAlbumInfoParameteres(artist, album);
            XmlDocument xmlDoc = null;
            Task<string> strXmlStr = null;

            try
            {
                // does the http request 
                strXmlStr = xmlHttp.DoGetRequestAsync(_baseURL, parameters);

                // loads up de text into the XmlDocument
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strXmlStr.Result);

                // parses the XML Doc into the artist structure
                ParseAlbumXml(artist, album, xmlDoc);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        private void ParseAlbumXml(Artist artist, Album album, XmlDocument xmlDoc)
        {            XmlNodeList trackNodes = null;

            string elementValue = null;
            XmlNode node = null;

            node = xmlDoc.SelectSingleNode("//lfm/album/wiki/summary");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null)
                album.Description = MusicDB.ExternalData.Helpers.BasicHelper.StripHtml(elementValue);

            node = xmlDoc.SelectSingleNode("//lfm/album/image[@size='extralarge']");
            elementValue = (node != null ? node.InnerText : null);
            if (elementValue != null && !string.IsNullOrEmpty(elementValue))
            {
                album.Logo = new Media(album, new MediaType(MediaType.MediaEnum.AlbumCover), elementValue);
                album.Logo.FileName = MusicDB.ExternalData.Helpers.BasicHelper.StripNonAlphaNumeric(album.Name) + ".png";
            }

            trackNodes = xmlDoc.SelectNodes("//lfm/album/tracks/track");

            foreach (XmlNode trackNode in trackNodes)
            {
                int duration;
                Song song = new Song();
                Song testSong = null;

                node = trackNode.SelectSingleNode("name");
                elementValue = (node != null ? node.InnerText : null);
                song.Name = elementValue;

                node = trackNode.SelectSingleNode("duration");
                elementValue = (node != null ? node.InnerText : null);
                if (Int32.TryParse(elementValue, out duration))
                    song.Length = TimeSpan.FromSeconds(duration);

                if (!album.Songs.TryGetValue(song.Name, out testSong))
                    album.Songs.Add(song.Name, song);

            }
        }

        /// <summary>
        /// initializes the parameteres used across all types of requests to LastFM database
        /// </summary>
        private void InitRequestParameters()
        {
            _baseParameters = new Dictionary<string, string>();

            _baseParameters.Add("api_key", _apiKey);

        }

        /// <summary>
        /// Gets the parameteres to get an artist
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetArtistInfoParameteres(string artistName)
        {

            Dictionary<string, string> parameteres = new Dictionary<string, string>(_baseParameters);

            parameteres.Add("method", "artist.getinfo");
            parameteres.Add("artist", artistName);

            return parameteres;
        }

        /// <summary>
        /// Gets the parameteres to get an artist
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetArtistAlbumsInfoParameteres(Artist artist)
        {

            Dictionary<string, string> parameteres = new Dictionary<string, string>(_baseParameters);

            parameteres.Add("method", "artist.gettopalbums");
            parameteres.Add("artist", artist.Name.ToLower());

            return parameteres;
        }

        /// <summary>
        /// Gets the parameteres to get an artist
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetAlbumInfoParameteres(Artist artist, Album album)
        {

            Dictionary<string, string> parameteres = new Dictionary<string, string>(_baseParameters);

            parameteres.Add("method", "album.getinfo");
            parameteres.Add("artist", artist.Name.ToLower());
            parameteres.Add("album", album.Name.ToLower());

            return parameteres;
        }
    }
}
