using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;

namespace Funcular.DomainTools.ClassBuilders
{
	/// <summary>
	/// Work in progress; will facilitate bulk delupserts via a datatable, but this
	/// requires handling of explicit vs. implicit id assignment, and either persistence
	/// tracking properties or separate procedures for add/update/delete, which defeats
	/// the point. 
	/// </summary>
	public class SaveProcedureBuilder
	{

		#region StringBuilder Helper Methods
		protected int _indentLevel = 0;
		readonly StringBuilder _sb = new StringBuilder();
		public SaveProcedureBuilder Clear()
		{
			_sb.Clear();
			_indentLevel = 0;
			return this;
		}


		protected string _inputTableVarTypeName;
		protected string _inputTableVarName;
		/// <summary>
		/// IMPORTANT: Run *after* WriteProcedureDeclaration
		/// </summary>
		/// <param name="columns"></param>
		/// <returns></returns>
		public SaveProcedureBuilder WriteMergeStatement(List<SchemaColumnInfo> columns)
		{
			// todo: figure out dependency order:
			Dictionary<string, List<SchemaColumnInfo>> schemasWithColumns = new Dictionary<string, List<SchemaColumnInfo>>();
			List<string> columnNames = columns
				.Select(c => c.BaseCatalogName + "." + c.BaseSchemaName + "." + c.BaseTableName)
				.Distinct()
				.ToList();
			foreach (var item in columnNames)
			{
				schemasWithColumns.Add(
					item,
					columns
						.Where(c => c.BaseCatalogName + "." + c.BaseSchemaName + "." + c.BaseTableName == item)
						.ToList());
			}
			foreach (var kvp in schemasWithColumns)
			{
				Write("MERGE INTO {0}", kvp.Key)
					.Write("(");
			}

			return this;
		}
		public SaveProcedureBuilder WriteUpsertStatementColumn(SchemaColumnInfo column, string prefix = "", string suffix = "")
		{
			string columnVariable = string.Format(
				"{0}{1}{2}",
				prefix,
				column.ColumnName,
				suffix);
			WriteLine(columnVariable);
			return this;
		}

		/// <summary>
		/// IMPORTANT: Run *after* WriteInputTableDeclaration
		/// </summary>
		/// <returns></returns>
		public SaveProcedureBuilder WriteProcedureDeclaration(string innerNamespace, string className)
		{
			this.WriteLine("CREATE PROCEDURE {0}[Save{1}] ",
					string.IsNullOrWhiteSpace(innerNamespace) ? "" : string.Format("[{0}].", innerNamespace),
					className)
				.WriteLine("(")
				.WriteLine("@{0} AS {1} READONLY", _inputTableVarName, _inputTableVarTypeName)
				.WriteLine(")")
				.WriteLine("AS");
			return this;
		}
		/// <summary>
		/// IMPORTANT: Run *after* WriteInputTableCreateTypeStatement
		/// </summary>
		/// <returns></returns>
		public SaveProcedureBuilder WriteInputTableDeclaration()
		{
			WriteLine("DECLARE {0} AS @{1};", _inputTableVarTypeName, _inputTableVarName);
			return this;
		}
		public SaveProcedureBuilder WriteInputTableCreateTypeStatement(string innerNamespace, string className, List<SchemaColumnInfo> columns)
		{
			_inputTableVarTypeName = string.Format("{0}[{1}Input]",
				string.IsNullOrWhiteSpace(innerNamespace) ? "" : string.Format("[{0}].", innerNamespace),
				className);
			_inputTableVarName = "tbl";
			WriteLine("CREATE TYPE [{0}].[{1}Input] AS table ", innerNamespace, className)
				.Write("(").WriteLine();
			SchemaColumnInfo item = columns[0];
			WriteLine(" {0} {1}{2}{3}{4}",
					item.ColumnName,
					item.DataTypeName.ToLower(),
					getItemSuffix(item),
					item.AllowDBNull ? "" : " NOT NULL",
					item.IsKey ? " UNIQUE" : "");
			for (int i = 1; i < columns.Count; i++)
			{
				item = columns[i];
				WriteLine(",{0} {1}{2}{3}{4}",
					item.ColumnName,
					item.DataTypeName.ToLower(),
					getItemSuffix(item),
					item.AllowDBNull ? "" : " NOT NULL",
					item.IsKey ? " UNIQUE" : "");
			}
			WriteLine(")");
			return this;
		}
		protected string getItemSuffix(SchemaColumnInfo col)
		{
			string ret = "";
			switch (col.DataTypeName.ToLower())
			{
				case "varchar":
				case "nvarchar":
					ret = "(" + col.ColumnSize + ")";
					break;
				// todo: decimal etc.
				default:
					ret = "";
					break;
			}
			return ret;
		}
		public SaveProcedureBuilder WriteLine()
		{
			_sb.AppendLine();
			return this;
		}
		public SaveProcedureBuilder WriteLine(string s, params object[] args)
		{
			Write(s, args).AppendLine();
			return this;
		}
		public SaveProcedureBuilder AppendLine()
		{
			_sb.AppendLine();
			return this;
		}
		public SaveProcedureBuilder Write(string s, params object[] args)
		{
			if (s.Trim() == ")" || s.ToUpper().EndsWith("BEGIN"))
				_indentLevel--;
			if (_indentLevel > 0)
				_sb.Append(new string('\t', _indentLevel));
			if (args.Length > 0)
				_sb.AppendFormat(s, args);
			else
				_sb.Append(s);
			if (s.Trim() == "(" || s.ToUpper().EndsWith("END"))
				_indentLevel++;
			return this; // _sb;
		}
		/// <summary>
		/// Gets the contents of the procedure being built by this instance
		/// </summary>
		public string GetSaveProcedureString()
		{
			return _sb.ToString();
		}
		#endregion
	}
}
