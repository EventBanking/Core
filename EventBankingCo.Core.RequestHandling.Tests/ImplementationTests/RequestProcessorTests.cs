using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using EventBankingCo.Core.RequestHandling.Implementation;
using EventBankingCo.Core.RequestHandling.Tests.TestHelpers;
using Moq;

namespace EventBankingCo.Core.RequestHandling.Tests.ImplementationTests
{
    public class RequestProcessorTests
    {
        private readonly Mock<IHandlerFactory> _mockHandlerFactory = new();

        private readonly Mock<ICoreLogger> _mockLogger = new();

        private readonly RequestProcessor _processor;

        public RequestProcessorTests()
        {
            _processor = new(_mockHandlerFactory.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Arrange
            IRequest<string>? request = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _processor.ProcessRequestAsync(request!));
        }

        [Fact]
        public async Task ProcessAsync_ShouldLogError_WhenRequestIsNull()
        {
            // Arrange
            IRequest<string>? request = null;

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _processor.ProcessRequestAsync(request!));

            _mockLogger.Verify(logger => logger.LogError(exception.Message, exception, null, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ProcessAsync_ShouldProcessRequest_WhenHandlerExists()
        {
            // Arrange
            var request = new TestRequest();
            var handler = new TestHandler();

            _mockHandlerFactory.Setup(hf => hf.CreateHandler<IRequest<string>, string>(request)).Returns(handler);

            // Act
            var result = await _processor.ProcessRequestAsync(request);

            // Assert
            Assert.Equal(request.ResultValue, result);
        }

        [Fact]
        public async Task ProcessAsync_ShouldThrowInvalidOperationException_WhenNoHandlerFound()
        {
            // Arrange
            var request = new TestRequest();

            _mockHandlerFactory.Setup(hf => hf.CreateHandler<IRequest<string>, string>(request)).Returns((TestHandler?)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _processor.ProcessRequestAsync(request));

            _mockLogger.Verify(logger => logger.LogError(exception.Message, exception, null, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
