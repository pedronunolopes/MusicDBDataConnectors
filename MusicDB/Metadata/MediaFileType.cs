using MusicDB.ExternalData.Connectors.Helpers;
using MusicDB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Entities.Metadata
{
    public partial class MediaFileType : BaseEntity
    {
        /// <summary>
        /// Defines Media File Type Enums
        /// </summary>
        public enum MediaFileEnum
        {
            Undefined = 0,
            Image = 1,
            Movie = 2
        }

        /// <summary>
        /// Contructor by ID
        /// </summary>
        /// <param name="mediaFileTypeId"></param>
        public MediaFileType(MediaFileEnum mediaFileTypeId) : base((int)mediaFileTypeId)
        {

        }

        /// <summary>
        /// Contructor by Name
        /// </summary>
        /// <param name="ssName"></param>
        public MediaFileType(string ssName) : base((int)MediaFileEnum.Undefined)
        {
            Name = MusicDB.ExternalData.Helpers.BasicHelper.ToTitleCase(ssName);
        }
    }
}
