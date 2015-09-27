using System;
using NUnit.Framework;

namespace DynamicCodeGenerator.Test
{
	[TestFixture("D:\\folder1\\folder2\\pseudo.csproj")]
	public class CodeFileManagerTests
	{
		public CodeFileManagerTests(string rootPath)
		{
			_rootPath = rootPath;
		}

		private string _rootPath;

		[TestCase( 
			"..\\..\\file.cs", 
			"D:\\file.cs"
		)]
		[TestCase( 
			"..\\file.cs", 
			"D:\\folder1\\file.cs"
		)]
		[TestCase( 
			"file.cs", 
			"D:\\folder1\\folder2\\file.cs"
		)]
		[TestCase( 
			"folder3\\file.cs", 
			"D:\\folder1\\folder2\\folder3\\file.cs"
		)]
		[TestCase( 
			"..\\folder3\\file.cs", 
			"D:\\folder1\\folder3\\file.cs"
		)]
		[TestCase( 
			"..\\..\\folder3\\file.cs", 
			"D:\\folder3\\file.cs"
		)]
		[TestCase(
			"..\\folder3\\..\\file.cs", 
			"D:\\folder1\\file.cs"
		)]
		[TestCase(
			"..\\folder3\\..\\folder4\\file.cs", 
			"D:\\folder1\\folder4\\file.cs"
		)]
		public void RelativeResolution(string relativePath, string expectedResolutionPath)
		{
			var manager = new CodeFileManager(_rootPath);
			var path = manager.AddFile(relativePath, CodeFileType.Compilation);
			Assert.That(manager.CompilationPaths.Count, Is.EqualTo(1));
			Assert.That(path, Is.EqualTo(expectedResolutionPath));
		}
		
		[Theory]
		public void VerifyAddFileThrow(CodeFileType type)
		{
			var manager = new CodeFileManager(_rootPath);
			Assert.Throws<ArgumentException>(() => manager.AddFile(null, type));
			Assert.Throws<ArgumentException>(() => manager.AddFile(string.Empty, type));
		}

		[Test]
		public void VerifyConstructorThrow()
		{
			Assert.Throws<ArgumentException>(() => new CodeFileManager(null));
			Assert.Throws<ArgumentException>(() => new CodeFileManager(string.Empty));
		}

		[Test]
		public void VerfiyPathRetrieval()
		{
			var manager = new CodeFileManager(_rootPath);
			Assert.That(manager.GetProjectFilePath(), Is.EqualTo(_rootPath));
		}
	}
}