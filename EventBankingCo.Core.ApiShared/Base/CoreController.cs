using EventBankingCo.Core.Domain.Exceptions;
using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBankingCo.Core.ApiShared.Base
{
    public class CoreController
    {
        protected readonly ICoreLogger<CoreController> _logger;

        private readonly IRequestProcessor _requestProcessor;

        public CoreController(ICoreLoggerFactory loggerFactory, IRequestProcessor requestProccessor)
        {
            _logger = loggerFactory.Create(this);

            _requestProcessor = requestProccessor ?? throw new ArgumentNullException(nameof(requestProccessor), "Request Processor cannot be null");
        }

        protected async Task<IActionResult> ExecuteCommand(ICommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _requestProcessor.ExecuteCommandAsync(command, cancellationToken);

                return new OkResult();
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        protected async Task<IActionResult> GetResponseAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _requestProcessor.GetResponseAsync(request, cancellationToken);

                return new ObjectResult(result);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

        private IActionResult HandleException(Exception e)
        {
            switch (e)
            {
                case BadRequestException badRequestException:

                    _logger.LogDebug(badRequestException.Message, badRequestException);
                    
                    return new ObjectResult(badRequestException.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                case NotFoundException notFoundException:

                    _logger.LogDebug(notFoundException.Message, notFoundException);

                    return new ObjectResult(notFoundException.Message)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                
                case AlreadyExistsException alreadyExistsException:

                    _logger.LogDebug(alreadyExistsException.Message, alreadyExistsException);

                    return new ObjectResult(alreadyExistsException.Message)
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                
                case UnexpectedOutcomeException unexpectedOutcomeException:

                    _logger.LogError(unexpectedOutcomeException.Message, unexpectedOutcomeException);

                    return new ObjectResult(unexpectedOutcomeException.Message)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                
                default:

                    _logger.LogError(e.Message, e);

                    return new ObjectResult("An unexpected error occurred")
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
            }
        }
    }
}
