﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;

namespace Funcular.DomainTools.ClassBuilders
{
    public class GeneratorOptions : IGeneratorOptions
	{

	    private string _outputDirectory;

        public string OutputDirectory
	    {
	        get { return ClassGenerator.RemoveInvalidPathCharacters(_outputDirectory); }
	        set { _outputDirectory = value; }
	    }

        public string ProjectFile { get; set; }

        public string SqlConnectionString { get; set; }
		public string SqlTextCommand { get; set; }
		public string AdditionalTokenFind { get; set; }
		public string AdditionalTokenReplace { get; set; }
		public string AdditionalCollapseTokens { get; set; }

		public string BaseNamespace { get; set; }
		public string EntityNamespace { get; set; }
		public string BusinessObjectsNamespace { get; set; }
		public string EntitiesInherit { get; set; }
		public string EntitiesImplementInterfaces { get; set; }
        public string Usings { get; set; }
        public bool GenerateInheritingBusinessObjects { get; set; }
        public bool RemoveDboFromOutputFolderPath { get; set; }
        public string EntitySuffix { get; set; }
        public string EntitySubdirectory { get; set; }
        public string BusinessObjectsSubdirectory { get; set; }

        public string DataProviderNamespace { get; set; }

		public bool GenerateFluentEFMappings { get; set; }
		public bool GenerateCrmSpecificProperties { get; set; }
		public bool ImplementIdGenerator { get; set; }
		public bool UseAutomaticProperties { get; set; }
        public bool PrimaryKeyGetsNamedId { get; set; }
		public string GeneratedIdDataType { get; set; }

		public bool AddDataAnnotationAttributes { get; set; }

	    public string EntityAttributes { get; set; }

        public string MappingAttributes{ get; set; }
        [Obsolete("These are on a per-entity basis now, not a single collection for the whole project.",false)]
        public IEnumerable<SchemaColumnInfo> ColumnInfos
        {
            get;
            set;
        }

	    public StringCollection SavedConnectionStrings { get; set; }
	}
}
