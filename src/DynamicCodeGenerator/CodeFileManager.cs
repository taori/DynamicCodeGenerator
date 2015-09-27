using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DynamicCodeGenerator
{
	public class CodeFileManager : ICodeFileManager
	{
		private readonly string _initialDirectory;

		public CodeFileManager(string initialDirectory)
		{
			if(string.IsNullOrEmpty(initialDirectory))
				throw new ArgumentException(nameof(initialDirectory), nameof(initialDirectory));

			RootSplits = initialDirectory.Split(new [] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			_initialDirectory = initialDirectory;
		}

		private string[] RootSplits { get; }

		public List<string> AbsolutePaths { get; } = new List<string>();

		/// <inheritdoc />
		public string AddFile(string relativePath)
		{
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentException(nameof(relativePath), nameof(relativePath));
			
			var pathSplits = relativePath.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
			var newPathSplits = new Stack<string>();
			foreach (var split in RootSplits)
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

			if(newPathSplits.Count == 0)
				return String.Empty;

			var resolved = string.Join(Path.DirectorySeparatorChar.ToString(), newPathSplits.Reverse().ToArray());

			AbsolutePaths.Add(resolved);
			return resolved;
		}
	}
}