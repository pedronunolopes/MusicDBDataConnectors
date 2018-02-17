using MusicDB.ExternalData.Connectors.Helpers;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Entities.Metadata
{
    public partial class Country : BaseEntity
    {

        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected override void InitFromDatabaseByName()
        {
            int? testId = null;

            if (_id == 0 && _ssName != null)
            {
                testId = GetCountryId(Name);
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
                testName = GetCountryId(Id);
                if (testName != null)
                    Name = testName;
            }
        }

        // Get CountryId
        const string _strGetCountryIdByName = "SELECT CountryId FROM dbo.t_MDCountry WHERE PrintableName = '{0}'";

        private int? GetCountryId(string strCountryName)
        {
            int? result = null;
            string sqlCommand = string.Format(_strGetCountryIdByName, strCountryName.Replace("'", "''"));
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarIntValue(sqlCommand);

            return result;
        }

        // Get CountryId
        const string _strGetCountryIdById = "SELECT PrintableName FROM dbo.t_MDCountry WHERE CountryId = '{0}'";

        private string GetCountryId(int CountryId)
        {
            string result = null;
            string sqlCommand = string.Format(_strGetCountryIdById, CountryId.ToString());
            DBHelper dBHelper = new DBHelper();

            result = dBHelper.GetScalarStringValue(sqlCommand);

            return result;
        }


        /// <summary>
        /// Commit entikty to database
        /// </summary>
        public override void Commit()
        {
            throw new InvalidOperationException("No country creation allowed on runtime");

            // commits data into the database
            //CreateOrUpdate();

        }


        /// <summary>
        /// Updates the Countrys in the database
        /// </summary>
        private void CreateOrUpdate()
        {
            int? testId = null;

            // checks if the Country already exists
            if (Id == 0 && Name != null)
            {
                testId = GetCountryId(Name);
                if (testId != null)
                    Id = testId.Value;
            }

            if (Id == 0)
                CreateCountryInDB();
            else
                UpdateCountryInDB();
        }

        const string _sqlUpdateCountry = "UPDATE dbo.t_MDCountry Set NAME = @Name WHERE CountryId = @CountryId";

        /// <summary>
        /// Updates and Country in the database
        /// </summary>
        private void UpdateCountryInDB()
        {
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);
            parameters.Add("@CountryId", Id.ToString());

            dBHelper.InsertUpdateRow(_sqlUpdateCountry, parameters);

        }

        const string _sqlCreateCountry = "INSERT INTO dbo.t_MDCountry (NAME) VALUES (@Name)";

        /// <summary>
        /// Creates the Country in the database
        /// </summary>
        private void CreateCountryInDB()
        {

            int? testId = null;
            DBHelper dBHelper = new DBHelper();
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@Name", Name);

            dBHelper.InsertUpdateRow(_sqlCreateCountry, parameters);

            // Updates the CountryId in the context class
            testId = GetCountryId(Name);
            if (testId != null)
                Id = testId.Value;
        }
    }
}
