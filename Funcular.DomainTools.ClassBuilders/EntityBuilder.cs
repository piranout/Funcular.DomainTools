#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Funcular.DomainTools.Utilities;
using IQToolkit;

#endregion

namespace Funcular.DomainTools.ClassBuilders
{
	public class EntityBuilder
	{
		protected IList<string> _includeNamespaces;
		private readonly IDictionary<string, string> _columnNameNewNameMappings = new Dictionary<string, string>();
		private readonly IDictionary<string, string> _classNameNewNameMappings = new Dictionary<string, string>();
        protected GeneratorOptions _options;
        //private string _inherits = " : BaseEntity";

        public EntityBuilder(GeneratorOptions options)
		{
			_options = options;
			_includeNamespaces = new List<string>()
				{
					"System",
					"System.Collections.Generic"
				};
            if(options.Usings.HasValue())
            _includeNamespaces.AddRange(options.Usings.Split(new []{';'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()));
		}

		#region StringBuilder Helper Methods

		protected int _indentLevel = 0;
		private readonly StringBuilder _sb = new StringBuilder();

		public EntityBuilder Clear()
		{
			_sb.Clear();
			_indentLevel = 0;
			return this;
		}

		public EntityBuilder WriteProperty(string propertyName, string propertyType, bool useNullableType, bool useAutomaticProperty)
		{
			string propertyVariable =
			    $"_{propertyName[0].ToString(CultureInfo.InvariantCulture).ToLower()}{propertyName.Substring(1)}";
			if (useNullableType)
				propertyType = $"{propertyType}?";
			if (_options.GenerateCrmSpecificProperties && _options.GenerateFluentEFMappings)
			{
			    var newPropertyname = propertyName.Collapse(StringHelpers.ChangeCaseTypes.PascalCase, false, _options.AdditionalCollapseTokens.Split(';'));
				this._columnNameNewNameMappings[propertyName] = newPropertyname;
				propertyName = newPropertyname;
			}
			if (!useAutomaticProperty)
			{

				WriteLine("protected {0} {1};", propertyType, propertyVariable);
				WriteLine("public virtual {0} {1}", propertyType, propertyName);
				WriteLine("{");
				WriteLine($"get {"{"} return {propertyVariable}; {"}"}");
				WriteLine($"set {"{"} {propertyVariable} = value; {"}"}");
				WriteLine("}");
			}
			else
			{
				WriteLine("public virtual {0} {1} {{ get; set; }}", propertyType, propertyName);
			}
			return this;
		}

		public EntityBuilder WriteClassDeclaration(string className, string inherits, string implementsInterfaces, string classAttributes, string additionalFind = null, string additionalReplace = null, string tableName = null)
		{
		    string implementsSeparator = 
				inherits.HasValue() && implementsInterfaces.HasValue() ? ", " : "";
			var inheritanceString = inherits.HasValue(nonWhitespaceOnly: true) ? inherits : " ";
			if (implementsInterfaces.HasValue())
			{
				implementsInterfaces = replaceTokens(implementsInterfaces, className, additionalFind, additionalReplace);
				inheritanceString += implementsSeparator + implementsInterfaces;
			}
			if (this._options.GenerateCrmSpecificProperties)
			{
				
					inheritanceString = inheritanceString.Replace("new_", "");
					string newClassName;
			    newClassName = 
                    className.Collapse(StringHelpers.ChangeCaseTypes.PascalCase, false, 
                    this._options.AdditionalCollapseTokens.HasValue() ? this._options.AdditionalCollapseTokens.Split(';') : "new_;".Split(';'));
			    this._classNameNewNameMappings[className] = newClassName;
					className = newClassName;
			}
		    WriteClassAttributes(classAttributes, tableName);
			WriteLine(
				"public partial class {0} {1}",
				className,
                inheritanceString.HasValue(nonWhitespaceOnly: true) ? ": " + inheritanceString.Trim() : "")
			.WriteLine("{");
			return this;
		}

	    private void WriteClassAttributes(string entityAttributes, string tableName)
	    {
            if (_options.AddColumnNameAttributes)
            {
                WriteLine($"[Table(\"{tableName}\")]");
            }
			if (string.IsNullOrWhiteSpace(entityAttributes))
	            return;
	        var originalIndentLevel = _indentLevel;
	        _indentLevel = 1;
	        var entityAttributeList = entityAttributes.Split(new[] {';', '|', '[', ']'}, StringSplitOptions.RemoveEmptyEntries).ToArray();
	        WriteLine();
	        foreach (var entityAttribute in entityAttributeList)
	        {
	            WriteLine($"[{entityAttribute}]");
	        }
	        _indentLevel = originalIndentLevel;
	    }

	    private string replaceTokens(string stringWithTokens, string entityName, string additionalFind = null, string additionalReplace = null)
		{
			string replacement = entityName;
			string ret = stringWithTokens.Replace("%classname%", entityName);
			if (additionalFind.HasValue() && additionalReplace.HasValue())
				ret = ret.Replace(additionalFind, additionalReplace);
			return ret;
		}

		public EntityBuilder WriteNamespaceDeclaration(string classNamespace)
		{
			_indentLevel = 0;
			this.WriteLine("namespace {0} ", classNamespace)
				.WriteLine("{");
			return this;
		}

		public EntityBuilder WriteUsingNamespaces()
		{
			_indentLevel = 0;
            if (_options.AddColumnNameAttributes && !_includeNamespaces.Contains("System.ComponentModel.DataAnnotations.Schema", StringComparer.OrdinalIgnoreCase))
            {
				_includeNamespaces.Add("System.ComponentModel.DataAnnotations.Schema");
            }
			_includeNamespaces = _includeNamespaces.OrderBy(x => x).ToList();
            foreach (string item in _includeNamespaces.Distinct())
			{
				WriteLine("using {0};", item);
			}
		    WriteLine();
			return this;
		}

		public EntityBuilder WriteLine(string s, params object[] args)
		{
			Write(s, args)
				.WriteLine();
			return this;
		}

		public EntityBuilder WriteLine()
		{
			_sb.AppendLine();
			return this;
		}

		public EntityBuilder Write(string s, params object[] args)
		{
			try
			{
				if (s.Trim() == "}")
					_indentLevel--;
				if (_indentLevel > 0)
					_sb.Append(new string('\t', _indentLevel));
				if (args.Length > 0)
					_sb.AppendFormat(s, args);
				else
					_sb.Append(s);
				if (s.Trim() == "{")
					_indentLevel++;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			return this; // _sb;
		}

		/// <summary>
		///     Gets the contents of the class being built by this instance
		/// </summary>
		public string GetClassString()
		{
			return _sb.ToString();
		}

		#endregion

		public IList<string> IncludeNamespaces
		{
			get { return _includeNamespaces; }
			set { _includeNamespaces = value; }
		}

		public IDictionary<string, string> ColumnNameNewNameMappings
		{
			get { return this._columnNameNewNameMappings; }
		}

		public IDictionary<string, string> ClassNameNewNameMappings
		{
			get { return this._classNameNewNameMappings; }
		}
	}
}