using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using EventBankingCo.Core.RequestHandling.Implementation;
using EventBankingCo.Core.RequestHandling.Tests.TestHelpers;
using Moq;

namespace EventBankingCo.Core.RequestHandling.Tests.ImplementationTests
{
    public class HandlerFactoryTests
    {
        private readonly HandlerFactory _handlerFactory;

        private readonly Mock<IHandlerDictionary> _mockHandlerDictionary = new();

        private readonly Mock<ITypeInstantiator> _mockTypeActivator = new();

        private readonly Mock<ICoreLogger> _mockLogger = new();

        public HandlerFactoryTests()
        {
            _handlerFactory = new(_mockTypeActivator.Object, _mockHandlerDictionary.Object, _mockLogger.Object);
        }

        [Fact]
        public void CreateHandler_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Arrange
            TestRequest? request = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _handlerFactory.CreateHandler<TestRequest, string>(request!));
        }

        [Fact]
        public void CreateHandler_ShouldReturnHandler_WhenRequestIsValid()
        {
            // Arrange
            var request = new TestRequest();
            var handlerType = typeof(TestHandler);

            _mockHandlerDictionary.Setup(h => h.GetHandlerType(It.IsAny<Type>())).Returns(handlerType);
            _mockTypeActivator.Setup(t => t.Instantiate(handlerType)).Returns(new TestHandler());

            // Act
            var handler = _handlerFactory.CreateHandler<TestRequest, string>(request);

            // Assert
            Assert.NotNull(handler);
            Assert.IsType<TestHandler>(handler);
        }

        [Fact]
        public void CreateHandler_ShouldThrowInvalidOperationException_WhenNoHandlerFound()
        {
            // Arrange
            var request = new TestRequest();

            _mockHandlerDictionary.Setup(h => h.GetHandlerType(It.IsAny<Type>())).Returns((Type?)null!);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _handlerFactory.CreateHandler<TestRequest, string>(request));
        }

        [Fact]
        public void CreateHandler_ShouldThrowInvalidOperationException_WhenHandlerCannotBeInstantiated()
        {
            // Arrange
            var request = new TestRequest();
            var handlerType = typeof(TestHandler);

            _mockHandlerDictionary.Setup(h => h.GetHandlerType(It.IsAny<Type>())).Returns(handlerType);
            _mockTypeActivator.Setup(t => t.Instantiate(handlerType)).Returns((object?)null!);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _handlerFactory.CreateHandler<TestRequest, string>(request));
        }
    }
}
