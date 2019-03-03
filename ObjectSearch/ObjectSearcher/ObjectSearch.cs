using ObjectSearcher.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectSearcher.ObjectSearcher
{
	public class ObjectSearch : IObjectSearch
	{
		public IEnumerable<TObj> Search<TObj>(IEnumerable<TObj> data, string[] searchStrings, SearchConfig config = null)
		{
			var conf = getConfig(config);
			var fieldPaths = _fieldFilter.GetFields(typeof(TObj), conf.SearchTypes, conf.IgnoreFields);

			var result = new List<TObj>();

			foreach(var objDta in data)
			{
				var searchableValues = _valueSelector.GetSearchableValues(fieldPaths, objDta);

				if (search(searchableValues, searchStrings, conf.SearchMode))
					result.Add(objDta);
			}

			return result;
		}

		private bool search(IEnumerable<string> values, string[] searchStrings, ESearchMode mode)
		{
			if (mode == ESearchMode.And)
			{
				return andCombine(values, searchStrings);
			}
			else if (mode == ESearchMode.Or)
			{
				return orCombine(values, searchStrings);
			}

			return false;
		}

		private bool orCombine(IEnumerable<string> values, string[] search)
		{
			foreach (var searchString in search)
			{
				if (values.Count(x => x.Contains(searchString)) > 0)
					return true;
			}
			return false;
		}

		private bool andCombine(IEnumerable<string> values, string[] search)
		{
			foreach(var searchString in search)
			{
				if (values.Count(x => x.Contains(searchString)) == 0)
					return false;
			}

			return true;
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

		private ValueSelector _valueSelector = new ValueSelector();
		private FieldFilter _fieldFilter = new FieldFilter();
		private SearchConfig _standardConfig = new SearchConfig {
			SearchMode = ESearchMode.And,
			IgnoreFields = new string[0],
			SearchTypes = new Type[] { typeof(string) } 
		};
	}
}
