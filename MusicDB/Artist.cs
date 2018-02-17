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
    public partial class  Artist : BaseEntity
    {   
       
        // Description
        string _ssDescription = null;

        public string Description { get => _ssDescription; set => _ssDescription = value; }

        // History
        string _ssHistory = null;

        public string History { get => _ssHistory; set => _ssHistory = value; }

        // Related Artists  
        Dictionary<string, Artist> _relatedArtists = new Dictionary<string, Artist>();

        public Dictionary<string, Artist> RelatedArtists { get => _relatedArtists; }

        // Music Styles
        Dictionary<string, MusicStyle> _styles = new Dictionary<string, MusicStyle>();

        public Dictionary<string, MusicStyle> MusicStyles { get => _styles; }

        // Artist Logo
        Media _logo = null;

        public Media Logo
        {
            get => _logo;
            set
            {
                _logo = value;

                // forces the media type to 
                if (_logo != null)
                {
                    _logo.MediaType = new MediaType(MediaType.MediaEnum.Logo);
                    _logo.FileType = new MediaFileType(MediaFileType.MediaFileEnum.Image);
                }
            }
        }

        // Related Artists  
        Dictionary<string, Album> _albums = new Dictionary<string, Album>();

        public Dictionary<string, Album> Albums { get => _albums; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Artist():base(0)
        {

        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Artist(string name) : base(0)
        {
            _ssName = name;
        }
    }
}
