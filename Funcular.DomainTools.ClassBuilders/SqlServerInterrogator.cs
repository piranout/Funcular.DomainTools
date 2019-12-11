using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;
using Funcular.DomainTools.Utilities;
using Humanizer;

namespace Funcular.DomainTools.ClassBuilders
{
	public class SqlServerInterrogator
	{
		protected readonly string _connectionString;


	    public static readonly string GetProcedureNamesCmdText = @"SELECT SPECIFIC_SCHEMA + '.' + ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE (ROUTINE_TYPE = 'PROCEDURE') ORDER BY ROUTINE_NAME";
	    private ICollection<ForeignKey> _foreignKeys;

	    public SqlServerInterrogator(string connectionString)
		{
			_connectionString = connectionString;
		}

		public string ConnectionString
		{
			get { return this._connectionString; }
		}

		/// <summary>
		/// Get info about each column in a stored procedure
		/// </summary>
		/// <param name="procedureName"></param>
		/// <returns></returns>
		public IList<SchemaColumnInfo> GetProcedureColumnInfoList(string procedureName)
		{
		    var columnInfoList = getProcedureSchema(_connectionString, procedureName).ToList<SchemaColumnInfo>();
            foreach (var schemaColumnInfo in columnInfoList)
            {
                schemaColumnInfo.PropertyName = 
                    schemaColumnInfo.ColumnName.Collapse(StringHelpers.ChangeCaseTypes.PascalCase, false, "_", " ", "-")
                    .Pascalize();
            }
            return columnInfoList;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList<SchemaColumnInfo> GetTableColumnInfoList(string tableName)
		{
		    var tableColumnInfoList = GetTableSchema(tableName).ToList<SchemaColumnInfo>();
		    var unqualifiedTableName = tableName
                .RemoveAll(new[] {'[', ']'})
                .RightOfLast(".")
                .Collapse(StringHelpers.ChangeCaseTypes.PascalCase, false, "_")
                .Singularize();
		    foreach (var schemaColumnInfo in tableColumnInfoList)
		    {
		        schemaColumnInfo.PropertyName = schemaColumnInfo.ColumnName.Collapse(StringHelpers.ChangeCaseTypes.PascalCase, false, "_", " ", "-");
                // C# DOES NOT ALLOW PROPERTY NAMES TO BE THE SAME AS THEIR ENCLOSING TYPE, SO:
		        if (schemaColumnInfo.PropertyName.Equals(unqualifiedTableName, StringComparison.OrdinalIgnoreCase))
		            schemaColumnInfo.PropertyName += "Name";
		    }
		    return tableColumnInfoList;
		}

	    public IList<SchemaColumnInfo> GetTextCommandColumnInfoList(string textCommand)
		{
	        var columnInfoList = GetTextCommandSchema(textCommand).ToList<SchemaColumnInfo>();
            foreach (var schemaColumnInfo in columnInfoList)
            {
                schemaColumnInfo.PropertyName = schemaColumnInfo.ColumnName.Collapse(StringHelpers.ChangeCaseTypes.PascalCase, false, "_").Pascalize();
            }
            return columnInfoList;

		}

		/// <summary>
		/// Does the work for GetColumnInfoList
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="procedureName"></param>
		/// <returns></returns>
		protected DataTable getProcedureSchema(string connectionString, string procedureName)
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					con.ConnectionString = connectionString;
					cmd.Connection = con;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = procedureName;
					con.Open();
					SqlCommandBuilder.DeriveParameters(cmd);
					// .ExecuteReader thinks the parameters aren't present if
					// you don't initialize their values to something; skip this
					// step and you'll get an exception saying 
					// "SelectProcedureName expects parameter @ParamName, which was not supplied."
					for (int i = 0; i < cmd.Parameters.Count; i++)
					{
						SqlParameter tmpParm = cmd.Parameters[i];
						if (tmpParm.Direction == ParameterDirection.Input
							|| tmpParm.Direction == ParameterDirection.InputOutput)
							tmpParm.Value = DBNull.Value;
					}
					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
					{
						return reader.GetSchemaTable();
					}
				}
			}
		}
		public DataTable GetTextCommandSchema(string textCommand, bool traceUnderlyingSchemaColumns = true)
		{
			using (SqlConnection con = new SqlConnection(this._connectionString))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					con.ConnectionString = _connectionString;
					cmd.Connection = con;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = textCommand;
					con.Open();
					using (SqlDataReader reader = cmd.ExecuteReader(traceUnderlyingSchemaColumns ? CommandBehavior.KeyInfo : CommandBehavior.Default))
					{
						if (traceUnderlyingSchemaColumns)
							return reader.GetSchemaTable();
						else
						{
							using (DataSet ds = new DataSet())
							{
								DataTable dt = new DataTable("CustomSqlStatement");
								ds.Tables.Add(dt);
								ds.Load(reader, LoadOption.OverwriteChanges, ds.Tables[0]);
								return dt;
							}
						}
					}
				}
			}
		}
		public DataTable GetTableSchema(string tableName)
		{
		    var tableNameTokens = tableName.Split('.');
		    var patchedTokens = new List<string>();
		    foreach (var tableNameToken in tableNameTokens)
		    {
		        patchedTokens.Add(tableNameToken.EnsureStartsWith("[").EnsureEndsWith("]"));
		    }
		    tableName = string.Join(".", patchedTokens);
			using (SqlConnection con = new SqlConnection(this._connectionString))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					con.ConnectionString = _connectionString;
					cmd.Connection = con;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "SELECT TOP 1 * FROM " + tableName;
					con.Open();
				    DataTable schemaTable;
				    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo))
					{
					    schemaTable = reader.GetSchemaTable();
					}
                    return schemaTable;

				}
			}
		}
		public IEnumerable<string> GetViewNames(bool includeSchema = true)
		{
			const string commandText = @"
				SELECT 
				TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS v
                WHERE ISNULL(ObjectProperty (object_id(v.TABLE_NAME), 'IsMSShipped'),0) = 0 
                ORDER BY TABLE_SCHEMA, TABLE_NAME";
			var ret = new List<string>();
			try
			{
				using (var con = new SqlConnection(this._connectionString))
				{
					using (var cmd = new SqlCommand())
					{
						cmd.Connection = con;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = commandText;
						con.Open();
						using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
						{
							while (reader.Read())
							{
							    var sTable = includeSchema 
                                    ? $"{reader.GetString(0)}.{reader.GetString(1)}" 
                                    : reader.GetString(1);
							    ret.Add(sTable);
							}
						    reader.Close();
							con.Close();
						}
					}
				}
			    this._foreignKeys = this._foreignKeys ?? GetAllForeignKeys();
			}
			catch(Exception ex)
			{
			    Debug.WriteLine($"Exception: {ex.ToString()}");
                return Enumerable.Empty<string>();
            }
            return ret;
		}

		public IEnumerable<string> GetTableNames(bool includeSchema = true)
		{
		    var tableName = includeSchema ? @"TABLE_SCHEMA + '.' + TABLE_NAME " : "TABLE_NAME ";

            var formattableString = $@"SELECT {tableName} FROM INFORMATION_SCHEMA.TABLES 
                WHERE (TABLE_TYPE = 'BASE TABLE') 
                AND ISNULL(ObjectProperty (object_id(TABLE_NAME), 'IsMSShipped'),0) = 0
                AND TABLE_NAME != '__RefactorLog'
                ORDER BY TABLE_SCHEMA, TABLE_NAME";
		    var sCmd =
			    formattableString;
			try
			{
				using (var con = new SqlConnection(this._connectionString))
				{
					using (var cmd = new SqlCommand())
					{
						cmd.Connection = con;
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = sCmd;
						var ret = new List<string>();
						con.Open();
						using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
						{
							while (reader.Read())
							{
							    ret.Add(reader.GetString(0));
							}
							reader.Close();
							con.Close();
			                this._foreignKeys = this._foreignKeys ?? GetAllForeignKeys();
                            return ret;
						}
					}
                }
            }
			catch
			{
				return Enumerable.Empty<string>();
            }
        }

	    public IEnumerable<string> GetStoredProcedureNames()
	    {
	        try
	        {
	            using (var con = new SqlConnection(this._connectionString))
	            {
	                using (var cmd = new SqlCommand())
	                {
	                    cmd.Connection = con;
	                    cmd.CommandType = CommandType.Text;
	                    cmd.CommandText = GetProcedureNamesCmdText;
	                    var ret = new List<string>();
	                    con.Open();
	                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
	                    {
	                        while (reader.Read())
	                        {
	                            var item = reader.GetString(0);
	                            if (!item.StartsWith("dt_", StringComparison.CurrentCultureIgnoreCase))
	                                ret.Add(item);
	                        }
	                        reader.Close();
	                        con.Close();
	                        return ret;
	                    }
	                }
	            }
	        }
	        catch (Exception ex)
	        {
	            System.Diagnostics.Trace.WriteLine(ex.ToString());
	            return Enumerable.Empty<string>();
	        }
	    }

	    public ICollection<ForeignKey> GetAllForeignKeys()
	    {
	        var commandText =
	            @"
                    select K.name as RelationshipName, S1.name as FromSchema, T1.name as FromTable, C1.name as FromColumn, S2.name as ToSchema, T2.name as ToTable, C2.name as ToColumn
                    from sys.foreign_keys as K
                    join sys.foreign_key_columns as C on K.object_id = C.constraint_object_id
                    join sys.columns as C1 on K.parent_object_id = C1.object_id 
                    join sys.tables as T1 on K.parent_object_id = T1.object_id 
                    join sys.columns as C2 on K.referenced_object_id = C2.object_id
                    join sys.tables as T2 on K.referenced_object_id = T2.object_id 
                    join sys.schemas S1 on S1.schema_id = t1.schema_id
                    join sys.schemas S2 on S2.schema_id = t2.schema_id
                    where C1.column_id = C.parent_column_id
                    and C2.column_id = C.referenced_column_id
                    and T1.Name != '__RefactorLog'
                    order by S1.Name, S2.Name";
	        ICollection<ForeignKey> collection = Enumerable.Empty<ForeignKey>().ToArray();
            try
            {
                var dataReflector = new DataReflector(_connectionString);
                collection = dataReflector.ExecuteCollection<ForeignKey>(commandText);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
	        return collection;
	    }
	}
}
