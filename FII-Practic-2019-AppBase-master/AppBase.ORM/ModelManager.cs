using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace AppBase.ORM
{
    public class ModelManager
    {
        /// <summary>
        /// Complete model with missing data
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="connStr">Connection string</param>
        public static void Complete(Model model, string connStr = null)
        {
            if (string.IsNullOrEmpty(connStr))
                connStr = CommonHelpers.GetConnectionString();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT sch.[name] AS [SchemaName], 
                           tbl.[name] AS [TableName], 
                           col.[name] AS [ColumnName],
                           typ.[name] AS [ColumnType],
                           col.[is_nullable] AS [IsNullable],
                           idx.[is_primary_key] AS [IsKey],
                           col.[max_length] AS [Size]
                    FROM [sys].[all_columns] AS col
                    LEFT JOIN [sys].[tables] AS tbl ON tbl.[object_id] = col.[object_id]
                    LEFT JOIN [sys].[schemas] AS sch ON sch.[schema_id] = tbl.[schema_id]
                    LEFT JOIN [sys].[types] AS typ ON typ.[user_type_id] = col.[user_type_id]
                    LEFT JOIN [sys].[index_columns] AS icl ON icl.[object_id] = col.[object_id] AND icl.[column_id] = col.[column_id]
                    LEFT JOIN [sys].[indexes] AS idx ON idx.[object_id] = col.[object_id] AND idx.[index_id] = icl.[index_id]
                    WHERE sch.[name] = 'dbo'
                    ";

                var tbl = new DataTable();
                using (var reader = cmd.ExecuteReader())
                    tbl.Load(reader);

                foreach (var entity in model.Entities)
                {
                    var rows = tbl.Select("[TableName] = '" + entity.TableName + "'");
                    foreach (var row in rows)
                    {
                        if (entity.Fields == null)
                            entity.Fields = new List<ModelField>();

                        var f = entity.Fields
                            .FirstOrDefault(x =>
                                string.Equals((string)row["ColumnName"], x.ColumnName,
                                    StringComparison.InvariantCultureIgnoreCase));
                        if (f == null)
                        {
                            f = new ModelField();
                            entity.Fields
                                .Add(f);
                        }

                        if (!string.IsNullOrEmpty(f.Relation))
                            continue;

                        #region Fill column info
                        f.ColumnName = (string)row["ColumnName"];
                        f.FieldName = f.FieldName ?? (string)row["ColumnName"];
                        f.ColumnType = (string)row["ColumnType"];
                        f.IsNullable = !row.IsNull("IsNullable") ? (bool?)row["IsNullable"] : null;
                        f.IsKey = !row.IsNull("IsKey") ? (bool?)row["IsKey"] : null;
                        f.ColumnSize = !row.IsNull("Size") ? (short?)row["Size"] : null;
                        #endregion
                    }

                    if (entity.Fields.Count !=
                        entity.Fields.Select(x => x.FieldName).Distinct().Count())
                        throw new ModelException(string.Format(
                            "\"{0}\" entity: has duplicate fields",
                            entity.Name
                            ));
                }

                #region Validate relations
                foreach (var entity in model.Entities)
                    foreach (var relationField in entity.GetRelationFields())
                    {
                        var chain =
                            model.GetRelationEntityChain(relationField.Relation, entity);

                        foreach (var field in chain.Relation.Fields)
                        {
                            #region Validate parent column
                            var parentColumnTableName = chain.Parent.TableName;
                            var parentColumnName = field.ParentColumnName;
                            var parentColumn = tbl.Select(
                                "[TableName] = '" + parentColumnTableName + "' AND [ColumnName] = '" + parentColumnName + "'");
                            if (parentColumn.Length != 1)
                                throw new ModelException(string.Format(
                                    "\"{0}\" relation: parent column \"{1}\" is missing on table \"{2}\"",
                                    relationField.Relation,
                                    parentColumnName,
                                    parentColumnTableName
                                    ));
                            #endregion

                            #region Validate child column
                            var childColumnTableName = chain.End2.TableName;
                            var childColumnName = field.ChildColumnName;
                            var childColumn = tbl.Select(
                                "[TableName] = '" + childColumnTableName + "' AND [ColumnName] = '" + childColumnName + "'");
                            if (childColumn.Length != 1)
                                throw new ModelException(string.Format(
                                    "\"{0}\" relation: child column \"{1}\" is missing on table \"{2}\"",
                                    relationField.Relation,
                                    childColumnName,
                                    childColumnTableName
                                    ));
                            #endregion
                        }

                        if (chain.RelationType == ModelRelationEntityChainType.Unknown)
                            throw new ModelException(string.Format(
                                "\"{0}\" relation: unknown relation type",
                                relationField.Relation
                                ));
                    }
                #endregion
            }
        }
    }
}
