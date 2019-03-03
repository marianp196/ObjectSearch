using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectSearcher.ObjectSearcher
{
	public class FieldFilter
	{
		public IEnumerable<FieldPath> GetFields(Type type ,Type[] searchTypes, string[] ignoreFields)
		{
			var result = new List<FieldPath>();
			
			foreach(PropertyInfo property in type.GetProperties())
			{				
				var f = getField(property, searchTypes, ignoreFields);
				if(f!= null)
					result.Add(f);
			}

			return result;
		}

		private FieldPath getField(PropertyInfo property, Type[] searchTypes, string[] ignoreFields)
		{
			var type = property.PropertyType;
			var isEnumerable = false;

			if (property.PropertyType.BaseType == typeof(IEnumerable<>).BaseType)
			{
				isEnumerable = true;
				type = property.PropertyType.GetGenericArguments()[0];
			}
				
			if (shouldIgnore(type, property.Name ,searchTypes, ignoreFields))
				return null;

			var result = new FieldPath() { Property = property, IsEnumeruable = isEnumerable};

			foreach(PropertyInfo prop in type.GetProperties())
			{			
				var child = getField(prop, searchTypes, ignoreFields);

				if (child == null)
					continue;

				result.Next.Add(child);
			}

			return result;
			
		}

		private bool shouldIgnore(Type type, string propertyName, Type[] searchTypes, string[] ignoreFields)
		{
			bool ignoreByName = ignoreFields.Where(f => propertyName == f).Count() > 0;
			bool ignoreByType = searchTypes.Where(t => type == t).Count() == 0;

			return ignoreByName || ignoreByType;
		}
	}

	public class FieldPath
	{
		public PropertyInfo Property { get; set; }
		public bool IsEnumeruable { get; set; }
		public IList<FieldPath> Next { get; set; } = new List<FieldPath>();
	}
}
