using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funcular.Ontology.Archetypes;

namespace Funcular.DomainTools.ClassBuilders
{
    public class TableBuilder
    {
        public IDictionary<string, Type> GetCreatableTypes()
        {
            var assembly = Assembly.GetAssembly(typeof (IIdentity<>));
            var types = assembly.GetTypes()
                .Where(x => x.IsAbstract && !x.IsInterface)
                .ToDictionary(x => x.Name);
            return types;
        }


    }
}