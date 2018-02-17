using MusicDB.Entities.Metadata;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Entities
{
    /// <summary>
    /// Song 
    /// </summary>
    public partial class Song : BaseEntity
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

        // Get AlbumId
        const string _strGetSongIdByName = "SELECT dbo.t_MUSong.SongId FROM dbo.t_MUSong INNER JOIN dbo.t_MUAlbumSong ON dbo.t_MUAlbumSong.SongId = dbo.t_MUSong.SongId WHERE dbo.t_MUSong.Name = '{0}' AND dbo.t_MUAlbumSong.AlbumId = {1}";

        private int? GetSongId(string strSongName, Album album)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetSongIdByName, strSongName.Replace("'", "''"), album.Id.ToString());
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
        public void Commit(Album album)
        {
            // commits data into the database
            CreateOrUpdate(album);

            // creates the association between artist and album
            if (album != null)
                InsertUpdateAlbumSongRelationInDB(album);

        }

        const string _sqlCreateAlbumSong = "INSERT INTO [dbo].[t_MUAlbumSong] ([AlbumId], [SongId]) VALUES (@AlbumId, @SongId)";
        const string _sqlCheckExistingAlbumSong = "SELECT COUNT(1) FROM [dbo].[t_MUAlbumSong] WHERE (AlbumId = @AlbumId AND SongId = @SongId)";

        /// <summary>
        /// Inserts music style in artist database
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        private void InsertUpdateAlbumSongRelationInDB(Album album)
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@SongId", Id.ToString());
            parameters.Add("@AlbumId", album.Id.ToString());

            if (dBHelper.GetScalarIntValue(_sqlCheckExistingAlbumSong, parameters) == 0)
            {
                dBHelper.InsertUpdateRow(_sqlCreateAlbumSong, parameters);
            }
        }

        /// <summary>
        /// Updates the artists in the database
        /// </summary>
        private void CreateOrUpdate(Album album)
        {
            int? testId = null;

            // checks if the artist already exists
            if (Id == 0 && Name != null)
            {
                testId = GetSongId(Name, album);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateSongInDB(album);
            else
                UpdateSongInDB();
        }

        const string _sqlUpdateSong = "UPDATE dbo.t_MUSong Set NAME = ISNULL(@Name, NAME), LENGTH = ISNULL(@Length, LENGTH) WHERE SongId = @SongId";

        /// <summary>
        /// Updates and Artist in the database
        /// </summary>
        private void UpdateSongInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Length", Length.ToString());
            parameters.Add("@SongId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateSong, parameters);

        }

        const string _sqlCreateSong = "INSERT INTO dbo.t_MUSong (NAME, LENGTH) OUTPUT INSERTED.SongId VALUES (@Name, @Length)";
      
        /// <summary>
        /// Creates the artist in the database
        /// </summary>
        private void CreateSongInDB(Album album)
        {

            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@Length", Length.ToString());
 
            Id = dBHelper.InsertUpdateRow(_sqlCreateSong, parameters, "@Id");

        }
    }
}
