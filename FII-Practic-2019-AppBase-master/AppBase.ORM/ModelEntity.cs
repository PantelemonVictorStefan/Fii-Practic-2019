using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AppBase.ORM
{
    public class ModelEntity
    {
        /// <summary>
        /// Get or set table name
        /// </summary>
        [JsonProperty("tableName")]
        public string TableName { get; set; }

        /// <summary>
        /// Get or set name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Get or set fields
        /// </summary>
        [JsonProperty("fields")]
        public IList<ModelField> Fields { get; set; }

        /// <summary>
        /// Get or set relations
        /// </summary>
        [JsonProperty("relations")]
        public IList<ModelRelation> Relations { get; set; }

        /// <summary>
        /// Get fields that represents the entity primary key
        /// </summary>
        /// <returns>A collection of fields</returns>
        public IList<ModelField> GetKeyFields()
        {
            return Fields.Where(x => x.IsKey == true && string.IsNullOrEmpty(x.Relation)).ToList();
        }

        /// <summary>
        /// Get fields
        /// </summary>
        /// <returns>A collection of fields</returns>
        public IList<ModelField> GetFields()
        {
            return Fields.Where(x => string.IsNullOrEmpty(x.Relation)).ToList();
        }

        /// <summary>
        /// Get fields meant for navigation
        /// </summary>
        /// <returns></returns>
        public IList<ModelField> GetRelationFields()
        {
            return Fields.Where(x => !string.IsNullOrEmpty(x.Relation)).ToList();
        }
    }
}
