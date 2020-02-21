using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;

namespace Funcular.DomainTools.ClassBuilders
{
    public interface IGeneratorOptions
    {
        string OutputDirectory { get; set; }
        string ProjectFile { get; set; }
        string SqlConnectionString { get; set; }
        string SqlTextCommand { get; set; }
        string AdditionalTokenFind { get; set; }
        string AdditionalTokenReplace { get; set; }
        string AdditionalCollapseTokens { get; set; }
        string BaseNamespace { get; set; }
        string EntityNamespace { get; set; }
        string BusinessObjectsNamespace { get; set; }
        string EntitiesInherit { get; set; }
        string EntitiesImplementInterfaces { get; set; }
        string Usings { get; set; }
        bool GenerateInheritingBusinessObjects { get; set; }
        bool RemoveDboFromOutputFolderPath { get; set; }
        string EntitySuffix { get; set; }
        bool TableNameStemIsInnerNamespace { get; set; }
        string EntitySubdirectory { get; set; }
        string BusinessObjectsSubdirectory { get; set; }
        string DataProviderNamespace { get; set; }
        bool GenerateFluentEFMappings { get; set; }
        bool GenerateCrmSpecificProperties { get; set; }
        bool ImplementIdGenerator { get; set; }
        bool UseAutomaticProperties { get; set; }
        bool PrimaryKeyGetsNamedId { get; set; }
        string GeneratedIdDataType { get; set; }
        bool AddDataAnnotationAttributes { get; set; }
        string EntityAttributes { get; set; }
        string MappingAttributes { get; set; }
        IEnumerable<SchemaColumnInfo> ColumnInfos { get; set; }
        StringCollection SavedConnectionStrings { get; set; }
    }
}