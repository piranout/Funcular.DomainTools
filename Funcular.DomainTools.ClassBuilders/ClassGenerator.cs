using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;
using Funcular.DomainTools.Utilities;
using Humanizer;

namespace Funcular.DomainTools.ClassBuilders
{
    public class ClassGenerator
    {
        public const StringHelpers.ChangeCaseTypes PascalCase = StringHelpers.ChangeCaseTypes.PascalCase;
        private readonly EntityBuilder _builder;
        private readonly GeneratorOptions _options;

        public ClassGenerator(EntityBuilder builder, GeneratorOptions options)
        {
            _builder = builder;
            _options = options;
        }

        public void CreateProcedureClass(string procedureName, ICollection<SchemaColumnInfo> schemaColumns/*, ClassConfiguration classConfiguration*/)
        {
            string className = BuildClassForProcedure(procedureName);
            string directory = GetEntityOutputDirectory();
            string classFilePath = Path.Combine(directory, string.Format("{0}{1}", className, ".cs"));

            var fileText = _builder.GetClassString();
            using (StreamWriter outfile =
                new StreamWriter(classFilePath))
            {
                outfile.Write(value: fileText.ToString(CultureInfo.InvariantCulture));
            }
            _builder.Clear();
        }

        public void WriteTableClass(string tableName, ICollection<SchemaColumnInfo> schemaColumns)
        {
            var classConfiguration = BuildClassForTable(tableName, schemaColumns);
            string entityFilePath = Path.Combine(classConfiguration.EntityOutputDirectory, $"{classConfiguration.ClassName}.cs");

            string entityFileText = this._builder.GetClassString();
            using (var outfile = new StreamWriter(entityFilePath))
            {
                outfile.Write(value: entityFileText);
            }

            // TODO:
            if (_options.GenerateInheritingBusinessObjects)
            {
                CreateBusinessObjectClass(classConfiguration);
                // var businessFileText = CreateBusinessObjectClass(classConfiguration);
            }


            
            // Create partial entity class if it doesn't exist:
            CreatePartialEntityFile(entityFilePath, entityFileText);
            if (_options.GenerateFluentEFMappings)
            {
                _builder.Clear();
                var mappingDirectoryPath = Path.Combine(classConfiguration.EntityOutputDirectory, "Mappings");
                new DirectoryInfo(mappingDirectoryPath).EnsureExists();
                string mappingFilePath = Path.Combine(mappingDirectoryPath, classConfiguration.ClassName + "Mapping.cs");
                WriteEntityFrameworkMappingConfiguration(classConfiguration);
                using (var outfile = new StreamWriter(mappingFilePath))
                {
                    outfile.Write(value: _builder.GetClassString());
                }
                var partialMappingClassName = mappingFilePath.Replace(".cs", ".partial.cs");
                if (!File.Exists(partialMappingClassName))
                {
                    WriteEntityFrameworkMappingPartialClass(configuration: classConfiguration);
                    using (var outfile = new StreamWriter(partialMappingClassName))
                    {
                        outfile.Write(value: this._builder.GetClassString());
                    }
                }
            }
            _builder.Clear();
        }

        public void CreateBusinessObjectClass(ClassConfiguration classConfiguration)
        {
            var businessObjectOutputDirectoryPath = GetBusinessObjectsOutputDirectory();
            var businessObjectNamespace = GetBusinessObjectNamespace();
            var businessObjectClassName = classConfiguration.ClassName.StripRight(_options.EntitySuffix);
            var businessObjectFilePath = Path.Combine(businessObjectOutputDirectoryPath, $"{businessObjectClassName}.cs");
            _builder.Clear()
                .WriteLine($"using {classConfiguration.EntityNamespace};")
                .WriteUsingNamespaces()
                .WriteLine()
                .WriteNamespaceDeclaration(businessObjectNamespace)
                .WriteClassDeclaration(businessObjectClassName,
                classConfiguration.InnerNamespace + "." + classConfiguration.ClassName, 
                string.Empty, 
                "Serializable",
                applyDataAnnotationAttributes: false)
                .WriteLine("}")
                .WriteLine("}");
            string businessObjectFileText = this._builder.GetClassString();
            using (var outfile = new StreamWriter(businessObjectFilePath))
            {
                outfile.Write(value: businessObjectFileText);
            }

            _builder.Clear();
        }

