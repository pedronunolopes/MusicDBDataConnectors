using MusicDB.ExternalData.Connectors.Helpers;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Entities.Metadata
{
    public partial class EventType : BaseEntity
    {

        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
            int? testId = null;

            if (_id == 0 && _ssName != null)
            {
                testId = GetEventTypeId(Name);
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
                testName = GetEventTypeId(Id);
                if (testName != null)
                    Name = testName;
            }
        }

        // Get EventTypeId
        const string _strGetEventTypeIdByName = "SELECT EventTypeId FROM dbo.t_MDEvent WHERE Name = '{0}'";

        private int? GetEventTypeId(string strEventTypeName)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetEventTypeIdByName, strEventTypeName.Replace("'", "''"));
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        // Get EventTypeId
        const string _strGetEventTypeIdById = "SELECT Name FROM dbo.t_MDEvent WHERE EventTypeId = '{0}'";

        private string GetEventTypeId(int EventTypeId)
        {
            string result = null;
            string sqlCommand = string.Format(_strGetEventTypeIdByName, EventTypeId.ToString());
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
        /// Updates the EventTypes in the database
        /// </summary>
        private void CreateOrUpdate()
        {
            int? testId = null;

            // checks if the EventType already exists
            if (Id == 0 && Name != null)
            {
                testId = GetEventTypeId(Name);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateEventTypeInDB();
            else
                UpdateEventTypeInDB();
        }

        const string _sqlUpdateEventType = "UPDATE dbo.t_MDEvent Set NAME = @Name WHERE EventTypeId = @EventTypeId";

        /// <summary>
        /// Updates and EventType in the database
        /// </summary>
        private void UpdateEventTypeInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@EventTypeId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateEventType, parameters);

        }

        const string _sqlCreateEventType = "INSERT INTO dbo.t_MDEvent (NAME) VALUES (@Name)";

        /// <summary>
        /// Creates the EventType in the database
        /// </summary>
        private void CreateEventTypeInDB()
        {

            int? testId = null;
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);

            dBHelper.InsertUpdateRow(_sqlCreateEventType, parameters);

            // Updates the EventTypeId in the context class
            testId = GetEventTypeId(Name);
            if (testId != null)
                Id = testId.Value;
        }
    }
}
