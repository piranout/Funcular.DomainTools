using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Funcular.DomainTools.ClassBuilders
{
	public static class DataTableExtensions
	{
        private static readonly Dictionary<Type, IList<PropertyInfo>> _typeProperties =
			new Dictionary<Type, IList<PropertyInfo>>();
		public static IList<T> ToList<T>(this DataTable table) where T : new()
		{
			IList<PropertyInfo> properties = tryGetProperties(t: typeof(T));
		    var list = new List<T>();
		    foreach (DataRow row in table.Rows)
		    {
		        list.Add(item: createItemFromRow<T>(row: row, properties: properties));
		    }
		    return list;
		}
		private static IList<PropertyInfo> tryGetProperties(Type t)
		{
			if(!_typeProperties.ContainsKey(t))
				_typeProperties[t] = t.GetProperties().ToList();
			return _typeProperties[t];
		}
		private static T createItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
		{
			var item = new T();
			foreach (var property in properties)
			{

			    try
			    {
			        var value = row[property.Name] == DBNull.Value ? "" : row[property.Name];
			        //value = value == DBNull.Value ? "" : value;
			        property.SetValue(obj: item, value, index: null);
			    }
			    catch (Exception e) // general catch b/c we expect data columns to not exist for some ColumnInfo properties
			    {
			        Console.WriteLine(e);
			    }
			}
			return item;
		}

    }
}
