using EventBankingCo.Core.RequestHandling.Abstraction;
using EventBankingCo.Core.RequestHandling.Implementation;
using EventBankingCo.Core.RequestHandling.Tests.TestHelpers;
using Moq;

namespace EventBankingCo.Core.RequestHandling.Tests.ImplementationTests
{
    public class RequestProcessorTests
    {
        private readonly Mock<IHandlerFactory> _mockHandlerFactory = new();

        private readonly CoreLoggingFactoryStub _coreLoggingFactoryStub = new();

        private readonly RequestProcessor _processor;

        public RequestProcessorTests()
        {
            _processor = new(_mockHandlerFactory.Object, _coreLoggingFactoryStub);
        }

        [Fact]
        public async Task ProcessAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Arrange
            IRequest<string>? request = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _processor.GetResponseAsync(request!));
        }

        [Fact]
        public async Task ProcessAsync_ShouldProcessRequest_WhenHandlerExists()
        {
            // Arrange
            var request = new TestRequest();
            var handler = new TestHandler(new CoreLoggingFactoryStub());

            _mockHandlerFactory.Setup(hf => hf.CreateHandler(It.Is<object>(_ => _.GetType() == typeof(TestRequest)))).Returns(handler);

            // Act
            var result = await _processor.GetResponseAsync(request);

            // Assert
            Assert.Equal(request.ResultValue, result);
        }
    }
}
