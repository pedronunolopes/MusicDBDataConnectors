using MusicDB.Entities;
using MusicDB.Entities.Helpers;
using MusicDB.ExternalData.Helpers;
using MusicDB.Helpers.Google;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Gets Artist Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            int indexArtist = 0;
            string strArtistName = txtBoxArtistName.Text;
            MusicDB.ExternalData.Connectors.LastFM.DataConnector _connector = new MusicDB.ExternalData.Connectors.LastFM.DataConnector();
            Artist _resultArtist;
            Stack<Artist> _artistStack = new Stack<Artist>();
            Dictionary<string, Artist> _loadedArtists = new Dictionary<string, Artist>();

            _resultArtist = _connector.GetArtistByName(strArtistName);

            if (_resultArtist.Logo.Image != null)
                _resultArtist.Logo.Save(@"C:\Users\Pedro Lopes\Desktop\MusicDB\Artists\"+ strArtistName + ".jpg");

            _resultArtist.Commit();

            return;
            _artistStack.Push(_resultArtist);

            while (indexArtist <= 100 && _artistStack.Count > 0)
            {
                Artist currArtist = _artistStack.Pop();
                Artist testArtist = null;

                if (!_loadedArtists.TryGetValue(currArtist.Name, out testArtist)) {

                    textBox2.AppendText("Adding " + currArtist.Name);
                    _resultArtist = _connector.GetArtistByName(currArtist.Name);

                    if (_resultArtist.Logo != null)
                        _resultArtist.Commit();

                    if (currArtist.Logo != null)
                        currArtist.Logo.Save(@"C:\Users\Pedro Lopes\Desktop\MusicDB\Artists\" + currArtist.Name + ".jpg");


                    foreach (Artist artist in _resultArtist.RelatedArtists.Values)
                    {
                        _artistStack.Push(artist);
                    }
                    _loadedArtists.Add(_resultArtist.Name, _resultArtist);

                    indexArtist++;

                }
                else
                {
                    textBox2.AppendText("Skipping " + currArtist.Name + " : already exists");
                }

                textBox2.AppendText(Environment.NewLine);

            }
        }

        /// <summary>
        /// Gets Artist Event Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            string strArtistName = txtBoxArtistName.Text;
            MusicDB.ExternalData.Connectors.Eventful.DataConnector _connector = new MusicDB.ExternalData.Connectors.Eventful.DataConnector();
            Artist artist = new Artist(strArtistName);
            List<BaseEntity> events = new List<BaseEntity>();

            foreach(Event eventItem in _connector.GetEventsByArtist(artist))
            {
                eventItem.Commit(artist);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Venue venue = new Venue();

            List<Venue> venuesList = venue.GetList();

            foreach(Venue venueItem in venuesList)
            {
                if (venueItem.Latitude == 0 && venueItem.Longitude == 0)
                {
                    GoogleMapsConnector gmConn = new GoogleMapsConnector();
                    //Location location = gmConn.GetLocation(venueItem.Name + ',' + venueItem.Address1 + ',' + venueItem.Country.Name);
                    Location location = gmConn.GetLocation(venueItem.Name + ',' + venueItem.Country.Name);

                    if (location != null)
                    {
                        venueItem.Latitude = location.Latitude;
                        venueItem.Longitude = location.Longitude;
                        venueItem.City = (venueItem.City == null ? location.City : venueItem.City);

                        if (venueItem.Country.Name == location.Country)
                          venueItem.Commit();
                    }
                }
            }
        }
    }
}
