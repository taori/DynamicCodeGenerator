using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynamicCodeGenerator.Helpers;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace DynamicCodeGenerator
{
    public class DynamicFileGeneratorTask : ITask
    {
		[Output]
	    public string GeneratedFilePaths { get; set; }

	    public bool Execute()
	    {
		    var taskErrorState = false;

		    try
			{
				var ws = MSBuildWorkspace.Create();
				var project = Task.Run(() => ws.OpenProjectAsync(BuildEngine.ProjectFileOfTaskNode)).Result;
				Compilation compilation;

				try
				{
					compilation = Task.Run(() => project.GetCompilationAsync()).Result;
				}
				catch (Exception ex)
				{
					BuildEngine.LogError($"Compilation process initiated by {nameof(DynamicFileGeneratorTask)} failed.");
					BuildEngine.LogError(ex.Message);
					return taskErrorState;
				}

				Assembly assembly = null;
				if (!GenerateIntermediateAssembly(compilation, ref assembly))
					return taskErrorState;

				if (assembly != null)
				{
					var codeFileManager = new CodeFileManager(Path.GetDirectoryName(BuildEngine.ProjectFileOfTaskNode));
					var userGeneratorTypes = assembly.ExportedTypes.Where(d => typeof (ICodeFileGenerator).IsAssignableFrom(d));
					var generators = CreateGenerators(userGeneratorTypes);
					ExecuteGenerators(generators, assembly, codeFileManager);

					GeneratedFilePaths = string.Join(",", codeFileManager.AbsolutePaths);
				}
			}
		    catch (Exception ex)
			{
				BuildEngine.LogError($"An exception occured within {nameof(DynamicFileGeneratorTask)}.");
				BuildEngine.LogError(ex.Message);

				return taskErrorState;
			}

		    return true;
	    }

	    private bool GenerateIntermediateAssembly(Compilation compilation, ref Assembly assembly)
	    {
		    using (var stream = new MemoryStream())
		    {
			    var emission = compilation.Emit(stream);
			    if (!emission.Success)
			    {
				    foreach (var diagnostic in emission.Diagnostics)
				    {
					    if (diagnostic.Severity == DiagnosticSeverity.Error)
						    BuildEngine.LogError(diagnostic.GetMessage(Thread.CurrentThread.CurrentUICulture));
				    }
				    return false;
			    }

			    stream.Position = 0;
			    assembly = Assembly.Load(stream.GetBuffer());
		    }

		    return true;
	    }

	    private static void ExecuteGenerators(List<ICodeFileGenerator> generators, Assembly assembly, CodeFileManager codeFileManager)
	    {
		    foreach (var generator in generators.OrderByDescending(d => d.GetPriority()))
		    {
			    generator.Process(assembly, codeFileManager);
		    }
	    }

	    private static List<ICodeFileGenerator> CreateGenerators(IEnumerable<Type> types)
	    {
		    var generators = new List<ICodeFileGenerator>();
			
		    foreach (var type in types)
		    {
			    var generator = Activator.CreateInstance(type) as ICodeFileGenerator;
			    if (generator != null)
			    {
				    generators.Add(generator);
			    }
		    }

		    return generators;
	    }

	    public IBuildEngine BuildEngine { get; set; }
	    public ITaskHost HostObject { get; set; }
    }
}
