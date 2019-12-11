using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Funcular.DomainTools.Utilities.FastReflection;

namespace Funcular.DomainTools.ClassBuilders
{
    public class DataReflector
    {
        private static readonly StringComparer _ordinalIgnoreCaseComparer = StringComparer.OrdinalIgnoreCase;
        public const StringComparison OrdinalIgnoreCaseComparison = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// A case-insensitive string dictionary mapping the type's property names to SQL column ordinals.
        /// </summary>
        private static readonly Dictionary<IntPtr, PropertyInfo[]> _propertyInfoCache = new Dictionary<IntPtr, PropertyInfo[]>();

        private static readonly Dictionary<IntPtr, Dictionary<string, int>> _columnOrdinalCache = new Dictionary<IntPtr, Dictionary<string, int>>();
        private static readonly Dictionary<IntPtr, Dictionary<string, string>> _columnNameCache = new Dictionary<IntPtr, Dictionary<string, string>>();
        private static readonly CachedReflector _cachedReflector = new CachedReflector();
        private static readonly Dictionary<IntPtr, string> _selectCommands = new Dictionary<IntPtr, string>();
        private readonly string _connectionString;

        public DataReflector(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ICollection<T> ExecuteCollection<T>(string commandText = null)
        {
            var t = typeof(T);
            var typePointer = t.TypeHandle.Value;
            // TODO: Infer command text from type when not supplied
            var cmdText = PrepareType<T>(typePointer, commandText);
            List<T> returnList;
            using (var conn = new SqlConnection(this._connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(cmdText, conn))
                {
                    PrepareColumns<T>(typePointer, cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return Enumerable.Empty<T>().ToArray();
                        returnList = new List<T>();
                        while (reader.Read())
                        {
                            var instance = _cachedReflector.Instantiate<T>();
                            foreach (var property in _propertyInfoCache[typePointer])
                            {
                                if (_columnOrdinalCache[typePointer].ContainsKey(property.Name))
                                    _cachedReflector.SetProperty<T, object>(property, instance,
                                        reader.GetValue(_columnOrdinalCache[typePointer][property.Name]));
                            }
                            returnList.Add(instance);
                        }
                    }
                }
            }
            return returnList;
        }

        private static string PrepareType<T>(IntPtr typePointer, string selectCommand)
        {
            var t = typeof(T);
            // initialize a dictionary of column ordinals for the type:
            if (!_columnOrdinalCache.ContainsKey(typePointer))
            {
                _columnOrdinalCache.Add(typePointer, new Dictionary<string, int>(_ordinalIgnoreCaseComparer));
            }
            // initialize a dictionary of column names for the type:
            if (!_columnNameCache.ContainsKey(typePointer))
            {
                _columnNameCache.Add(typePointer, new Dictionary<string, string>(_ordinalIgnoreCaseComparer));
            }
            // initialize a dictionary of property infos:
            if (!_propertyInfoCache.ContainsKey(typePointer))
            {
                _propertyInfoCache.Add(typePointer, t.GetProperties(BindingFlags.Instance | BindingFlags.Public));
            }
            /* TODO: Build type:table/view/select command inference and lookup; */
            /* TODO: Make selectCommand parameter optional; */
            if (!_selectCommands.ContainsKey(typePointer))
            {
                _selectCommands.Add(typePointer, selectCommand);
            }
            var cmdText = _selectCommands[typePointer];
            return cmdText;
        }

        /// <summary>
        /// Maps column names to property names of <typeparamref name="T"/>.
        /// Modified command text of <paramref name="cmd"/>; if ‘SELECT *’ 
        /// is used, replaces asterisk with comma-separated list of column 
        /// names (with newlines).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typePointer"></param>
        /// <param name="cmd"></param>
        private static void PrepareColumns<T>(IntPtr typePointer, SqlCommand cmd)
        {
            if (!_columnOrdinalCache.ContainsKey(typePointer))
            {
                _columnOrdinalCache[typePointer] = new Dictionary<string, int>(_ordinalIgnoreCaseComparer);
            }
            if (!_columnOrdinalCache[typePointer].Any())
            {
                using (var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    var schemaTable = reader.GetSchemaTable();
                    if (schemaTable == null)
                        return;
                    var dataColumns = schemaTable.Rows
                        .Cast<DataRow>()
                        .Select(r =>
                            new Tuple<string, int>(r.Field<String>("ColumnName"), r.Field<int>("ColumnOrdinal")))
                        //new  { ColumnName = r.Field<String>("ColumnName"), ColumnOrdinal = r.Field<int>("ColumnOrdinal")  })
                        .ToList();
                    foreach (var property in _propertyInfoCache[typePointer])
                    {
                        int columnOrdinal;
                        if (!_columnOrdinalCache[typePointer].TryGetValue(property.Name, out columnOrdinal))
                        {
                            Tuple<string, int> col;
                            if (null != (col = dataColumns
                                .FirstOrDefault(
                                    x => x.Item1.Equals(property.Name, OrdinalIgnoreCaseComparison))))
                            {
                                _columnOrdinalCache[typePointer].Add(property.Name, col.Item2);
                                _columnNameCache[typePointer].Add(property.Name, col.Item1);
                            }
                        }
                    }
                }
                if (cmd.CommandText.Contains("*") && _columnNameCache.ContainsKey(typePointer))
                {
                    var columnNames = _columnNameCache[typePointer].Values
                        .Aggregate((s1, s2) => s1 + "\r\n\t," + s2);
                    cmd.CommandText = cmd.CommandText.Replace("*", columnNames);
                }

            }
        }
    }
}