        private static void CreatePartialEntityFile(string entityFilePath, string entityFileText)
        {
            var partialEntityFilePath = entityFilePath.Replace(".cs", ".partial.cs");
            if (!File.Exists(partialEntityFilePath))
            {
                var sb = new StringBuilder();
                var sr = new StringReader(entityFileText);
                string line;
                do
                {
                    line = sr.ReadLine();
                    // don't duplicate class attributes:
                    if(line?.Trim().StartsWith("[") == false)
                        sb.AppendLine(line);
                } while (line?.Contains("public partial class") == false);

                // get the class open curly brace
                sb.AppendLine(line = sr.ReadLine());
                // flip it to close the class
                sb.AppendLine(line?.Replace("{", "}"));
                // then close the namespace
                sb.AppendLine("}");


                var partialEntityFileText = sb;
                using (var outfile = new StreamWriter(partialEntityFilePath))
                {
                    outfile.Write(value: partialEntityFileText);
                }
            }
        }


        /// <summary>
        /// Gets the BusinessObjects output directory and ensures it exists
        /// </summary>
        /// <param name="classConfiguration"></param>
        /// <returns></returns>
        private string GetBusinessObjectsOutputDirectory(ClassConfiguration classConfiguration = null)
        {
            var businessObjectsOutputDirectory = Path.Combine(_options.OutputDirectory, _options.BusinessObjectsSubdirectory).EnsureEndsWith("\\");
            if (!Directory.Exists(businessObjectsOutputDirectory))
                Directory.CreateDirectory(businessObjectsOutputDirectory);
            return businessObjectsOutputDirectory;
        }

        /// <summary>
        /// Gets the Entity output directory and ensures it exists
        /// </summary>
        /// <param name="classConfiguration"></param>
        /// <returns></returns>
        private string GetEntityOutputDirectory(ClassConfiguration classConfiguration = null)
        {
            var entityOutputDirectory = Path.Combine(_options.OutputDirectory, _options.EntitySubdirectory).EnsureEndsWith("\\");
            if (!Directory.Exists(entityOutputDirectory))
                Directory.CreateDirectory(entityOutputDirectory);
            return entityOutputDirectory;
        }

        public static string RemoveInvalidPathCharacters(string s)
        {
            var invalidCharacters = Path.GetInvalidPathChars();
            return s.RemoveAll(invalidCharacters);

        }

        public void CreateTextCommandClass(string textCommand, ICollection<SchemaColumnInfo> schemaColumns)
        {
            string className = BuildClassForTextCommand(textCommand);
            string classFilePath = Path.Combine(_options.OutputDirectory, $"{className}.cs");

            string fileText = this._builder.GetClassString();
            using (StreamWriter outfile =
                new StreamWriter(classFilePath))
            {
                outfile.Write(fileText.ToString(CultureInfo.InvariantCulture));
            }
            _builder.Clear();
        }

        #region Protected Methods
        /// <summary>
        /// Interrogates a stored proc, builds the class file string from its output, and returns the name of the class.
        /// </summary>
        /// <param name="fullyQualifiedProcedureName"></param>
        /// <returns></returns>
        protected string BuildClassForProcedure(string fullyQualifiedProcedureName, bool alsoCreateSaveStoredProc = false)
        {
            _builder.Clear();
            string procedureName = fullyQualifiedProcedureName.RemoveAll(new char[] { '[', ']' });
            string innerNamespace = StringHelpers.LeftOfFirst(procedureName, ".");
            string entityNamespace = this._options.EntityNamespace.StripRight(".");
            if (!string.IsNullOrWhiteSpace(innerNamespace))
                entityNamespace += "." + innerNamespace;
            string className = GetClassName(fullyQualifiedProcedureName); //string className = StringHelpers.RightOfLast(procedureName, ".");
            string suffix = StringHelpers.RightOfLast(procedureName, "By");
            if (!string.IsNullOrEmpty(suffix))
                suffix = "By" + suffix;
            else
            {
                suffix = StringHelpers.RightOfLast(procedureName, "All");
            }
            className = className.StripRight(suffix); // strip select modifiers
            className = className.Singularize(false);


            className = className.StripLeft("Select"); // common get prefix
            className = className.StripLeft("Get");  // common get prefix

            /*EntityBuilder tempQualifier = this._builder.WriteUsingNamespaces()
                .WriteNamespaceDeclaration(entityNamespace);*/
            _builder.WriteUsingNamespaces()
                .WriteLine()
                .WriteNamespaceDeclaration(entityNamespace)
                .WriteClassDeclaration(className: className, inherits: _options.EntitiesInherit, implementsInterfaces: _options.EntitiesImplementInterfaces, classAttributes: _options.EntityAttributes, tableName: procedureName);
            foreach (var col in GetProcedureColumns(fullyQualifiedProcedureName))
            {
                string propertyName = col.ColumnName;
                string propertyType = col.DataType.Name;
                bool useNullableType = (col.AllowDBNull && col.DataType.IsValueType);
                AddDataAnnotationAttributes(col: col);
                _builder.WriteProperty(propertyName, propertyType, useNullableType, _options.UseAutomaticProperties);
            }
            _builder.WriteLine("}")
                .WriteLine("}");
            return className;
        }

