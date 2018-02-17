using MusicDB.Entities.Helpers;
using MusicDB.Entities.Metadata;
using MusicDB.Helpers;
using MusicDB.Helpers.Google;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Entities
{
    /// <summary>
    /// Event 
    /// </summary>
    public partial class Event : BaseEntity
    {
        /// <summary>
        /// Initializes from database By Id
        /// </summary>
        protected override void InitFromDatabaseById()
        {
        }

        /// <summary>
        /// Initializes from database By Id
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
        }

        // Get ArtistId
        const string _strGetEventIdByName = "SELECT dbo.t_MUEvent.EventId FROM dbo.t_MUEvent INNER JOIN dbo.t_MUArtistEvent ON dbo.t_MUArtistEvent.EventId = dbo.t_MUEvent.EventId WHERE dbo.t_MUEvent.Name = '{0}' AND dbo.t_MUArtistEvent.ArtistId = {1} AND dbo.t_MUEvent.Date = '{2}'";

        private int? GetEventId(string strEventName, Artist Artist)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetEventIdByName, strEventName.Replace("'", "''"), Artist.Id.ToString(), Date.ToString("yyyy-MM-dd HH:mm:ss"));
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        /// <summary>
        /// Commit entity to database
        /// </summary>
        public override void Commit()
        {
            Commit(null);
        }

        /// <summary>
        /// Commit entity to database
        /// </summary>
        public void Commit(Artist artist)
        {
            // commits data into the database
            CreateOrUpdate(artist);

            // creates the association between artist and Artist
            if (artist != null)
                InsertUpdateArtistEventRelationInDB(artist);

        }

        const string _sqlCreateArtistEvent = "INSERT INTO [dbo].[t_MUArtistEvent] ([ArtistId], [EventId]) VALUES (@ArtistId, @EventId)";
        const string _sqlCheckExistingArtistEvent = "SELECT COUNT(1) FROM [dbo].[t_MUArtistEvent] WHERE (ArtistId = @ArtistId AND EventId = @EventId)";

        /// <summary>
        /// Inserts music style in artist database
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        private void InsertUpdateArtistEventRelationInDB(Artist Artist)
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@EventId", Id.ToString());
            parameters.Add("@ArtistId", Artist.Id.ToString());

            if (dBHelper.GetScalarIntValue(_sqlCheckExistingArtistEvent, parameters) == 0)
            {
                dBHelper.InsertUpdateRow(_sqlCreateArtistEvent, parameters);
            }
        }

        /// <summary>
        /// Updates the artists in the database
        /// </summary>
        private void CreateOrUpdate(Artist Artist)
        {
            int? testId = null;

            // checks if the artist already exists
            if (Id == 0 && Name != null)
            {
                testId = GetEventId(Name, Artist);
                if (testId != null)
                    Id = testId.Value;
            }


            // HARCODE - DO NOT ALLOW VENUE EDITION (EXCEPT LATITUDE AND LONGITUDE COORDINATES)
            if (Venue.Id != 0)
            {
                Venue tempVenue = new Venue(Venue.Id);
                tempVenue.InitFromDB();
                tempVenue.Latitude = Venue.Latitude;
                tempVenue.Longitude = Venue.Longitude;
                tempVenue.City = (tempVenue.City != null ? tempVenue.City : this.City);

                tempVenue.Commit();
            } else
                Venue = null;

            if (Id == 0)
                CreateEventInDB(Artist);
            else
                UpdateEventInDB();
        }

        const string _sqlUpdateEvent = "UPDATE dbo.t_MUEvent Set NAME = ISNULL(@Name, NAME), Description = ISNULL(@Description, Description), VenueId = ISNULL(@VenueId, VenueId), DATE = ISNULL(@Date, Date), CITY = ISNULL(@City, City), EventTypeId = ISNULL(@EventTypeId, EventTypeId) WHERE EventId = @EventId";

        /// <summary>
        /// Updates and Artist in the database
        /// </summary>
        private void UpdateEventInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Description", Description);
            parameters.Add("@Date", Date.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("@EventTypeId", Type.Id);
            parameters.Add("@EventId", Id.ToString());
            parameters.Add("@City", City);
            parameters.Add("@VenueId", (Venue != null ? (object)Venue.Id : null));


            dBHelper.InsertUpdateRow(_sqlUpdateEvent, parameters);

        }

        const string _sqlCreateEvent = "INSERT INTO dbo.t_MUEvent (NAME, EVENTTYPEID, DATE, CITY, DESCRIPTION, VENUEID) OUTPUT INSERTED.EventId VALUES (@Name, @EventTypeId, @Date, @City, @Description, @VenueId)";
      
        /// <summary>
        /// Creates the artist in the database
        /// </summary>
        private void CreateEventInDB(Artist Artist)
        {

            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Description", Description);
            parameters.Add("@EventTypeId", Type.Id);
            parameters.Add("@Date", Date.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add("@City", City);
            parameters.Add("@VenueId", (Venue != null ? (object)Venue.Id : null));


            Id = dBHelper.InsertUpdateRow(_sqlCreateEvent, parameters, "@Id");

        }
    }
}
