using MusicDB.ExternalData.Connectors.Helpers;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Entities.Metadata
{
    public partial class MediaType : BaseEntity
    {

        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
            int? testId = null;

            if (_id == 0 && _ssName != null)
            {
                testId = GetMediaTypeId(Name);
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
                testName = GetMediaTypeId(Id);
                if (testName != null)
                    Name = testName;
            }
        }

        // Get MediaTypeId
        const string _strGetMediaTypeIdByName = "SELECT MediaTypeId FROM dbo.t_MDMedia WHERE Name = '{0}'";

        private int? GetMediaTypeId(string strMediaTypeName)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetMediaTypeIdByName, strMediaTypeName.Replace("'", "''"));
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        // Get MediaTypeId
        const string _strGetMediaTypeIdById = "SELECT Name FROM dbo.t_MDMedia WHERE MediaTypeId = '{0}'";

        private string GetMediaTypeId(int MediaTypeId)
        {
            string result = null;
            string sqlCommand = string.Format(_strGetMediaTypeIdByName, MediaTypeId.ToString());
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
        /// Updates the MediaTypes in the database
        /// </summary>
        private void CreateOrUpdate()
        {
            int? testId = null;

            // checks if the MediaType already exists
            if (Id == 0 && Name != null)
            {
                testId = GetMediaTypeId(Name);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateMediaTypeInDB();
            else
                UpdateMediaTypeInDB();
        }

        const string _sqlUpdateMediaType = "UPDATE dbo.t_MDMedia Set NAME = @Name WHERE MediaTypeId = @MediaTypeId";

        /// <summary>
        /// Updates and MediaType in the database
        /// </summary>
        private void UpdateMediaTypeInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@MediaTypeId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateMediaType, parameters);

        }

        const string _sqlCreateMediaType = "INSERT INTO dbo.t_MDMedia (NAME) VALUES (@Name)";

        /// <summary>
        /// Creates the MediaType in the database
        /// </summary>
        private void CreateMediaTypeInDB()
        {

            int? testId = null;
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);

            dBHelper.InsertUpdateRow(_sqlCreateMediaType, parameters);

            // Updates the MediaTypeId in the context class
            testId = GetMediaTypeId(Name);
            if (testId != null)
                Id = testId.Value;
        }
    }
}
