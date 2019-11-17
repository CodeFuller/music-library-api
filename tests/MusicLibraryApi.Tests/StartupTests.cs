using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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

			var configurationStub = new Mock<IConfiguration>();
			configurationStub.Setup(x => x.GetSection(It.IsAny<string>())).Returns(Mock.Of<IConfigurationSection>());

			var target = new Startup(configurationStub.Object);

			// Act

			target.Configure(appStub.Object, Mock.Of<IWebHostEnvironment>(), Mock.Of<ILoggerFactory>());

			// Assert

			Assert.AreSame(loggerStub, ErrorHandlingMiddleware.Logger);
		}
	}
}
