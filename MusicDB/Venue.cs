using MusicDB.Entities.Metadata;
using MusicDB.ExternalData.Connectors.Helpers;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Entities
{
    /// <summary>
    /// Venue entity
    /// </summary>
    public partial class Venue : BaseEntity
    {

        string _description = null;

        /// <summary>
        /// Venue description
        /// </summary>
        public string Description { get => _description; set => _description = value; }

        string _address1 = null;

        /// <summary>
        /// City of the Venue
        /// </summary>
        public string Address1 { get => _address1; set => _address1 = value; }

        string _address2 = null;

        /// <summary>
        /// City of the Venue
        /// </summary>
        public string Address2 { get => _address2; set => _address2 = value; }

        string _city = null;

        /// <summary>
        /// City of the Venue
        /// </summary>
        public string City { get => _city; set => _city = value; }

        Country _country = null;

        /// <summary>
        /// City of the Venue
        /// </summary>
        public Country Country { get => _country; set => _country = value; }

        decimal _latitude = 0;

        /// <summary>
        /// Latitude
        /// </summary>
        public decimal Latitude { get => _latitude; set => _latitude = value; }

        decimal _longitude = 0;

        /// <summary>
        /// Latitude
        /// </summary>
        public decimal Longitude { get => _longitude; set => _longitude = value; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Venue():base(0)
        {

        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Venue(int id) : base(id)
        {

        }



    }
}
