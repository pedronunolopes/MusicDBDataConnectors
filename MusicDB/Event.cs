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
    /// Event entity
    /// </summary>
    public partial class Event : BaseEntity
    {

        DateTime _date = new DateTime();

        /// <summary>
        /// Event Date
        /// </summary>
        public DateTime Date { get => _date; set => _date = value; }

        string _description = null;

        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get => _description; set => _description = value; }

        EventType _type = new EventType(EventType.EventTypeEnum.Undefined);

        /// <summary>
        /// Event Type
        /// </summary>
        public EventType Type { get => _type; set => _type = value; }

        string _city = null;

        /// <summary>
        /// City of the event
        /// </summary>
        public string City { get => _city; set => _city = value; }

        Venue _venue = null;

        /// <summary>
        /// Venue of the event
        /// </summary>
        public Venue Venue { get => _venue; set => _venue = value; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Event():base(0)
        {

        }



    }
}
