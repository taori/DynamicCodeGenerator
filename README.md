# DynamicCodeGenerator

If you ever had to work with t4 templates, you might have noticed, that the process of creating files based on t4 templates can be tedious and unintuitive. At some point of a task i thought i could solve a feature by generating code once depending on reflection.

While i am sure there are ways to generate code depending on the reflection state of your project file, i found this process of learning a new templating syntax, researching how to create merged/singular files very time consuming and thus decided to create a simple solution for this for the next time i need it.

##How can i use this?

It's fairly simple actually.

```
public class TestGenerator : ICodeFileGenerator
{
		public int GetPriority()
		{
			return 100;
		}

		public void Process(Assembly assembly, ICodeFileManager manager)
		{
			var path = manager.AddFile("GeneratedClass1.cs", CodeFileType.Compilation);

			var typeNames = string.Join(",", assembly.ExportedTypes.Select(s => s.FullName));
			File.WriteAllText(path, "some csharp syntax");
		}
}
```
