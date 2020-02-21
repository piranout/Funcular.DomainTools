#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Funcular.DomainTools.ClassBuilders;
using Funcular.DomainTools.ClassBuilders.SqlMetaData;

#endregion

namespace Funcular.DomainTools.Applications.Properties
{
    internal sealed partial class Settings : IGeneratorOptions
	{
	    public Settings()
        {
        }

	    public string this[int index]
	    {
	        get { return Funcular.DomainTools.Applications.Properties.Settings.Default[index]; } 
	    }
		private static readonly Dictionary<Type, IList<PropertyInfo>> _typeProperties =
			new Dictionary<Type, IList<PropertyInfo>>();

        public static Settings FromGeneratorOptions(GeneratorOptions options)
        {
            IList<PropertyInfo> generatorProperties = tryGetProperties(typeof(GeneratorOptions));
            IList<PropertyInfo> settingsProperties = tryGetProperties(typeof(Settings));
            Settings ret = new Settings();
            if (generatorProperties.Any() && settingsProperties.Any())
            {
                foreach (PropertyInfo generatorProperty in generatorProperties)
                {
                    PropertyInfo settingsProperty = settingsProperties.FirstOrDefault(
                        gp =>
                        gp.Name == generatorProperty.Name &&
                        gp.PropertyType == generatorProperty.PropertyType);
                    if (settingsProperty == null)
                        continue;
                    try
                    {
                        settingsProperty.SetValue(ret, generatorProperty.GetValue(options));
                        settingsProperty.SetValue(Funcular.DomainTools.Applications.Properties.Settings.Default, generatorProperty.GetValue(options));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return ret;
        }

		public GeneratorOptions AsGeneratorOptions()
		{
			IList<PropertyInfo> generatorProperties = tryGetProperties(typeof (GeneratorOptions));
			IList<PropertyInfo> settingsProperties = tryGetProperties(typeof (Settings));
			GeneratorOptions ret = new GeneratorOptions();
			if (settingsProperties.Any() && generatorProperties.Any())
			{
				foreach (PropertyInfo setting in settingsProperties)
				{
					PropertyInfo generatorProperty = generatorProperties.FirstOrDefault(
						gp =>
						gp.Name == setting.Name &&
						gp.PropertyType == setting.PropertyType);
					if (generatorProperty == null)
						continue;
					try
					{
						generatorProperty.SetValue(ret, setting.GetValue(this));
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
					}
				}
			}
			return ret;
		}

		private static IList<PropertyInfo> tryGetProperties(Type t)
		{
			if (!_typeProperties.ContainsKey(t))
			{
				_typeProperties[t] = t.GetProperties(
					//BindingFlags.DeclaredOnly | BindingFlags.Instance
					).ToList();
			}
			return _typeProperties[t] ?? new List<PropertyInfo>();
		}


        public IEnumerable<SchemaColumnInfo> ColumnInfos { get; set; }
    }
}