using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicDB.Entities.Metadata;

namespace MusicDB.Entities
{
    /// <summary>
    /// Artist 
    /// </summary>
    public partial class Album : BaseEntity
    {   
       
        // Description
        string _ssDescription = null;

        public string Description { get => _ssDescription; set => _ssDescription = value; }

        // Related Artists  
        Dictionary<string, Song> _songs = new Dictionary<string, Song>();

        public Dictionary<string, Song> Songs { get => _songs; }

        // Artist Logo
        Media _logo = null;

        public Media Logo {
            get => _logo;
            set
            {
                _logo = value;

                // forces the media type to 
                if (_logo != null)
                {
                    _logo.MediaType = new MediaType(MediaType.MediaEnum.AlbumCover);
                    _logo.FileType = new MediaFileType(MediaFileType.MediaFileEnum.Image);
                }
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Album() : base(0)
        {

        }
    }
}
