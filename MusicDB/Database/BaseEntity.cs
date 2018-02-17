using System;

namespace MusicDB.Entities
{
    public partial class BaseEntity
    {
        /// <summary>
        ///  Initializes the record from the database if needed
        /// </summary>
        protected void InitFromDatabase()
        {
            if (_id < 1 && _ssName != null)
                InitFromDatabaseByName();

            if (_id > 0 && _ssName == null)
                InitFromDatabaseById();

        }

        /// <summary>
        /// Initializes from database By Name
        /// </summary>
        protected virtual void InitFromDatabaseByName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes from database By Id
        /// </summary>
        protected virtual void InitFromDatabaseById()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Commmit to the database
        /// </summary>
        public virtual void Commit() { }

    }
}
