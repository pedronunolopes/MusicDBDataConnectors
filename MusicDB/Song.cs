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
    public partial class Song : BaseEntity
    {
        TimeSpan _duration = new TimeSpan();

        public TimeSpan Length { get => _duration; set => _duration = value; }

        /// <summary>
        /// Default Contructor
        /// </summary>
        public Song() : base(0)
        {
        }
    }
}
