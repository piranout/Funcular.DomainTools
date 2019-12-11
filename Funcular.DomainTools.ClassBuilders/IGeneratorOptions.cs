using System;
using System.Collections.Generic;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;

namespace Funcular.DomainTools.ClassBuilders
{
    public interface IGeneratorOptions
    {
        string OutputDirectory { get; set; }
        string BaseNamespace { get; set; }
        string EntityNamespace { get; set; }
        string DataProviderNamespace { get; set; }
        string EntitiesInherit { get; set; }
        string EntitiesImplementInterfaces { get; set; }
        string SqlConnectionString { get; set; }
        string SqlTextCommand { get; set; }
        string AdditionalTokenFind { get; set; }
        string AdditionalTokenReplace { get; set; }
        string AdditionalCollapseTokens { get; set; }

        bool GenerateFluentEFMappings { get; set; }
        bool GenerateCrmSpecificProperties { get; set; }
        bool ImplementIdGenerator { get; set; }
        bool UseAutomaticProperties { get; set; }
        String GeneratedIdDataType { get; set; }
        IEnumerable<SchemaColumnInfo> ColumnInfos { get; set; }
        string Usings { get; set; }
    }
}