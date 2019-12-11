using System.Collections.Generic;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;

namespace Funcular.DomainTools.ClassBuilders
{
    public class ClassConfiguration
    {
        #region Nonpublic fields

        private ICollection<ForeignKey> _foreignKeys;

        private readonly ICollection<SchemaColumnInfo> _schemaColumns;

        private string _entityNamespace;
        private SchemaColumnInfo _primaryKeyColumn;
        private string _innerNamespace;
        private string _unqualifiedTableName;
        private string _className;

        #endregion

        public ClassConfiguration(ICollection<SchemaColumnInfo> schemaColumns)
        {
            _schemaColumns = schemaColumns;
        }

        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        public string EntityNamespace
        {
            get { return _entityNamespace; }
            set { _entityNamespace = value; }
        }

        public SchemaColumnInfo PrimaryKeyColumn
        {
            get { return _primaryKeyColumn; }
            set { _primaryKeyColumn = value; }
        }

        public string InnerNamespace
        {
            get { return _innerNamespace; }
            set { _innerNamespace = value; }
        }

        public string UnqualifiedTableName
        {
            get { return _unqualifiedTableName; }
            set { _unqualifiedTableName = value; }
        }

        public ICollection<ForeignKey> ForeignKeys
        {
            get { return _foreignKeys; }
            set { _foreignKeys = value; }
        }

        public ICollection<SchemaColumnInfo> SchemaColumns
        {
            get { return _schemaColumns; }
        }
    }
}