        private void AddDataAnnotationAttributes(SchemaColumnInfo col)
        {
            if (_options.AddDataAnnotationAttributes)
            {
                _builder.WriteLine($"[Column(\"{col.ColumnName}\")]");
                if (col.ProviderSpecificDataType == typeof(System.Data.SqlTypes.SqlString))
                {
                    _builder.WriteLine($"[MaxLength(\"{col.ColumnSize}\")]");
                }
            }
        }

        protected string BuildClassForTextCommand(string textCommand, string overrideEntityNamespace = null)
        {
            _builder.Clear();
            List<string> namespaces = new List<string>
				{
					this._options.BaseNamespace, 
					this._options.EntityNamespace
				};
            int startPos = textCommand.LastIndexOf("FROM", System.StringComparison.Ordinal) + 4;
            string fullyQualifiedTableOrViewName = textCommand.Substring(startPos).Trim();
            int whitespacePos = fullyQualifiedTableOrViewName.IndexOfAny(new char[] { '\r', '\n', '\t', ' ' });
            if (whitespacePos > 0)
                fullyQualifiedTableOrViewName = fullyQualifiedTableOrViewName.Substring(0, whitespacePos + 1).Trim();
            fullyQualifiedTableOrViewName = fullyQualifiedTableOrViewName.RemoveAll(new char[] { '[', ']' });
            string tableOrViewName = StringHelpers.RightOfLast(fullyQualifiedTableOrViewName, ".");
            string innerNamespace = StringHelpers.LeftOfFirst(fullyQualifiedTableOrViewName, ".");
            if (!string.IsNullOrWhiteSpace(innerNamespace) && innerNamespace.ToLower() != "dbo")
            {
                namespaces.Add(innerNamespace);
            }
            var entityNamespace = string.Join(".", namespaces
                .FindAll(s => !string.IsNullOrEmpty(s))
                .ToArray());
            string className = GetClassName(tableOrViewName);

            _builder.WriteUsingNamespaces()
                .WriteLine()
                .WriteNamespaceDeclaration(entityNamespace)
                .WriteClassDeclaration(className, _options.EntitiesInherit, _options.EntitiesImplementInterfaces, _options.EntityAttributes, tableName: tableOrViewName);

            foreach (var col in GetTextCommandColumns(textCommand))
            {
                if (col.IsHidden)
                    continue;
                string propertyName = col.ColumnName;
                string propertyType = col.DataType.Name;
                bool useNullableType = (col.AllowDBNull && col.DataType.IsValueType);
                this._builder.WriteProperty(propertyName, propertyType, useNullableType, _options.UseAutomaticProperties);
            }
            _builder.WriteLine("}");
            _builder.WriteLine("}");
            return className;
        }

