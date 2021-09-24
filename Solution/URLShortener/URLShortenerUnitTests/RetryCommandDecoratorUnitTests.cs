using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using UrlShortenerDomainLayer.Interfaces;
using UrlShortenerDomainLayer.Decorators.Command;

namespace UrlShortenerUnitTests
{
    public class RetryCommandDecoratorUnitTests
    {
        [Fact]
        public void Test_ReturnsCorrectValueAfterFewErrors()
        {
            // Arrange
            int expected = 5;
            var queryMock = new Mock<ICommand<int, int>>();
            queryMock.SetupSequence(x => x.Execute(It.IsAny<int>()))
                .Throws(new ArgumentException())
                .Throws(new ArgumentException())
                .Throws(new ArgumentException())
                .Returns(expected);

            var sut = new RetryCommandDecorator<int, int>(queryMock.Object);

            // Act
            var actual = sut.Execute(0);

            // Assert
            Assert.Equal(expected, actual);
            queryMock.Verify(x => x.Execute(It.IsAny<int>()), Times.Exactly(4));
        }

        [Fact]
        public void Test_ThrowsAfterRetriesOnErrors()
        {
            // Arrange
            var queryMock = new Mock<ICommand<int, int>>();
            queryMock.SetupSequence(x => x.Execute(It.IsAny<int>()))
                .Throws(new ArgumentException())
                .Throws(new ArgumentException())
                .Throws(new ArgumentException())
                .Throws(new ArgumentException())
                .Throws(new ArgumentException())
                .Throws(new ArgumentException());

            var sut = new RetryCommandDecorator<int, int>(queryMock.Object);

            // Act
            Assert.Throws<ArgumentException>(() => sut.Execute(0));
            queryMock.Verify(x => x.Execute(It.IsAny<int>()), Times.Exactly(5));
        }

        [Fact]
        public void Test_ReturnsCorrectValueOnNoErrors()
        {
            // Arrange
            int expected = 5;
            var queryMock = new Mock<ICommand<int, int>>();
            queryMock.SetupSequence(x => x.Execute(It.IsAny<int>()))
                .Returns(expected);

            var sut = new RetryCommandDecorator<int, int>(queryMock.Object);

            // Act
            var actual = sut.Execute(0);

            // Assert
            Assert.Equal(expected, actual);
            queryMock.Verify(x => x.Execute(It.IsAny<int>()), Times.Exactly(1));
        }
    }
}
