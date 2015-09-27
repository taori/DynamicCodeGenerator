using System;
using Microsoft.Build.Framework;

namespace DynamicCodeGenerator.Helpers
{
	public static class BuildEngineExtensions
	{
		public static void LogError(this IBuildEngine source, string msg)
		{
			source.LogErrorEvent(new BuildErrorEventArgs(null, null, null, 0, 0, 0, 0, message: msg, helpKeyword: null, senderName: "DynamicFileGeneratorTask"));
		}

		public static void LogMessage(this IBuildEngine source, string message, MessageImportance importance)
		{
			source.LogMessageEvent(new BuildMessageEventArgs("", null, null, importance, DateTime.Now));
		}
	}
}