        /// <summary>
        /// Interrogates a table, builds the class file STRING from its output, and returns the name of the class.
        /// Note: the class's .cs file contents have still not been written after this method call.
        /// </summary>
        /// <param name="fullyQualifiedTableName"></param>
        /// <returns></returns>
        protected ClassConfiguration BuildClassForTable(string fullyQualifiedTableName, ICollection<SchemaColumnInfo> schemaColumns, string overrideEntityNamespace = null)
        {
            _builder.Clear();
            string tableName = fullyQualifiedTableName.RemoveAll(new char[] { '[', ']' });
            string innerNamespace = StringHelpers.LeftOfFirst(tableName, ".");
            
            var entityNamespace = GetEntityNamespace();

            bool foundPk = false;
            SchemaColumnInfo primaryKeyColumn = null;

            if ((primaryKeyColumn = (schemaColumns.FirstOrDefault(col => col.IsKey))) != null)
            {
                foundPk = true;
            }
            
            string className = GetClassName(fullyQualifiedTableName);
            string unqualifiedTableName = StringHelpers.RightOfLast(tableName, ".");
            var config = new ClassConfiguration(schemaColumns)
            {
                ClassName = className,
                EntityNamespace = entityNamespace,
                InnerNamespace = innerNamespace,
                PrimaryKeyColumn = primaryKeyColumn,
                UnqualifiedTableName = unqualifiedTableName
            };
            config.EntityOutputDirectory = GetEntityOutputDirectory(config);

            _builder
                .WriteUsingNamespaces()
                .WriteLine()
                .WriteNamespaceDeclaration(entityNamespace)
                .WriteClassDeclaration(
                    className: className, 
                    // TODO: Move class-specific _Options properties onto ClassConfiguration
                    inherits: _options.EntitiesInherit, 
                    implementsInterfaces: _options.EntitiesImplementInterfaces, 
                    classAttributes: _options.EntityAttributes, 
                    additionalFind: _options.AdditionalTokenFind, 
                    additionalReplace: _options.AdditionalTokenReplace,
                    tableName: tableName);
           

            //List<SchemaColumnInfo> columnInfos = getTableColumns(fullyQualifiedTableName);
            foreach (var col in schemaColumns)
            {
                string propertyName = col.PropertyName; //col.ColumnName;
                string propertyType = col.DataType.Name;
                string columnName = col.ColumnName ?? col.BaseColumnName;
                bool useNullableType = (col.AllowDBNull && col.DataType.IsValueType);
                string newPropertyName = propertyName != col.ColumnName
                    ? propertyName
                    : col.ColumnName.Collapse(StringHelpers.ChangeCaseTypes.PascalCase, false, this._options.AdditionalCollapseTokens.Split(';'));
                if (col.IsKey || (!foundPk && (columnName.Equals(unqualifiedTableName + "ID", StringComparison.OrdinalIgnoreCase) || columnName.Equals("ID", StringComparison.OrdinalIgnoreCase))))
                {
                    foundPk = true;
                    primaryKeyColumn = col;
                    primaryKeyColumn.PropertyName = newPropertyName = _options.PrimaryKeyGetsNamedId ? "Id" : primaryKeyColumn.PropertyName;
                    if (_options.GenerateCrmSpecificProperties && !className.EndsWith("ExtensionBase"))
                    {
                        _builder.WriteLine("public virtual {0} {1}", propertyType, newPropertyName)
                            .WriteLine("{");
                        _builder.WriteLine("get { return Id; }");
                        _builder.WriteLine("set { Id = value; }");
                        _builder.WriteLine("}");
                    }
                }

                _builder.ColumnNameNewNameMappings[columnName] = newPropertyName;
                AddDataAnnotationAttributes(col: col);
                _builder.WriteProperty(newPropertyName, propertyType, useNullableType, _options.UseAutomaticProperties);
            }
            if (_options.GenerateCrmSpecificProperties && !className.EndsWith("ExtensionBase"))
            {
                string extensionClassName = className.StripRight("Base") + "ExtensionBase";
                _builder.WriteProperty("Extension", extensionClassName, false, _options.UseAutomaticProperties);
            }
            _builder.WriteLine("}");
            _builder.WriteLine("}");
            return config;
        }

        public string GetBusinessObjectNamespace()
        {
            // TODO add inner namespaces:
            var suffix = _options.BusinessObjectsNamespace.HasValue()
                ? _options.BusinessObjectsNamespace
                : "BusinessObjects";
            return string.Join(".", _options.BaseNamespace, suffix);
        }

        public string GetEntityNamespace()
        {
            // TODO add inner namespaces:
            var suffix = _options.EntityNamespace.HasValue()
                ? _options.EntityNamespace
                : "Entities";
            return string.Join(".", _options.BaseNamespace, suffix);
        }

        public void WriteEntityFrameworkMappingConfiguration(ClassConfiguration classConfiguration)
        {
            var newClassName = BeginMappingConfigurationClass(classConfiguration);
            _builder.WriteLine("public " + newClassName + "Mapping()")
                .WriteLine("{"); //constructor
            if (classConfiguration.PrimaryKeyColumn != null)
            {
                _builder.WriteLine("HasKey(item => item.{0});",
                    _builder.ColumnNameNewNameMappings.ContainsKey(classConfiguration.PrimaryKeyColumn.ColumnName)
                        ? _builder.ColumnNameNewNameMappings[classConfiguration.PrimaryKeyColumn.ColumnName]
                        : classConfiguration.PrimaryKeyColumn.PropertyName);
            }
            foreach (SchemaColumnInfo schemaColumnInfo in classConfiguration.SchemaColumns)
            {
                // map any columns with different names from the corresponding entity properties:
                if (_builder.ColumnNameNewNameMappings.ContainsKey(schemaColumnInfo.ColumnName))
                {
                    string newPropertyName = _builder.ColumnNameNewNameMappings[schemaColumnInfo.ColumnName];
                    if (newPropertyName != schemaColumnInfo.ColumnName)
                    {
                        _builder.WriteLine(
                            "Property(item => item.{0}).HasColumnName(\"{1}\");", newPropertyName, schemaColumnInfo.ColumnName);
                    }
                }
            }
            if (classConfiguration.InnerNamespace != null)
            {
                if (classConfiguration.InnerNamespace.ToLower() == "dbo")
                {
                    _builder.WriteLine("ToTable(\"{0}\");", classConfiguration.UnqualifiedTableName);
                }
                else
                {
                    _builder.WriteLine("ToTable(\"{0}\", \"{1}\");", classConfiguration.UnqualifiedTableName, classConfiguration.InnerNamespace);
                }
                if (_options.GenerateCrmSpecificProperties && !newClassName.Contains("ExtensionBase"))
                {
                    _builder.WriteLine("HasRequired(item => item.Extension).WithRequiredPrincipal();");
                }
            }
            _builder.WriteLine("initialize();");
            this._builder.WriteLine("}"); //constructor
            CloseEntityConfigurationClass();
        }

