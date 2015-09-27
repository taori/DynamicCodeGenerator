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
			var compilationPath = manager.AddFile("GeneratedClass1.cs", CodeFileType.Compilation);
			var typeNames = string.Join(",", assembly.ExportedTypes.Select(s => s.FullName));

			File.WriteAllText(compilationPath, string.Format(@"
// {0}
public class GeneratedClass1 {{

}}
", typeNames));

			var contentPath = manager.AddFile("GeneratedContent1.txt", CodeFileType.Content);
			
			File.WriteAllText(contentPath, string.Format(@"{0} {1}", typeNames, DateTime.Now.ToString("F")));
		}
	}
}
