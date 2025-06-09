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

        public RequestProcessorIntegrationTests()
        {
            var coreLoggingFactoryStub = new CoreLoggingFactoryStub();

            var handlerDictionary = HandlerDictionary.FromAssemblyOf<TestRequest>();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ICoreLoggerFactory, CoreLoggingFactoryStub>();

            serviceCollection.AddRequestHandling(handlerDictionary);

            var typeInstantiator = new TypeInstantiator(serviceCollection.BuildServiceProvider(), coreLoggingFactoryStub);

            var handlerFactory = new HandlerFactory(typeInstantiator, handlerDictionary, coreLoggingFactoryStub);

            _requestProcessor = new RequestProcessor(handlerFactory, coreLoggingFactoryStub);
        }

        [Fact]
        public async Task ProcessRequest_ShouldReturnExpectedResult_ForValidRequest()
        {
            // Arrange
            var request = new TestRequest();

            // Act
            var result = await _requestProcessor.GetResponseAsync(request);

            // Assert
            Assert.Equal(request.ResultValue, result);
        }

        [Fact]
        public async Task ProcessRequest_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Arrange
            TestRequest? request = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _requestProcessor.GetResponseAsync(request!));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task ProcessRequest_ShouldKeyNotFoundException_WhenNoHandlerFound()
        {
            Mock<IRequest<string>> mockRequest = new();

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _requestProcessor.GetResponseAsync(mockRequest.Object));

            Assert.Contains("No handler found for request type", exception.Message);
        }

        [Fact]
        public async Task ProcessCommand_ShouldThrowArgumentNullException_WhenCommandIsNull()
        {
            // Arrange
            ICommand? command = null;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _requestProcessor.ExecuteCommandAsync(command!, CancellationToken.None));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task ProcessCommand_ShouldThrowKeyNotFoundException_WhenNoHandlerFound()
        {
            // Arrange
            Mock<ICommand> mockCommand = new();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _requestProcessor.ExecuteCommandAsync(mockCommand.Object, CancellationToken.None));

            Assert.Contains("No handler found for request type", exception.Message);
        }

        [Fact]
        public async Task ProcessCommand_ShouldExecuteSuccessfully_WhenValidCommandIsProvided()
        {
            // Arrange
            var command = new TestCommand();

            // Act
            var exception = await Record.ExceptionAsync(async () => await _requestProcessor.ExecuteCommandAsync(command, CancellationToken.None));

            // Assert
            Assert.Null(exception);
        }
    }
}
