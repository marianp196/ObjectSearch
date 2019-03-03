using System;
using System.Collections.Generic;
using ObjectSearcher.Abstraction;
using ObjectSearcher.ObjectSearcher;

namespace ObjctSearchTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var sut = new ObjectSearch();

			string[] search = new string[] { "hallo" };
			var data = new List<Test> { new Test() };

			sut.Search<Test>(data, search, new SearchConfig()
			{ SearchTypes = new Type[] { typeof(TestChild), typeof(string) } });
		}
	}

	public class Test
	{
		public string ID { get; set; } = "";
		public string Text { get; set; } = "hallo";
		public TestChild Child { get; set; } =new TestChild();
	}
	public class TestChild
	{
		public string Str { get; set; } = "ottp";
	}
}
