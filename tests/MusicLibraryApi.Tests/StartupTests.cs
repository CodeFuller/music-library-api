using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MusicLibraryApi.GraphQL;

namespace MusicLibraryApi.Tests
{
	[TestClass]
	public class StartupTests
	{
		[TestMethod]
		public void Configure_SetsLoggerForErrorHandlingMiddleware()
		{
			// Arrange

			var loggerStub = Mock.Of<ILogger>();

			var serviceProviderStub = new Mock<IServiceProvider>();
			serviceProviderStub.Setup(x => x.GetService(typeof(ILogger))).Returns(loggerStub);

			var appStub = new Mock<IApplicationBuilder>();
			appStub.Setup(x => x.ApplicationServices).Returns(serviceProviderStub.Object);

			// Act

			Startup.Configure(appStub.Object, Mock.Of<IWebHostEnvironment>());

			// Assert

			Assert.AreSame(loggerStub, ErrorHandlingMiddleware.Logger);
		}
	}
}
