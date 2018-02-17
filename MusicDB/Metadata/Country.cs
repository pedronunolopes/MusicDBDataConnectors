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
        /// Constructor by name
        /// </summary>
        /// <param name="ssName"></param>
        public Country(string ssName) : base(0)
        {
            Name = MusicDB.ExternalData.Helpers.BasicHelper.ToTitleCase(ssName);
        }

        /// <summary>
        /// Constructor by name
        /// </summary>
        /// <param name="id"></param>
        public Country(int id) : base(id)
        {
        }
    }

}
