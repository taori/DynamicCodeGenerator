using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DynamicCodeGenerator;

namespace ProductionTest
{
	public class TestGenerator : ICodeFileGenerator
	{
		public int GetPriority()
		{
			return 100;
		}

		public void Process(Assembly assembly, ICodeFileManager manager)
		{
			var path = manager.AddFile("GeneratedClass1.cs");

			var typeNames = string.Join(",", assembly.ExportedTypes.Select(s => s.FullName));
			File.WriteAllText(path, string.Format(@"
// {0}
public class GeneratedClass1 {{

}}
", typeNames));
		}
	}
}
