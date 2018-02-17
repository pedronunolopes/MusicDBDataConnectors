using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicDB.Entities
{
   public partial class BaseEntity
    {   
        // Id
        protected int _id = 0;

        public int Id {

            get
            {
                if (_id < 1)
                    InitFromDatabase();

                return _id;
            }

            set => _id = value;
        }

        // Name
        protected string _ssName = null;

        public string Name {
            get
            {
                if (_ssName == null)
                    InitFromDatabase();

                return _ssName;
            }
            set => _ssName = value; }
        

        // InsertDate
        DateTime _insertDate = new DateTime();

        public DateTime InsertDate { get => _insertDate; set => _insertDate = value; }


        // InsertDate
        DateTime _updateDate = new DateTime();

        public DateTime UpdateDate { get => _updateDate; set => _updateDate = value; }

        public BaseEntity(int id)
        {
            _id = id;
        }



    }

    public static class MusicDBExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        public static void Commit(this List<BaseEntity> list)
        {   
            foreach (BaseEntity item in list)
            {
                item.Commit();
            }
        }
    }
}
