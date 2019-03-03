using ObjectSearcher.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectSearcher.ObjectSearcher
{
	public class ObjectSearch : IObjectSearch
	{
		public IEnumerable<SearchResult<TObj>> Search<TObj>(IEnumerable<TObj> data, string[] searchStrings, SearchConfig config = null)
		{
			var conf = getConfig(config);
			var fieldPaths = _fieldFilter.GetFields(typeof(TObj), conf.SearchTypes, conf.IgnoreFields);


			foreach(var objDta in data)
			{
				var searchableValues = new List<string>();
				foreach (var fieldPath in fieldPaths)
				{
					 searchableValues.AddRange(GetValuesFromObject<TObj>(objDta, fieldPath));
				}
				Console.WriteLine(searchableValues);
			}

			return null;
		}

		private IEnumerable<string> GetValuesFromObject<TObj>(TObj obj, FieldPath fieldPath)
		{
			var result = new List<string>();

			if (fieldPath.Next.Count == 0)
			{
				var value = fieldPath.Property.GetValue(obj).ToString();
				result.Add(value);
				return result;
			}
			else
			{
				var propValue = fieldPath.Property.GetValue(obj);
				foreach (var fieldChild in fieldPath.Next)
				{
					var valueList = GetValuesFromObject(propValue, fieldChild);
					result.AddRange(valueList);
				}				
			}
		
			return result;
		}
				
		private SearchConfig getConfig(SearchConfig input)
		{
			if (input == null)
				return _standardConfig;

			if (input.SearchMode == null)
				input.SearchMode = _standardConfig.SearchMode;
			if (input.IgnoreFields == null)
				input.IgnoreFields = _standardConfig.IgnoreFields;
			if (input.SearchTypes == null)
				input.SearchTypes = _standardConfig.SearchTypes;

			return input;
		}

		private FieldFilter _fieldFilter = new FieldFilter();
		private SearchConfig _standardConfig = new SearchConfig {
			SearchMode = ESearchMode.And,
			IgnoreFields = new string[0],
			SearchTypes = new Type[] { typeof(string) } 
		};
	}
}
