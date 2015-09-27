using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DynamicCodeGenerator
{
	public class CodeFileManager : ICodeFileManager
	{
		private readonly string _projectFilePath;

		public CodeFileManager(string projectFilePath)
		{
			if(string.IsNullOrEmpty(projectFilePath))
				throw new ArgumentException(nameof(projectFilePath), nameof(projectFilePath));

			_projectFilePath = projectFilePath;

			var directory = Path.GetDirectoryName(projectFilePath);

			ProjectDirectorySplits = directory.Split(new [] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
		}

		private string[] ProjectDirectorySplits { get; }

		/// <summary>
		/// List of absolute paths for compilation files
		/// </summary>
		public List<string> CompilationPaths { get; } = new List<string>();

		/// <summary>
		/// List of absolute paths for conten files
		/// </summary>
		public List<string> ContentPaths { get; } = new List<string>();
		
		public string AddFile(string relativePath, CodeFileType type)
		{
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentException(nameof(relativePath), nameof(relativePath));

			var pathSplits = relativePath.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
			var newPathSplits = new Stack<string>();
			foreach (var split in ProjectDirectorySplits)
			{
				newPathSplits.Push(split);
			}

			foreach (var split in pathSplits)
			{
				if (split == "..")
				{
					newPathSplits.Pop();
				}
				else
				{
					newPathSplits.Push(split);
				}
			}

			if (newPathSplits.Count == 0)
				return String.Empty;

			var resolved = string.Join(Path.DirectorySeparatorChar.ToString(), newPathSplits.Reverse().ToArray());

			switch (type)
			{
				case CodeFileType.Compilation:
					CompilationPaths.Add(resolved);
					break;
				case CodeFileType.Content:
					ContentPaths.Add(resolved);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
			return resolved;
		}

		public string GetProjectFilePath()
		{
			return _projectFilePath;
		}
	}
}