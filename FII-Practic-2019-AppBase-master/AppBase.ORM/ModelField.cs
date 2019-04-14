using Newtonsoft.Json;
using System;

namespace AppBase.ORM
{
    public class ModelField
    {
        /// <summary>
        /// Get or set the field name
        /// </summary>
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        /// <summary>
        /// Get or set relation (composed out of entity name + '.' + relation name)
        /// </summary>
        [JsonProperty("relation")]
        public string Relation { get; set; }

        /// <summary>
        /// Get or set the flag specifying whether to delete or not external entities when parent is deleted
        /// </summary>
        [JsonProperty("cascadeDelete")]
        public bool? CascadeDelete { get; set; }

        /// <summary>
        /// Get or set the column name
        /// </summary>
        [JsonProperty("columnName")]
        public string ColumnName { get; set; }

        /// <summary>
        /// Get or set the column type
        /// </summary>
        [JsonProperty("columnType")]
        public string ColumnType { get; set; }

        /// <summary>
        /// Get or set the flag specifying whether column is or not nullablek
        /// </summary>
        [JsonProperty("isNullable")]
        public bool? IsNullable { get; set; }

        /// <summary>
        /// Get or set the flag specifying whether column is or not contained by the primary key
        /// </summary>
        [JsonProperty("isKey")]
        public bool? IsKey { get; set; }

        /// <summary>
        /// Get or set the column size
        /// </summary>
        [JsonProperty("columnSize")]
        public int? ColumnSize { get; set; }

        /// <summary>
        /// Get field type
        /// </summary>
        /// <returns>Type</returns>
        public string GetFieldType()
        {
            if (string.IsNullOrEmpty(ColumnType))
                throw new Exception(
                    "GetFieldType() should only be invoked " +
                    "on fields that have a ColumnType"
                    );

            var colType =
                ColumnType.ToLower();
            Type type = null;

            // SQL Server Data Types and Their .NET Framework Equivalents
            // https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2005/ms131092(v=sql.90)
            #region Get CLR data type (.NET Framework)
            switch (colType)
            {
                case "varbinary":
                case "binary":
                case "image":
                case "rowversion":
                    type = typeof(Byte[]);
                    break;
                case "varchar":
                case "char":
                case "nvarchar":
                case "nchar":
                case "text":
                case "ntext":
                    type = typeof(String);
                    break;
                case "uniqueidentifier":
                    type = typeof(Guid);
                    break;
                case "bit":
                    type = typeof(Boolean);
                    break;
                case "tinyint":
                    type = typeof(Byte);
                    break;
                case "smallint":
                    type = typeof(Int16);
                    break;
                case "int":
                    type = typeof(Int32);
                    break;
                case "bigint":
                    type = typeof(Int64);
                    break;
                case "smallmoney":
                case "money":
                case "numeric":
                case "decimal":
                    type = typeof(Decimal);
                    break;
                case "real":
                    type = typeof(Single);
                    break;
                case "float":
                    type = typeof(Double);
                    break;
                case "smalldatetime":
                case "datetime":
                    type = typeof(DateTime);
                    break;
            }
            #endregion

            if (type != null)
                return
                    IsNullable == true &&
                    type.IsValueType
                        ? "System.Nullable<" + type.FullName + ">"
                        : type.FullName;

            throw new ModelException("Unknown ColumnType \"" + colType + "\"");
        }
    }
}
