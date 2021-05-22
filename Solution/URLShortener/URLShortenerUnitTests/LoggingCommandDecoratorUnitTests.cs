using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using URLShortenerDomainLayer.Interfaces;
using URLShortenerDomainLayer.Decorators.Command;
using Microsoft.Extensions.Logging;

namespace URLShortenerUnitTests
{
    
    public class LoggingCommandDecoratorUnitTests
    {
        [Fact]
        public void Test_LogsOnExecute()
        {
            // Arrange
            var commandMock = new Mock<ICommand<int, int>>();
            var expected = 5;
            commandMock.Setup(x => x.Execute(It.IsAny<int>())).Returns(expected);

            var loggerMock = new Mock<ILogger<LoggingCommandDecorator<int, int>>>();
            var decorator = new LoggingCommandDecorator<int, int>(commandMock.Object, loggerMock.Object);

            // Act
            var result = decorator.Execute(2);

            // Assert
            Assert.Equal(expected, result);
            commandMock.Verify(x => x.Execute(It.IsAny<int>()), Times.Once);
            // check if there's only one log entry
            loggerMock.Verify(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<ArgumentException>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
            // and that it has LogLevel of Information
            loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<ArgumentException>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
    }
}
