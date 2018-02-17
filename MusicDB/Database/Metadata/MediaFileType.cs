using MusicDB.Helpers;
using System.Collections.Generic;

namespace MusicDB.Entities.Metadata
{
    public partial class MediaFileType : BaseEntity
    {

        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
            int? testId = null;

            if (_id == 0 && _ssName != null)
            {
                testId = GetMediaFileTypeId(Name);
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
                testName = GetMediaFileTypeId(Id);
                if (testName != null)
                    Name = testName;
            }
        }

        // Get MediaFileTypeId
        const string _strGetMediaFileTypeIdByName = "SELECT MediaFileTypeId FROM dbo.t_MDMediaFile WHERE Name = '{0}'";

        private int? GetMediaFileTypeId(string strMediaFileTypeName)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetMediaFileTypeIdByName, strMediaFileTypeName.Replace("'", "''"));
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        // Get MediaFileTypeId
        const string _strGetMediaFileTypeIdById = "SELECT Name FROM dbo.t_MDMediaFile WHERE MediaFileTypeId = '{0}'";

        private string GetMediaFileTypeId(int mediaFileTypeId)
        {
            string result = null;
            string sqlCommand = string.Format(_strGetMediaFileTypeIdByName, mediaFileTypeId.ToString());
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
        /// Updates the MediaFileTypes in the database
        /// </summary>
        private void CreateOrUpdate()
        {
            int? testId = null;

            // checks if the MediaFileType already exists
            if (Id == 0 && Name != null)
            {
                testId = GetMediaFileTypeId(Name);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateMediaFileTypeInDB();
            else
                UpdateMediaFileTypeInDB();
        }

        const string _sqlUpdateMediaFileType = "UPDATE dbo.t_MDMediaFile Set NAME = @Name WHERE MediaFileTypeId = @MediaFileTypeId";

        /// <summary>
        /// Updates and MediaFileType in the database
        /// </summary>
        private void UpdateMediaFileTypeInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@MediaFileTypeId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateMediaFileType, parameters);

        }

        const string _sqlCreateMediaFileType = "INSERT INTO dbo.t_MDMediaFile (NAME) VALUES (@Name)";

        /// <summary>
        /// Creates the MediaFileType in the database
        /// </summary>
        private void CreateMediaFileTypeInDB()
        {

            int? testId = null;
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);

            dBHelper.InsertUpdateRow(_sqlCreateMediaFileType, parameters);

            // Updates the MediaFileTypeId in the context class
            testId = GetMediaFileTypeId(Name);
            if (testId != null)
                Id = testId.Value;
        }
    }
}
