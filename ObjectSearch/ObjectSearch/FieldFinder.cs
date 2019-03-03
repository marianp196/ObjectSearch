using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectSearch.ObjectSearch
{
	public class FieldFinder
	{
		public IEnumerable<FieldResult> GetFields(Type type ,Type[] searchTypes, string[] ignoreFields)
		{
			var result = new List<FieldResult>();
			
			foreach(PropertyInfo property in type.GetProperties())
			{
				if (shouldIgnore(property, searchTypes, ignoreFields))
					continue;
				var f = getField(property, searchTypes, ignoreFields);
				result.Add(f);
			}

			return result;
		}

		private FieldResult getField(PropertyInfo property, Type[] searchTypes, string[] ignoreFields)
		{			
			var propType = property.GetType();
			var result = new FieldResult() { Property = property };

			if(propType.IsByRef)
			{
				foreach(PropertyInfo prop in propType.GetProperties())
				{
					if (shouldIgnore(prop, searchTypes, ignoreFields))
						continue;

					var child = getField(prop, searchTypes, ignoreFields);

					result.Next.Add(child);
				}

				return result;
			}
			else
			{
				return result;
			}
		}

		private bool shouldIgnore(PropertyInfo propertyInfo, Type[] searchTypes, string[] ignoreFields)
		{
			bool ignoreByName = ignoreFields.Where(f => propertyInfo.Name == f).Count() > 1;
			bool ignoreByType = searchTypes.Where(t => propertyInfo.GetType() == t).Count() == 0;

			return ignoreByName || ignoreByType;
		}
	}

	public class FieldResult
	{
		public PropertyInfo Property { get; set; }
		public IList<FieldResult> Next { get; set; } = new List<FieldResult>();
	}
}
