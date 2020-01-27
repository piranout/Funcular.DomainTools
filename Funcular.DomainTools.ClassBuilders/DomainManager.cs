using System;
using System.Collections.Generic;
using System.Data;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;
using Funcular.DomainTools.Utilities;

namespace Funcular.DomainTools.ClassBuilders
{
    public class DomainManager
    {
        protected readonly ICollection<ClassConfiguration> _classConfigurations = new List<ClassConfiguration>();
        protected readonly IDictionary<string, ICollection<SchemaColumnInfo>> _schemaColumnDictionary = new Dictionary<string, ICollection<SchemaColumnInfo>>();

        public GeneratorOptions GeneratorOptions
        {
            get { return _generatorOptions; }
            set { _generatorOptions = value; }
        }

        protected readonly ClassGenerator _classGenerator;

        protected readonly string _connectionString;

        protected readonly SqlServerInterrogator _interrogator;
        protected readonly IList<string> _tableNames = new List<string>();
        protected readonly IList<string> _viewNames = new List<string>();
        protected readonly IList<string> _storedProcedureNames = new List<string>();
        private GeneratorOptions _generatorOptions;

        public DomainManager(GeneratorOptions generatorOptions, ClassGenerator classGenerator, SqlServerInterrogator interrogator)
        {
            _generatorOptions = generatorOptions;
            _classGenerator = classGenerator;
            _interrogator = interrogator;
            _connectionString = interrogator.ConnectionString;
        }

        public string ConnectionString
        {
            get { return _interrogator.ConnectionString; }
        }

        public IList<string> GetViewNames()
        {
            _viewNames.Clear();
            return _viewNames.AddRange(_interrogator.GetViewNames());
        }

        public IList<string> GetTableNames()
        {
            _tableNames.Clear();
            return _tableNames.AddRange(_interrogator.GetTableNames());
        }

        public IList<string> GetStoredProcedureNames()
        {
            _storedProcedureNames.Clear();
            return _storedProcedureNames.AddRange(_interrogator.GetStoredProcedureNames());
        }

        public DomainManager CreateClass(CommandType commandType, string commandText)
        {
            switch (commandType)
            {
                case CommandType.Text:
                    _classGenerator.CreateTextCommandClass(commandText, _schemaColumnDictionary[commandText]);
                    break;
                case CommandType.StoredProcedure:
                    _classGenerator.CreateProcedureClass(commandText, _schemaColumnDictionary[commandText]);
                    break;
                case CommandType.TableDirect:
                    _classGenerator.WriteTableClass(commandText, _schemaColumnDictionary[commandText]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandType));
            }
            return this;
        }

        public IList<SchemaColumnInfo> GetTableColumnInfoList(string tableName)
        {
            var list = _interrogator.GetTableColumnInfoList(tableName);
            _schemaColumnDictionary[tableName] = list;
            return list;
        }

        public IList<SchemaColumnInfo> GetTextCommandColumnInfoList(string textCommand)
        {
            var list = _interrogator.GetTextCommandColumnInfoList(textCommand);
            _schemaColumnDictionary[textCommand] = list;
            return list;
        }

        public IList<SchemaColumnInfo> GetProcedureColumnInfoList(string procedureName)
        {
            var list = _interrogator.GetProcedureColumnInfoList(procedureName);
            _schemaColumnDictionary[procedureName] = list;
            return list;
        }
    }
}