using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectSearch.Abstraction
{
	public interface IObjectSearch
	{
		IEnumerable<SearchResult<TObj>> Search<TObj>(IEnumerable<TObj> data, 
													 string[] searchStrings, SearchConfig config);		
	}

	public class SearchConfig
	{
		public ESearchMode SearchMode { get; set; }
		public Type[] SearchTypes { get; set; }
		public string[] IgnoreFields { get; set; }
	}

	public enum ESearchMode
	{
		And, Or
	}

	public class SearchResult<TObj>
	{
		public int Importance { get; set; }
		public TObj Object { get; set; }
	}
}
