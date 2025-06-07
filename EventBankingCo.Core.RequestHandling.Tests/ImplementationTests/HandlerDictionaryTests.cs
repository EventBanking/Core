using EventBankingCo.Core.RequestHandling.Abstraction;
using EventBankingCo.Core.RequestHandling.Implementation;
using EventBankingCo.Core.RequestHandling.Tests.TestHelpers;

namespace EventBankingCo.Core.RequestHandling.Tests.ImplementationTests
{
    public class HandlerDictionaryTests
    {
        private readonly IHandlerDictionary _handlerDictionary;

        public HandlerDictionaryTests()
        {
            _handlerDictionary = HandlerDictionary.FromAssemblyOf<TestRequest>();
        }

        [Fact]
        public void GetHandlerType_ShouldReturnCorrectType_ForTestRequest()
        {
            // Arrange
            var requestType = typeof(TestRequest);

            // Act
            var handlerType = _handlerDictionary.GetHandlerType(requestType);

            // Assert
            Assert.NotNull(handlerType);
            Assert.Equal(typeof(TestHandler), handlerType);
        }

        [Fact]
        public void GetHandlerType_ShouldThrowKeyNotFoundException_WhenNoHandlerExists()
        {
            // Arrange
            var unknownRequestType = typeof(string); // Assuming no handler for string

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => _handlerDictionary.GetHandlerType(unknownRequestType));
        }
    }
}
