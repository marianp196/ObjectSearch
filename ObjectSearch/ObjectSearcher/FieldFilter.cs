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
			if (shouldIgnore(property, searchTypes, ignoreFields))
				return null;

			var propType = property.PropertyType;
			var result = new FieldPath() { Property = property };

			foreach(PropertyInfo prop in propType.GetProperties())
			{			
				var child = getField(prop, searchTypes, ignoreFields);

				if (child == null)
					continue;

				result.Next.Add(child);
			}

			return result;
			
		}

		private bool shouldIgnore(PropertyInfo propertyInfo, Type[] searchTypes, string[] ignoreFields)
		{
			bool ignoreByName = ignoreFields.Where(f => propertyInfo.Name == f).Count() > 1;
			bool ignoreByType = searchTypes.Where(t => propertyInfo.PropertyType == t).Count() == 0;

			return ignoreByName || ignoreByType;
		}
	}

	public class FieldPath
	{
		public PropertyInfo Property { get; set; }
		public IList<FieldPath> Next { get; set; } = new List<FieldPath>();
	}
}
