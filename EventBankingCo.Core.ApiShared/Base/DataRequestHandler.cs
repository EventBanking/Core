using EventBankingCo.Core.DataAccess.Abstraction;
using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using EventBankingCo.Core.RequestHandling.Base;

namespace EventBankingCo.Core.ApiShared.Base
{
    public abstract class DataRequestHandler<TRequest, TResponse> : RequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly IDataAccess _dataAccess;

        protected DataRequestHandler(ICoreLoggerFactory loggerFactory, IDataAccess dataAccess) : base(loggerFactory)
        {
            _dataAccess = dataAccess;
        }
    }
}
