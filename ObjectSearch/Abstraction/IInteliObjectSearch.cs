using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectSearch.Abstraction
{
	public interface IInteliObjectSearch
	{
		IEnumerable<TObj> Search<TObj>(IEnumerable<TObj> data, string search);
	}
}
