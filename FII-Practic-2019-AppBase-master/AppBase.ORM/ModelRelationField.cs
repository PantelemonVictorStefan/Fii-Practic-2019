using Newtonsoft.Json;

namespace AppBase.ORM
{
    public class ModelRelationField
    {
        /// <summary>
        /// Get or set parent column name
        /// </summary>
        [JsonProperty("parentColumnName")]
        public string ParentColumnName { get; set; }

        /// <summary>
        /// Get or set child column name
        /// </summary>
        [JsonProperty("childColumnName")]
        public string ChildColumnName { get; set; }
    }
}
