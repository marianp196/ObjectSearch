using ObjectSearch.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectSearch.ObjectSearch
{
	public class ObjectSearch : IObjectSearch
	{
		public IEnumerable<SearchResult<TObj>> Search<TObj>(IEnumerable<TObj> data, string[] searchStrings, SearchConfig config = null)
		{
			var conf = getConfig(config);



			return null;
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

		private SearchConfig _standardConfig = new SearchConfig { SearchMode = ESearchMode.And,
			IgnoreFields = new string[0],
			SearchTypes = new Type[] { typeof(string) } };
		}
	}
}
