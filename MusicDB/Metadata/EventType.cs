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
        /// Defines Event Type Enums
        /// </summary>
        public enum EventTypeEnum
        {
            Undefined = 0,
            Tour = 1,
            Concert = 2
        }

        /// <summary>
        /// Contructor by ID
        /// </summary>
        /// <param name="EventTypeId"></param>
        public EventType(EventTypeEnum EventTypeId) : base((int)EventTypeId)
        {

        }

        /// <summary>
        /// Constructor by Name
        /// </summary>
        /// <param name="ssName"></param>
        public EventType(string ssName) : base((int)EventTypeEnum.Undefined)
        {
            Name = MusicDB.ExternalData.Helpers.BasicHelper.ToTitleCase(ssName);
        }
    }
}
