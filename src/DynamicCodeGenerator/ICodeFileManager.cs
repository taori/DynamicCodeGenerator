namespace DynamicCodeGenerator
{
	public interface ICodeFileManager
	{
		/// <summary>
		/// Calling this method will add a virtual reference to compile your project with.
		/// </summary>
		/// <param name="assembly">The assembly you are receiving here is an intermediate assembly which represents the current project.</param>
		/// <param name="relativePath">
		/// <para>this parameter expects a path along the lines of:</para>
		/// <para>file.cs</para>
		/// <para>../parentsiblingfolder/file.cs</para>
		/// <para>subfolder/file.cs</para>
		/// </param>
		/// <param name="type">the type of file you want to register for compilation</param>
		/// <returns></returns>
		string AddFile(string relativePath, CodeFileType type);

		/// <summary>
		/// This method enables you to do custom processing of information obtainable through your project file.
		/// </summary>
		/// <returns>returns the absolute path to the project file this CodeFileManager is issueing.</returns>
		string GetProjectFilePath();
	}
}