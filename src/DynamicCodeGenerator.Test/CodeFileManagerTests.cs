using System;
using NUnit.Framework;

namespace DynamicCodeGenerator.Test
{
	[TestFixture("D:\\folder1\\folder2\\")]
	[TestFixture("D:\\folder1\\folder2")]
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

		public void TestRelativeResolution(string relativePath, string expectedResolutionPath)
		{
			var manager = new CodeFileManager(_rootPath);
			var path = manager.AddFile(relativePath);
			Assert.That(manager.AbsolutePaths.Count, Is.EqualTo(1));
			Assert.That(path, Is.EqualTo(expectedResolutionPath));
		}

		[Test]
		public void VerifyAddFileThrow()
		{
			var manager = new CodeFileManager(_rootPath);
			Assert.Throws<ArgumentException>(() => manager.AddFile(null));
			Assert.Throws<ArgumentException>(() => manager.AddFile(string.Empty));
		}

		[Test]
		public void VerifyConstructorThrow()
		{
			Assert.Throws<ArgumentException>(() => new CodeFileManager(null));
			Assert.Throws<ArgumentException>(() => new CodeFileManager(string.Empty));
		}
	}
}