using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DynamicCodeGenerator;

namespace NugetInstallTestProject
{
    public enum SomeEnum
    {
		A,
		B,
		C
    }

	public class TestGeneragor : ICodeFileGenerator
	{
		public int GetPriority()
		{
			return 0;
		}

		public void Process(Assembly assembly, ICodeFileManager manager)
		{
			var txtPath = manager.AddFile("test.txt", CodeFileType.Content);
			File.WriteAllText(txtPath, "just some content "+ string.Join(", ", assembly.GetTypes().Select(s => s.FullName)));
			var csPath = manager.AddFile("testCompilation.cs", CodeFileType.Compilation);
			File.WriteAllText(csPath, @"
public enum SomeOtherEnum
    {
		A,
		B,
		C
    }");
		}
	}
}
