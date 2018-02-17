using MusicDB.Entities.Metadata;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Entities
{
    /// <summary>
    /// Album 
    /// </summary>
    public partial class Album : BaseEntity
    {
        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
            int? testId = null;

            if (_id == 0 && Name != null)
            {
                testId = GetAlbumId(Name);
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
                testName = GetAlbumName(Id);
                if (testName != null)
                    Name = testName;
            }
        }

        // Get AlbumId
        const string _strGetAlbumIdByName = "SELECT AlbumId FROM dbo.t_MUAlbum WHERE Name = '{0}'";

        private int? GetAlbumId(string strAlbumName)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetAlbumIdByName, strAlbumName.Replace("'", "''"));
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        // Get AlbumId
        const string _strGetAlbumIdById = "SELECT Name FROM dbo.t_MUAlbum WHERE AlbumId = '{0}'";

        private string GetAlbumName(int AlbumId)
        {
            string result = null;
            string sqlCommand = string.Format(_strGetAlbumIdById, AlbumId);
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarStringValue(sqlCommand);

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
            CreateOrUpdate();

            // creates the association between artist and album
            if (artist != null)
                InsertUpdateArtistAlbumRelationInDB(artist);

            foreach (Song song in Songs.Values)
            {   
                song.Commit(this);
            }

            // registers the logo in the database
            if (Logo != null)
                Logo.Commit();
        }

        const string _sqlCreateArtistAlbum = "INSERT INTO [dbo].[t_MUArtistAlbum] ([ArtistId], [AlbumId]) VALUES (@ArtistId, @AlbumId)";
        const string _sqlCheckExistingArtistAlbum = "SELECT COUNT(1) FROM [dbo].[t_MUArtistAlbum] WHERE (ArtistId = @ArtistId AND AlbumId = @AlbumId)";

        /// <summary>
        /// Inserts music style in artist database
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        private void InsertUpdateArtistAlbumRelationInDB(Artist artist)
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ArtistId", artist.Id.ToString());
            parameters.Add("@AlbumId", Id.ToString());

            if (dBHelper.GetScalarIntValue(_sqlCheckExistingArtistAlbum, parameters) == 0)
            {
                dBHelper.InsertUpdateRow(_sqlCreateArtistAlbum, parameters);
            }
        }

        /// <summary>
        /// Updates the artists in the database
        /// </summary>
        private void CreateOrUpdate()
        {
            int? testId = null;

            // checks if the artist already exists
            if (Id == 0 && Name != null)
            {
                testId = GetAlbumId(Name);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateAlbumInDB();
            else
                UpdateAlbumInDB();
        }

        const string _sqlUpdateAlbum = "UPDATE dbo.t_MUAlbum Set NAME = ISNULL(@Name, NAME), DESCRIPTION = ISNULL(@Description, DESCRIPTION) WHERE AlbumId = @AlbumId";

        /// <summary>
        /// Updates and Artist in the database
        /// </summary>
        private void UpdateAlbumInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Description", Description);
            //parameters.Add("@Logo", (Logo != null ? Logo.MediaBinary : null));
            parameters.Add("@AlbumId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateAlbum, parameters);

        }

        const string _sqlCreateAlbum = "INSERT INTO dbo.t_MUAlbum (NAME, DESCRIPTION) VALUES (@Name, @Description)";
      
        /// <summary>
        /// Creates the artist in the database
        /// </summary>
        private void CreateAlbumInDB()
        {

            int? testId = null;
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Byte[] imageByteArray = (Logo != null ? Logo.MediaBinary : null);

            parameters.Add("@Name", Name);
            parameters.Add("@Description", Description);
            //parameters.Add("@Logo", imageByteArray);


            dBHelper.InsertUpdateRow(_sqlCreateAlbum, parameters);

            // Updates the ArtistId in the context class
            testId = GetAlbumId(Name);
            if (testId != null)
                Id = testId.Value;
        }
    }
}
