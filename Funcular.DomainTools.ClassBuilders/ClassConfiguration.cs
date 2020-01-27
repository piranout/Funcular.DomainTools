using System.Collections.Generic;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;

namespace Funcular.DomainTools.ClassBuilders
{
    public class ClassConfiguration
    {
        #region Nonpublic fields

        #endregion

        public ClassConfiguration(ICollection<SchemaColumnInfo> schemaColumns)
        {
            SchemaColumns = schemaColumns;
        }

        public string ClassName { get; set; }

        public string EntityNamespace { get; set; }

        public SchemaColumnInfo PrimaryKeyColumn { get; set; }

        public string InnerNamespace { get; set; }

        public string UnqualifiedTableName { get; set; }

        public string EntityOutputDirectory { get; set; }

        public ICollection<ForeignKey> ForeignKeys { get; set; }

        public ICollection<SchemaColumnInfo> SchemaColumns { get; }
    }
}