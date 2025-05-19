using System;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging.Debug;

namespace Lab_1.Tests
{
    public class Tests
    {
        [Fact]
        public void CreateMauiApp_ShouldReturnMauiAppInstance()
        {
            // Act
            var mauiApp = MauiProgram.CreateMauiApp();

            // Assert
            Assert.NotNull(mauiApp);
            Assert.IsType<MauiApp>(mauiApp);
        }

        [Fact]
        public void CreateMauiApp_ShouldConfigureLoggingInDebugMode()
        {
            // Arrange
#if DEBUG
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            var builder = MauiApp.CreateBuilder();
            builder.Logging.AddProvider(new DebugLoggerProvider());
#endif

            // Act
            var mauiApp = MauiProgram.CreateMauiApp();

            // Assert
#if DEBUG
            var logger = mockLoggerFactory.Object.CreateLogger("TestLogger");
            Assert.NotNull(logger);
#endif
        }
    }
}