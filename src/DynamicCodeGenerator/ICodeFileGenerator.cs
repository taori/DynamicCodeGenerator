using System.Reflection;

namespace DynamicCodeGenerator
{
	public interface ICodeFileGenerator
	{
		/// <summary>
		/// Generators will be executed in a descending order.
		/// </summary>
		/// <returns></returns>
		int GetPriority();

		/// <summary>
		/// This method enables you to create files with an assembly generated from the current project.
		/// </summary>
		/// <param name="assembly">The assembly you are receiving here is an intermediate assembly which represents the current project.</param>
		/// <param name="manager">manager class to create files with.</param>
		void Process(Assembly assembly, ICodeFileManager manager);
	}
}