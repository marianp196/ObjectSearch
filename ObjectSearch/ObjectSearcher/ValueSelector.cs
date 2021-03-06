﻿using ObjectSearcher.ObjectSearcher;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectSearcher.ObjectSearcher
{
	public class ValueSelector
	{
		public IEnumerable<string> GetSearchableValues<TObj>(IEnumerable<FieldPath> fieldPaths, TObj objDta)
		{
			var searchableValues = new List<string>();
			foreach (var fieldPath in fieldPaths)
			{
				searchableValues.AddRange(GetValuesFromObject<TObj>(objDta, fieldPath));
			}
			return searchableValues;
		}

		private IEnumerable<string> GetValuesFromObject<TObj>(TObj obj, FieldPath fieldPath)
		{
			var result = new List<string>();

			if (fieldPath.Next.Count == 0)
			{
				var value = fieldPath.Property.GetValue(obj);

				if (fieldPath.IsEnumeruable)
				{
					foreach(var item in (IEnumerable<object>)value)
					{
						if (item != null && item.ToString() != "")
							result.Add(item.ToString());
					}
				}
				else
				{
					if (value != null && value.ToString() != "")
						result.Add(value.ToString());
				}
				return result;
			}
			else
			{
				var propValue = fieldPath.Property.GetValue(obj);
				if (propValue == null)
					return result;

				if (fieldPath.IsEnumeruable)
				{
					foreach(var item in (IEnumerable<object>)propValue)
					{
						foreach (var fieldChild in fieldPath.Next)
						{
							var valueList = GetValuesFromObject(item, fieldChild);
							result.AddRange(valueList);
						}
					}
				}
				else
				{
					foreach (var fieldChild in fieldPath.Next)
					{
						var valueList = GetValuesFromObject(propValue, fieldChild);
						result.AddRange(valueList);
					}
				}
				
			}

			return result;
		}
	}
}
