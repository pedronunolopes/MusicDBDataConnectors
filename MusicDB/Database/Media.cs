using MusicDB.Entities.Metadata;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicDB.Entities
{
    /// <summary>
    /// Media 
    /// </summary>
    public partial class Media : BaseEntity
    {

        /// <summary>
        /// Initializes from database By Id
        /// </summary>
        protected override void InitFromDatabaseById()
        {
        }

        // Get ArtistId
        const string _strGetMediaIdByArtist = "SELECT MediaId FROM dbo.t_MUArtistMedia WHERE ArtistId = {0} AND MediaTypeId = {1}";

        private int? GetArtistId(string strArtistName)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetMediaIdByArtist, BaseEntity.Id, MediaType.Id);
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        /// <summary>
        /// Commit entikty to database
        /// </summary>
        public override void Commit()
        {
            if (_id == 0)
                TryInitId(); 

            // commits data into the database
            CreateOrUpdate();

            if(BaseEntity is Artist)
            {
                InsertUpdateMediaArtistRelationInDB(this);
            }

            if (BaseEntity is Album)
            {
                InsertUpdateMediaAlbumRelationInDB(this);
            }
        }

        /// <summary>
        /// Tries to check if Media already exists in database before creating another
        /// </summary>
        private void TryInitId()
        {
            if (BaseEntity != null)
            {
                if (BaseEntity is Artist)
                {
                    Id = CheckMediaArtistRelationInDB();
                }

                if (BaseEntity is Album)
                {
                    Id = CheckMediaAlbumRelationInDB();
                }
            }
        }

        const string _sqlGetExistingAlbumMediaRelationShip = "SELECT MediaId FROM [dbo].[t_MUAlbumMedia] WHERE AlbumId = {0} AND MediaTypeId = {1}";

        /// <summary>
        /// Checks if multimedia exists for the artist
        /// </summary>
        /// <returns></returns>
        private int CheckMediaAlbumRelationInDB()
        {
            int? result = null;
            string sqlCommand = string.Format(_sqlGetExistingAlbumMediaRelationShip, BaseEntity.Id, MediaType.Id);
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return (result != null ? result.Value : 0);
        }
    
    const string _sqlGetExistingArtistMediaRelationShip = "SELECT MediaId FROM [dbo].[t_MUArtistMedia] WHERE ArtistId = {0} AND MediaTypeId = {1}";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int CheckMediaArtistRelationInDB()
        {
            int? result = null;
            string sqlCommand = string.Format(_sqlGetExistingArtistMediaRelationShip, BaseEntity.Id, MediaType.Id);
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return (result != null? result.Value : 0);
        }

        /// <summary>
        /// Updates the Medias in the database
        /// </summary>
        private void CreateOrUpdate()
        {
            if (Id == 0)
                CreateMediaInDB();
            else
                UpdateMediaInDB();
        }

        const string _sqlUpdateMedia = "UPDATE dbo.t_MTMedia Set FileName = ISNULL(@FileName, FileName), MimeType = ISNULL(@MimeType, MimeType), MediaFileTypeId = ISNULL(@MediaFileTypeId, MediaFileTypeId), FileBinary = ISNULL(@FileBinary, FileBinary) WHERE MediaId = @MediaId";

        /// <summary>
        /// Updates and Media in the database
        /// </summary>
        private void UpdateMediaInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@FileName", FileName);
            parameters.Add("@MimeType", MimeType);
            parameters.Add("@MediaFileTypeId", FileType.Id);
            parameters.Add("@FileBinary", (MediaBinary != null ? MediaBinary : null));
            parameters.Add("@MediaId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateMedia, parameters);

        }

        const string _sqlCreateMedia = "INSERT INTO dbo.t_MTMedia (FileName, MimeType, MediaFileTypeId,FileBinary)  OUTPUT INSERTED.MediaId VALUES (@FileName, @MimeType, @MediaFileTypeId, @FileBinary)";

        /// <summary>
        /// Creates the Media in the database
        /// </summary>
        private void CreateMediaInDB()
        {

            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@FileName", FileName);
            parameters.Add("@MimeType", MimeType);
            parameters.Add("@MediaFileTypeId", FileType.Id);
            parameters.Add("@FileBinary", (MediaBinary != null ? MediaBinary : null));

            Id = dBHelper.InsertUpdateRow(_sqlCreateMedia, parameters, "@Id");

        }

        const string _sqlCreateArtistMediaRelationship = "INSERT INTO [dbo].[t_MUArtistMedia] ([ArtistId], [MediaId], [MediaTypeId]) VALUES (@ArtistId, @MediaId, @MediaTypeId)";
        const string _sqlCheckExistingArtistMediaRelationShip = "SELECT COUNT(1) FROM [dbo].[t_MUArtistMedia] WHERE ArtistId = @ArtistId AND MediaId = @MediaId";

        /// <summary>
        /// Updates and Media in the database
        /// </summary>
        private void InsertUpdateMediaArtistRelationInDB(Media Media)
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ArtistId", BaseEntity.Id.ToString());
            parameters.Add("@MediaId", Id.ToString());
            parameters.Add("@MediaTypeId", MediaType.Id.ToString());

            if (dBHelper.GetScalarIntValue(_sqlCheckExistingArtistMediaRelationShip, parameters) == 0)
            {
                dBHelper.InsertUpdateRow(_sqlCreateArtistMediaRelationship, parameters);
            }
        }

        const string _sqlCreateAlbumMediaRelationship = "INSERT INTO [dbo].[t_MUAlbumMedia] ([AlbumId], [MediaId], [MediaTypeId]) VALUES (@AlbumId, @MediaId, @MediaTypeId)";
        const string _sqlCheckExistingAlbumMediaRelationShip = "SELECT COUNT(1) FROM [dbo].[t_MUAlbumMedia] WHERE AlbumId = @AlbumId AND MediaId = @MediaId";

        /// <summary>
        /// Updates and Media in the database
        /// </summary>
        private void InsertUpdateMediaAlbumRelationInDB(Media Media)
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@AlbumId", BaseEntity.Id.ToString());
            parameters.Add("@MediaId", Id.ToString());
            parameters.Add("@MediaTypeId", MediaType.Id.ToString());

            if (dBHelper.GetScalarIntValue(_sqlCheckExistingAlbumMediaRelationShip, parameters) == 0)
            {
                dBHelper.InsertUpdateRow(_sqlCreateAlbumMediaRelationship, parameters);
            }
        }
    }
}
