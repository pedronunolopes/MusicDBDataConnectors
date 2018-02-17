using MusicDB.Entities.Metadata;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Entities
{
    /// <summary>
    /// Artist 
    /// </summary>
    public partial class Artist : BaseEntity
    {
        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
            int? testId = null;

            if (_id == 0 && Name != null)
            {
                testId = GetArtistId(Name);
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
                testName = GetArtistName(Id);
                if (testName != null)
                    Name = testName;
            }
        }

        // Get ArtistId
        const string _strGetArtistIdByName = "SELECT ArtistId FROM dbo.t_MUArtist WHERE Name = '{0}'";

        private int? GetArtistId(string strArtistName)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetArtistIdByName, strArtistName.Replace("'", "''"));
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        // Get ArtistId
        const string _strGetArtistIdById = "SELECT Name FROM dbo.t_MUArtist WHERE ArtistId = '{0}'";

        private string GetArtistName(int artistId)
        {
            string result = null;
            string sqlCommand = string.Format(_strGetArtistIdById, artistId);
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

            foreach(Artist artist in RelatedArtists.Values)
            {
                // Commits all related artists
                if (artist.Logo != null)
                {
                    artist.Commit();
                    InsertUpdateArtistRelationInDB(artist);
                }
            }

            foreach (MusicStyle musicStyle in MusicStyles.Values)
            {
                // Commits all related music styles
                musicStyle.Commit();
                InsertUpdateArtistMusicStyleInDB(musicStyle);
  
            }

            foreach (Album album in Albums.Values)
            {
                // Commits all albums
                album.Commit(this);

            }

            // registers the logo in the database
            if (Logo != null)
            {
                Logo.Commit();
            }
        }

        const string _sqlCreateArtistMusicStyle = "INSERT INTO [dbo].[t_MUArtistMusicStyle] ([ArtistId], [MusicStyleId]) VALUES (@ArtistId, @MusicStyleId)";
        const string _sqlCheckExistingArtistMusicStyle = "SELECT COUNT(1) FROM [dbo].[t_MUArtistMusicStyle] WHERE (ArtistId = @ArtistId AND MusicStyleId = @MusicStyleId)";

        /// <summary>
        /// Inserts music style in artist database
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        private void InsertUpdateArtistMusicStyleInDB(MusicStyle musicStyle)
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ArtistId", Id.ToString());
            parameters.Add("@MusicStyleId", musicStyle.Id.ToString());

            if (dBHelper.GetScalarIntValue(_sqlCheckExistingArtistMusicStyle, parameters) == 0)
            {
                dBHelper.InsertUpdateRow(_sqlCreateArtistMusicStyle, parameters);
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
                testId = GetArtistId(Name);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateArtistInDB();
            else
                UpdateArtistInDB();
        }

        const string _sqlUpdateArtist = "UPDATE dbo.t_MUArtist Set NAME = ISNULL(@Name, NAME), DESCRIPTION = ISNULL(@Description, DESCRIPTION) WHERE ArtistId = @ArtistId";

        /// <summary>
        /// Updates and Artist in the database
        /// </summary>
        private void UpdateArtistInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Description", Description);
            parameters.Add("@ArtistId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateArtist, parameters);

        }

        const string _sqlCreateArtist = "INSERT INTO dbo.t_MUArtist (NAME, DESCRIPTION) VALUES (@Name, @Description)";
      
        /// <summary>
        /// Creates the artist in the database
        /// </summary>
        private void CreateArtistInDB()
        {

            int? testId = null;
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Description", Description);

            dBHelper.InsertUpdateRow(_sqlCreateArtist, parameters);

            // Updates the ArtistId in the context class
            testId = GetArtistId(Name);
            if (testId != null)
                Id = testId.Value;
        }

        const string _sqlCreateArtistRelationship = "INSERT INTO [dbo].[t_MUArtistCorrelation] ([ArtistId1], [ArtistId2]) VALUES (@ArtistId1, @ArtistId2)";
        const string _sqlCheckExistingArtistRelationShip = "SELECT COUNT(1) FROM [dbo].[t_MUArtistCorrelation] WHERE (ArtistId1 = @ArtistId1 AND ArtistId2 = @ArtistId2) OR (ArtistId1 = @ArtistId2 AND ArtistId2 = @ArtistId2)";
        /// <summary>
        /// Updates and Artist in the database
        /// </summary>
        private void InsertUpdateArtistRelationInDB(Artist artist)
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ArtistId1", Id.ToString());
            parameters.Add("@ArtistId2", artist.Id.ToString());

            if (dBHelper.GetScalarIntValue(_sqlCheckExistingArtistRelationShip, parameters) == 0)
            {
                dBHelper.InsertUpdateRow(_sqlCreateArtistRelationship, parameters);
            }
        }
    }
}
