using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using URLShortenerDomainLayer.Interfaces;
using URLShortenerDomainLayer.Decorators.Query;

namespace URLShortenerUnitTests
{
    public class CachingQueryDecoratorUnitTests
    {
        [Fact]
        public void Test_ReturnsValueFromCache()
        {
            // Arrange
            string key = "test_key";
            int expected = 5;
            int param = 2;
            var queryMock = new Mock<IQuery<int, int>>();
            queryMock.Setup(x => x.Execute(It.IsAny<int>())).Returns(expected);

            var cacheServiceMock = new Mock<ICache>();
            cacheServiceMock.Setup(x => x.GetKey(param)).Returns("test_key");
            cacheServiceMock.Setup(x => x.Has(key)).Returns(true);
            cacheServiceMock.Setup(x => x.Get<int>(key)).Returns(expected);

            var sut = new CachingQueryDecorator<int, int>
                (queryMock.Object, cacheServiceMock.Object);

            // Act
            var actual = sut.Execute(2);

            // Assert
            Assert.Equal(expected, actual);
            queryMock.Verify(x => x.Execute(It.IsAny<int>()), Times.Never);
            cacheServiceMock.Verify(x => x.GetKey(It.IsAny<int>()), Times.Once);
            cacheServiceMock.Verify(x => x.Has(key), Times.Once);
            cacheServiceMock.Verify(x => x.Get<int>(key), Times.Once);
            cacheServiceMock.Verify(x => x.Set<int>(
                It.IsAny<string>(), 
                It.IsAny<int>()
                ), Times.Never);
        }

        [Fact]
        public void Test_ReturnsCorrectValueOnCacheMiss()
        {
            // Arrange
            string key = "test_key";
            int expected = 5;
            int param = 2;
            var queryMock = new Mock<IQuery<int, int>>();
            queryMock.Setup(x => x.Execute(It.IsAny<int>())).Returns(expected);

            var cacheServiceMock = new Mock<ICache>();
            cacheServiceMock.Setup(x => x.GetKey(param)).Returns("test_key");
            cacheServiceMock.Setup(x => x.Has(key)).Returns(false);

            var sut = new CachingQueryDecorator<int, int>
                (queryMock.Object, cacheServiceMock.Object);

            // Act
            var actual = sut.Execute(2);

            // Assert
            Assert.Equal(expected, actual);
            queryMock.Verify(x => x.Execute(It.IsAny<int>()), Times.Once);
            cacheServiceMock.Verify(x => x.GetKey(It.IsAny<int>()), Times.Once);
            cacheServiceMock.Verify(x => x.Has(key), Times.Once);
            cacheServiceMock.Verify(x => x.Get<int>(key), Times.Never);
            cacheServiceMock.Verify(x => x.Set<int>(key, expected), Times.Once);
        }
    }
}
