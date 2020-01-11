using System;
using System.IO;
using System.Reflection;

namespace MusicLibraryApi.IntegrationTests.Utility
{
	public static class Paths
	{
		private static string GetCurrentDirectory()
		{
			var currentAssembly = Assembly.GetExecutingAssembly().Location;
			return Path.GetDirectoryName(currentAssembly) ?? throw new InvalidOperationException("Failed to get current directory");
		}

		public static string GetTestDataDirectory()
		{
			return Path.Combine(GetCurrentDirectory(), "TestData");
		}

		public static string GetTestRunSettingsPath()
		{
			return Path.Combine(GetCurrentDirectory(), "TestRunSettings.json");
		}

		public static string GetTestDataFilePath(string fileName)
		{
			return Path.Combine(GetTestDataDirectory(), fileName);
		}
	}
}