        public void WriteEntityFrameworkMappingPartialClass(ClassConfiguration configuration)
        {
            var newClassName = BeginMappingConfigurationClass(configuration, true);
            _builder.WriteLine("protected void initialize()");
            _builder.WriteLine("{");
            _builder.WriteLine("// ADD RELATIONSHIPS AND CUSTOM LOGIC HERE");
            _builder.WriteLine("}");
            CloseEntityConfigurationClass();
        }

        protected void CloseEntityConfigurationClass()
        {
            this._builder.WriteLine("}"); //class
            this._builder.WriteLine("}"); //namespace
        }

        private string BeginMappingConfigurationClass(ClassConfiguration configuration, bool isPartial = false)
        {
            this._builder.Clear();
            string newClassName = this._builder.ClassNameNewNameMappings.ContainsKey(configuration.ClassName)
                ? this._builder.ClassNameNewNameMappings[configuration.ClassName]
                : configuration.ClassName;

            this._builder
                .WriteLine("using System.Data.Entity.ModelConfiguration;")
                .WriteUsingNamespaces()
                .WriteLine()
                .WriteNamespaceDeclaration(configuration.EntityNamespace + ".Mappings");
            if (isPartial == true)
            {
                this._builder
                    .WriteLine("/* This file is for hand-written custom relationships and navigation properties;")
                    .WriteLine($" *  edit this file only, not the corresponding .cs file. */");
            }
            else
            {
                this._builder
                    .WriteLine("/* This file was created by a generator; do not edit it directly. In order to add")
                    .WriteLine(" *\trelationships and navigation properties, use the corresponding .partial.cs file. */");
            }
            _builder.WriteClassDeclaration(className: newClassName + "Mapping", inherits: "EntityTypeConfiguration<" + newClassName + ">", implementsInterfaces: null, classAttributes: _options.MappingAttributes);
            return newClassName;
        }

        protected string GetBusinessObjectClassName(ClassConfiguration classConfiguration, string className)
        {
            if (_options.EntitySuffix.HasValue() &&
                className.EndsWith(_options.EntitySuffix, StringComparison.OrdinalIgnoreCase))
            {
                return className
                    .LeftOfLast(_options.EntitySuffix);
            }

            return className;
        }

        protected string GetClassName(string fullyQualifiedTableName)
        {
            string tableName = fullyQualifiedTableName.RemoveAll(new[] { '[', ']' });
            string className;
            className = StringHelpers.RightOfLast(tableName, ".");
            className = className.Collapse(PascalCase, false, "_");
            className = className.Singularize(inputIsKnownToBePlural: false);
            if (_options.EntitySuffix.HasValue())
            {
                className = $"{className}{_options.EntitySuffix}";
            }
            return className;
        }

        protected List<SchemaColumnInfo> GetTextCommandColumns(string textCommand)
        {
            SqlServerInterrogator inter = new SqlServerInterrogator(_options.SqlConnectionString);
            return inter.GetTextCommandColumnInfoList(textCommand).ToList();
        }
        protected List<SchemaColumnInfo> GetTableColumns(string tableName)
        {
            SqlServerInterrogator inter = new SqlServerInterrogator(_options.SqlConnectionString);
            return inter.GetTableColumnInfoList(tableName).ToList();
        }
        protected List<SchemaColumnInfo> GetProcedureColumns(string procedureName)
        {
            SqlServerInterrogator inter = new SqlServerInterrogator(_options.SqlConnectionString);
            return inter.GetProcedureColumnInfoList(procedureName).ToList();
        }
        #endregion Protected Methods
    }

}
