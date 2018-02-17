using MusicDB.Entities.Metadata;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MusicDB.Entities
{
    /// <summary>
    /// Venue 
    /// </summary>
    public partial class Venue : BaseEntity
    {
        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
            int? testId = null;

            if (_id == 0 && Name != null)
            {
                testId = GetVenueId(Name);
                if (testId != null)
                    Id = testId.Value;
            }
        }

        /// <summary>
        /// Initializes from database By Id
        /// </summary>
        protected override void InitFromDatabaseById()
        {
            string testName = null;

            if (_id > 0 && _ssName == null)
            {
                testName = GetVenueName(Id);
                if (testName != null)
                    Name = testName;
            }
        }

        // Get VenueId
        const string _strGetVenueIdByName = "SELECT VenueId FROM dbo.t_VNVenue WHERE LEFT(Name, LEN('{0}')) = '{0}' OR LEFT('{0}', LEN(NAME)) = NAME";
        const string _strGetVenueCountByName = "SELECT COUNT(1) FROM dbo.t_VNVenue WHERE LEFT(Name, LEN('{0}')) = '{0}' OR LEFT('{0}', LEN(NAME)) = NAME";

        private int? GetVenueId(string strVenueName)
        {
            int? result = null;
            string sqlCommandGetName = string.Format(_strGetVenueIdByName, strVenueName.Replace("'", "''"));
            string sqlCommandGetCount = string.Format(_strGetVenueCountByName, strVenueName.Replace("'", "''"));
            DBHelper dBHelper = new DBHelper();

            if (dBHelper.GetScalarIntValue(sqlCommandGetCount, null) == 1)
            {
                result = dBHelper.GetScalarIntValue(sqlCommandGetName);
            }

            return result;
        }

        // Get VenueId
        const string _strGetVenueIdById = "SELECT Name FROM dbo.t_VNVenue WHERE VenueId = '{0}'";

        private string GetVenueName(int VenueId)
        {
            string result = null;
            string sqlCommand = string.Format(_strGetVenueIdById, VenueId);
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarStringValue(sqlCommand);

            return result;
        }

        /// <summary>
        /// Commit entikty to database
        /// </summary>
        public override void Commit()
        {
            // commits data into the database
            CreateOrUpdate();

        }

        /// <summary>
        /// Updates the Venues in the database
        /// </summary>
        private void CreateOrUpdate()
        {
            int? testId = null;

            // checks if the Venue already exists
            if (Id == 0 && Name != null)
            {
                testId = GetVenueId(Name);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateVenueInDB();
            else
                UpdateVenueInDB();
        }

        const string _sqlUpdateVenue = "UPDATE dbo.t_VNVenue Set NAME = ISNULL(@Name, NAME), DESCRIPTION = ISNULL(@Description, DESCRIPTION),  COUNTRYID = ISNULL(@CountryId, COUNTRYID), CITY = ISNULL(@City, City), ADDRESS1 = ISNULL(@Address1, ADDRESS1), ADDRESS2 = ISNULL(@Address2, ADDRESS2), LATITUDE = ISNULL(@Latitude, LATITUDE), LONGITUDE = ISNULL(@Longitude, LONGITUDE) WHERE VenueId = @VenueId";

        /// <summary>
        /// Updates and Venue in the database
        /// </summary>
        private void UpdateVenueInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Description", Description);
            parameters.Add("@CountryId", Country.Id);
            parameters.Add("@City", City);
            parameters.Add("@Address1", Address1);
            parameters.Add("@Address2", Address2);
            parameters.Add("@Latitude", Latitude);
            parameters.Add("@Longitude", Longitude);

            parameters.Add("@VenueId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateVenue, parameters);

        }

        const string _sqlCreateVenue = "INSERT INTO dbo.t_VNVenue (NAME, DESCRIPTION, COUNTRYID, CITY, ADDRESS1, ADDRESS2, LATITUDE, LONGITUDE) VALUES (@Name, @Description, @CountryId, @City, @Address1, @Address2, @Latitude, @Longitude)";
      
        /// <summary>
        /// Creates the Venue in the database
        /// </summary>
        private void CreateVenueInDB()
        {

            int? testId = null;
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Description", Description);
            parameters.Add("@CountryId", Country.Id);
            parameters.Add("@City", City);
            parameters.Add("@Address1", Address1);
            parameters.Add("@Address2", Address2);
            parameters.Add("@Latitude", Latitude);
            parameters.Add("@Longitude", Longitude);

            dBHelper.InsertUpdateRow(_sqlCreateVenue, parameters);

            // Updates the VenueId in the context class
            testId = GetVenueId(Name);
            if (testId != null)
                Id = testId.Value;
        }


        const string _sqlInitVenue = "SELECT [VenueId], [Name], [Description], [CountryId], [City], [Address1], [Address2], [Location], [Latitude], [Longitude], [InsertDate], [UpdateDate] FROM [dbo].[t_VNVenue] WHERE VenueId = @VenueId";

        /// <summary>
        /// Updates and Venue in the database
        /// </summary>
        public void InitFromDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@VenueId", Id.ToString());

            dBHelper.InitRecord(_sqlInitVenue, parameters, ReadRecordFromDB, this);

        }

        const string _sqlListVenues = "SELECT t_VNVenue.[VenueId], t_VNVenue.[Name], t_VNVenue.[Description], t_VNVenue.[CountryId], t_VNVenue.[City], TabAddress.Field1 AS [Address1], t_VNVenue.[Address2], t_VNVenue.[Location], t_VNVenue.[Latitude], t_VNVenue.[Longitude], t_VNVenue.[InsertDate], t_VNVenue.[UpdateDate] FROM [dbo].[t_VNVenue] LEFT JOIN t_VNVenueContact ON t_VNVenueContact.VenueId = t_VNVenue.VenueId LEFT JOIN t_CTContact AS TabAddress ON TabAddress.ContactId = t_VNVenueContact.ContactId AND TabAddress.ContactTypeId = 4";

        /// <summary>
        /// Updates and Venue in the database
        /// </summary>
        public List<Venue> GetList()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = null;
            List<BaseEntity> venues = null;
            List<Venue> result = null;

            dBHelper.GetRecordList(_sqlListVenues, parameters, ReadRecordFromDB, NewRecord, out venues);
            
            if (venues != null && venues.Count > 0)
            {
                result = new List<Venue>();
                foreach (BaseEntity venue in venues)
                    result.Add((Venue)venue);

            }

            return result;
        }

        /// <summary>
        /// Reads a row to a venue record
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="venue"></param>
        public BaseEntity ReadRecordFromDB(SqlDataReader reader, BaseEntity venue)
        {
            // reads a row
            ((Venue)venue).Id = (int)(reader["VenueId"] != DBNull.Value ? int.Parse(reader["VenueId"].ToString()) : 0);
            ((Venue)venue).Name = (string)(reader["Name"] != DBNull.Value ? reader["Name"] : null);
            ((Venue)venue).Description = (string)(reader["Description"] != DBNull.Value ? reader["Description"] : null);
            ((Venue)venue).Country = new Country((int)(reader["CountryId"] != DBNull.Value ? int.Parse(reader["CountryId"].ToString()) : 0));
            ((Venue)venue).City = (string)(reader["City"] != DBNull.Value ? reader["City"] : null);
            ((Venue)venue).Address1 = (string)(reader["Address1"] != DBNull.Value ? reader["Address1"] : null);
            ((Venue)venue).Address2 = (string)(reader["Address2"] != DBNull.Value ? reader["Address2"] : null);
            ((Venue)venue).Latitude = (decimal)(reader["Latitude"] != DBNull.Value ? decimal.Parse(reader["Latitude"].ToString()) : 0);
            ((Venue)venue).Longitude = (decimal)(reader["Longitude"] != DBNull.Value ? decimal.Parse(reader["Longitude"].ToString()) : 0);
            ((Venue)venue).InsertDate = (DateTime)(reader["InsertDate"] != DBNull.Value ? reader["InsertDate"] : new DateTime());
            ((Venue)venue).UpdateDate = (DateTime)(reader["UpdateDate"] != DBNull.Value ? reader["UpdateDate"] : new DateTime());

            return venue;
        }

        /// <summary>
        /// Creates new instance delegate
        /// </summary>
        /// <returns></returns>
        public BaseEntity NewRecord()
        {
            return new Venue();
        }
    }
}
