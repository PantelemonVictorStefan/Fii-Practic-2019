using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBase.ORM
{
    public class ModelRelationEntityChain
    {
        /// <summary>
        /// Get or set entity relation
        /// </summary>
        public ModelRelation Relation { get; set; }

        /// <summary>
        /// Get or set entity relation type
        /// </summary>
        public ModelRelationEntityChainType RelationType { get; set; } =
            ModelRelationEntityChainType.Unknown;

        /// <summary>
        /// Get or set entity relation END-1
        /// </summary>
        public ModelEntity End1 { get; set; }

        /// <summary>
        /// Get or set entity relation PARENT
        /// </summary>
        public ModelEntity Parent { get; set; }

        /// <summary>
        /// Get or set entity relation END-2
        /// </summary>
        public ModelEntity End2 { get; set; }

        /// <summary>
        /// Get relation fields
        /// </summary>
        /// <returns>A collection of fields</returns>
        public Dictionary<ModelField, ModelField> GetRelationFields()
        {
            var relationFields =
                new Dictionary<ModelField, ModelField>();

            foreach (var field in Relation.Fields)
                relationFields.Add(
                    End1.Fields.FirstOrDefault(
                        x => x.ColumnName != null &&
                        x.ColumnName.Equals(field.ChildColumnName, StringComparison.InvariantCultureIgnoreCase)),
                    End2.Fields.FirstOrDefault(
                        x => x.ColumnName != null &&
                        x.ColumnName.Equals(field.ParentColumnName, StringComparison.InvariantCultureIgnoreCase))
                        );

            return relationFields;
        }
    }
}
