using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;

namespace EventBankingCo.Core.ApiShared.Base
{
    public class CoreController
    {
        protected readonly ICoreLogger _logger;

        protected readonly IRequestProcessor _requestProcessor;

        public CoreController(ICoreLogger logger, IRequestProcessor requestProccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");

            _requestProcessor = requestProccessor ?? throw new ArgumentNullException(nameof(requestProccessor), "Request Processor cannot be null");
        }
    }
}
