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
        /// Defines Media File Type Enums
        /// </summary>
        public enum MediaEnum
        {
            Undefined = 0,
            Logo = 1,
            AlbumCover = 2
        }

        /// <summary>
        /// Contructor by ID
        /// </summary>
        /// <param name="mediaFileTypeId"></param>
        public MediaType(MediaEnum mediaFileTypeId) : base((int)mediaFileTypeId)
        {

        }

        /// <summary>
        /// Constructor by Name
        /// </summary>
        /// <param name="ssName"></param>
        public MediaType(string ssName) : base((int)MediaEnum.Undefined)
        {
            Name = MusicDB.ExternalData.Helpers.BasicHelper.ToTitleCase(ssName);
        }
    }
}
