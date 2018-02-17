using MusicDB.ExternalData.Connectors.Helpers;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Entities.Metadata
{
    public partial class MusicStyle : BaseEntity
    {

        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
            int? testId = null;

            if (_id == 0 && _ssName != null)
            {
                testId = GetMusicStyleId(Name);
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
                testName = GetMusicStyleId(Id);
                if (testName != null)
                    Name = testName;
            }
        }

        // Get MusicStyleId
        const string _strGetMusicStyleIdByName = "SELECT MusicStyleId FROM dbo.t_MDMusicStyle WHERE Name = '{0}'";

        private int? GetMusicStyleId(string strMusicStyleName)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetMusicStyleIdByName, strMusicStyleName.Replace("'", "''"));
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        // Get MusicStyleId
        const string _strGetMusicStyleIdById = "SELECT Name FROM dbo.t_MDMusicStyle WHERE MusicStyleId = '{0}'";

        private string GetMusicStyleId(int MusicStyleId)
        {
            string result = null;
            string sqlCommand = string.Format(_strGetMusicStyleIdByName, MusicStyleId.ToString());
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
        /// Updates the MusicStyles in the database
        /// </summary>
        private void CreateOrUpdate()
        {
            int? testId = null;

            // checks if the MusicStyle already exists
            if (Id == 0 && Name != null)
            {
                testId = GetMusicStyleId(Name);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateMusicStyleInDB();
            else
                UpdateMusicStyleInDB();
        }

        const string _sqlUpdateMusicStyle = "UPDATE dbo.t_MDMusicStyle Set NAME = @Name WHERE MusicStyleId = @MusicStyleId";

        /// <summary>
        /// Updates and MusicStyle in the database
        /// </summary>
        private void UpdateMusicStyleInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@MusicStyleId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateMusicStyle, parameters);

        }

        const string _sqlCreateMusicStyle = "INSERT INTO dbo.t_MDMusicStyle (NAME) VALUES (@Name)";

        /// <summary>
        /// Creates the MusicStyle in the database
        /// </summary>
        private void CreateMusicStyleInDB()
        {

            int? testId = null;
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);

            dBHelper.InsertUpdateRow(_sqlCreateMusicStyle, parameters);

            // Updates the MusicStyleId in the context class
            testId = GetMusicStyleId(Name);
            if (testId != null)
                Id = testId.Value;
        }
    }
}
