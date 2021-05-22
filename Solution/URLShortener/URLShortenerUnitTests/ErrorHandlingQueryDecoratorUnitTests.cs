using System;
using Xunit;
using Moq;
using URLShortenerDomainLayer.Interfaces;
using URLShortenerDomainLayer.Decorators.Query;
using Microsoft.Extensions.Logging;

namespace URLShortenerUnitTests
{
    public class ErrorHandlingQueryDecoratorUnitTests
    {
        [Fact]
        public void Test_ThrowsOnInnerException()
        {
            // Arrange
            var queryMock = new Mock<IQuery<int, int>>();
            queryMock.Setup(x => x.Execute(It.IsAny<int>())).Throws(new ArgumentException("Test exception"));

            var loggerMock = new Mock<ILogger<ErrorHandlingQueryDecorator<int, int>>>();
            var decorator = new ErrorHandlingQueryDecorator<int, int>(queryMock.Object, loggerMock.Object);
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => decorator.Execute(2));
            queryMock.Verify(x => x.Execute(It.IsAny<int>()), Times.Once);
            loggerMock.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<ArgumentException>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public void Test_ReturnsCorrectValueOnNoException()
        {
            // Arrange
            int expected = 5;
            var commandMock = new Mock<IQuery<int, int>>();
            commandMock.Setup(x => x.Execute(It.IsAny<int>())).Returns(expected);

            var loggerMock = new Mock<ILogger<ErrorHandlingQueryDecorator<int, int>>>();
            var decorator = new ErrorHandlingQueryDecorator<int, int>(commandMock.Object, loggerMock.Object);

            // Act
            var result = decorator.Execute(2);

            // Assert
            Assert.Equal(expected, result);
            commandMock.Verify(x => x.Execute(It.IsAny<int>()), Times.Once);
            loggerMock.Verify(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<ArgumentException>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Never);
        }
    }
}
