using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AppBase.ORM
{
    public class BaseEntity : IFlattingObject<BaseEntity>
    {
        /// <summary>
        /// Flatten object
        /// </summary>
        /// <returns>A collection of object</returns>
        public HashSet<BaseEntity> Flatten()
        {
            var bag = new HashSet<BaseEntity>();
            Flatten(ref bag);
            return bag;
        }

        /// <summary>
        /// Flatten object
        /// </summary>
        /// <param name="bag">A reference to a collection where all objects will be collected</param>
        public void Flatten(ref HashSet<BaseEntity> bag)
        {
            if (!bag.Add(this))
                return;
            foreach (var prop in GetType().GetProperties())
                if (typeof(IFlattingObject<BaseEntity>).IsAssignableFrom(prop.PropertyType))
                {
                    var val = prop.GetValue(this) as IFlattingObject<BaseEntity>;
                    if (val != null)
                        val.Flatten(ref bag);
                }
        }

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="conn">DB connection</param>
        /// <returns>Repository</returns>
        public virtual BaseRepository CreateRepository(SqlConnection conn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize entity from a data row
        /// </summary>
        /// <param name="row">Data row</param>
        public virtual void FromDataRow(DataRow row)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize entity from another entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void FromEntity(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clone entity
        /// </summary>
        /// <returns>A clone</returns>
        public virtual BaseEntity Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get entity key
        /// </summary>
        /// <returns>A dictionary that represents the key</returns>
        public virtual Dictionary<string, object> GetKey()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load all missing properties
        /// </summary>
        /// <param name="conn">DB connection</param>
        public void Load(SqlConnection conn)
        {
            FromEntity(CreateRepository(conn).SelectOne(GetKey()));
        }
    }
}
