using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppBase.ORM
{
    public class ModelRelation
    {
        /// <summary>
        /// Get or set name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Get or set entity name
        /// </summary>
        [JsonProperty("entityName")]
        public string EntityName { get; set; }

        /// <summary>
        /// Get or set relation fields
        /// </summary>
        [JsonProperty("fields")]
        public IList<ModelRelationField> Fields { get; set; }
    }
}
