using System;

namespace MusicLibraryApi.Client
{
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
	internal class ApiConnectionSettings
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
	{
		public Uri? BaseUrl { get; set; }
	}
}
