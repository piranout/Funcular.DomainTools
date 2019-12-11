using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Funcular.DomainTools.Utilities
{
	public class ObjectDumper
	{
		private int _level;
		private readonly int _indentSize;
		private readonly StringBuilder _stringBuilder;
		private readonly List<int> _hashListOfFoundElements;

		private ObjectDumper(int indentSize)
		{
			_indentSize = indentSize;
			_stringBuilder = new StringBuilder();
			_hashListOfFoundElements = new List<int>();
		}

		public static string Dump(object element)
		{
			return Dump(element, 2);
		}

		public static string Dump(object element, int indentSize)
		{
			var instance = new ObjectDumper(indentSize);
			return instance.dumpElement(element);
		}

		private string dumpElement(object element)
		{
			if (element == null || element is ValueType || element is string)
			{
				write(formatValue(element));
			}
			else
			{
				var objectType = element.GetType();
				if (!typeof(IEnumerable).IsAssignableFrom(objectType))
				{
					write("{{{0}}}", objectType.FullName);
					_hashListOfFoundElements.Add(element.GetHashCode());
					_level++;
				}

				var enumerableElement = element as IEnumerable;
				if (enumerableElement != null)
				{
					foreach (object item in enumerableElement)
					{
						if (item is IEnumerable && !(item is string))
						{
							_level++;
							dumpElement(item);
							_level--;
						}
						else
						{
							if (!alreadyTouched(item))
								dumpElement(item);
							else
								write("{{{0}}} <-- bidirectional reference found", item.GetType().FullName);
						}
					}
				}
				else
				{
					MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
					foreach (var memberInfo in members)
					{
						var fieldInfo = memberInfo as FieldInfo;
						var propertyInfo = memberInfo as PropertyInfo;

						if (fieldInfo == null && propertyInfo == null)
							continue;

						var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;
						object value = fieldInfo != null
										   ? fieldInfo.GetValue(element)
										   : propertyInfo.GetValue(element, null);

						if (type.IsValueType || type == typeof(string))
						{
							write("{0}: {1}", memberInfo.Name, formatValue(value));
						}
						else
						{
							var isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
							write("{0}: {1}", memberInfo.Name, isEnumerable ? "..." : "{ }");

							var alreadyTouched = !isEnumerable && this.alreadyTouched(value);
							_level++;
							if (!alreadyTouched)
								dumpElement(value);
							else
								write("{{{0}}} <-- bidirectional reference found", value.GetType().FullName);
							_level--;
						}
					}
				}
				if (!typeof(IEnumerable).IsAssignableFrom(objectType))
				{
					_level--;
				}
			}
			return _stringBuilder.ToString();
		}

		private bool alreadyTouched(object value)
		{
			var hash = value.GetHashCode();
			foreach (int t in this._hashListOfFoundElements)
			{
				if (t == hash)
					return true;
			}
			return false;
		}

		private void write(string value, params object[] args)
		{
			var space = new string(' ', _level * _indentSize);
			if (args != null)
				value = string.Format(value, args);
			_stringBuilder.AppendLine(space + value);
		}

		private string formatValue(object o)
		{
			if (o == null)
				return ("null");
			if (o is DateTime)
				return (((DateTime)o).ToShortDateString());
			if (o is string)
				return string.Format("\"{0}\"", o);
			if (o is ValueType)
				return (o.ToString());
			if (o is IEnumerable)
				return ("...");
			return ("{ }");
		}
	}
}
