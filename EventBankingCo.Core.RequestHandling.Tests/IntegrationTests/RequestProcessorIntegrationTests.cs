using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using EventBankingCo.Core.RequestHandling.Implementation;
using EventBankingCo.Core.RequestHandling.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EventBankingCo.Core.RequestHandling.Tests.IntegrationTests
{
    public class RequestProcessorIntegrationTests
    {
        private readonly RequestProcessor _requestProcessor;

        private readonly Mock<ICoreLogger> _mockLogger = new();

        public RequestProcessorIntegrationTests()
        {
            var handlerDictionary = HandlerDictionary.FromAssemblyOf<TestRequest>();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(_mockLogger.Object);

            serviceCollection.AddRequestHandling(handlerDictionary);

            var typeInstantiator = new TypeInstantiator(serviceCollection.BuildServiceProvider(), _mockLogger.Object);

            var handlerFactory = new HandlerFactory(typeInstantiator, handlerDictionary, _mockLogger.Object);

            _requestProcessor = new RequestProcessor(handlerFactory, _mockLogger.Object);
        }

        [Fact]
        public async Task ProcessRequest_ShouldReturnExpectedResult_ForValidRequest()
        {
            // Arrange
            var request = new TestRequest();

            // Act
            var result = await _requestProcessor.ProcessRequestAsync(request);

            // Assert
            Assert.Equal(request.ResultValue, result);
        }

        [Fact]
        public async Task ProcessRequest_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Arrange
            TestRequest? request = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _requestProcessor.ProcessRequestAsync(request!));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task ProcessRequest_ShouldKeyNotFoundException_WhenNoHandlerFound()
        {
            Mock<IRequest<string>> mockRequest = new();

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _requestProcessor.ProcessRequestAsync(mockRequest.Object));

            Assert.Contains("No handler found for request type", exception.Message);
        }

        [Fact]
        public async Task ProcessCommand_ShouldThrowArgumentNullException_WhenCommandIsNull()
        {
            // Arrange
            ICommand? command = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _requestProcessor.ProcessCommandAsync(command!, CancellationToken.None));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task ProcessCommand_ShouldThrowKeyNotFoundException_WhenNoHandlerFound()
        {
            // Arrange
            Mock<ICommand> mockCommand = new();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _requestProcessor.ProcessCommandAsync(mockCommand.Object, CancellationToken.None));

            Assert.Contains("No handler found for request type", exception.Message);
        }

        [Fact]
        public async Task ProcessCommand_ShouldExecuteSuccessfully_WhenValidCommandIsProvided()
        {
            // Arrange
            var command = new TestCommand();

            // Act
            var exception = await Record.ExceptionAsync(async () => await _requestProcessor.ProcessCommandAsync(command, CancellationToken.None));

            // Assert
            Assert.Null(exception);
        }
    }
}
