using System;

namespace Funcular.DomainTools.ClassBuilders.SqlMetaData
{
    public partial class SchemaColumnInfo
    {
        public string ColumnName { get; set; }
        public string PropertyName { get; set; }
        public string DataTypeName { get; set; }
        public bool IsKey { get; set; }
        public bool IsUnique { get; set; }
        public int ColumnSize { get; set; }
        public int NumericPrecision { get; set; }
        public int NumericScale { get; set; }
        public int ColumnOrdinal { get; set; }
        public bool AllowDBNull { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsAutoIncrement { get; set; }
        public bool IsAliased { get; set; }
        public string BaseServerName { get; set; }
        public string BaseCatalogName { get; set; }
        public string BaseColumnName { get; set; }
        public string BaseSchemaName { get; set; }
        public string BaseTableName { get; set; }
        public Type DataType { get; set; }
        public bool IsExpression { get; set; }
        public bool IsRowVersion { get; set; }
        public bool IsHidden { get; set; }
        public bool IsLong { get; set; }
        public bool IsReadOnly { get; set; }
        public Type ProviderSpecificDataType { get; set; }
        public bool IsColumnSet { get; set; }
    }
}