using MusicDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.ExternalData.Connectors
{
    interface IDataConnector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistName"></param>
        Artist GetArtistByName(string strArtistName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistName"></param>
        List<Event> GetEventsByArtist(Artist artist);

    }
}
