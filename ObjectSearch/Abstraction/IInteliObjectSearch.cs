using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectSearcher.Abstraction
{
	public interface IInteliObjectSearch
	{
		IEnumerable<TObj> Search<TObj>(IEnumerable<TObj> data, string search);
	}
}
