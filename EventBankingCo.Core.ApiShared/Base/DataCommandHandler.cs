using EventBankingCo.Core.DataAccess.Abstraction;
using EventBankingCo.Core.Logging.Abstraction;
using EventBankingCo.Core.RequestHandling.Abstraction;
using EventBankingCo.Core.RequestHandling.Base;

namespace EventBankingCo.Core.ApiShared.Base
{
    public abstract class DataCommandHandler<TRequest> : CommandHandler<TRequest> where TRequest : ICommand
    {
        protected readonly IDataAccess _dataAccess;

        protected DataCommandHandler(ICoreLoggerFactory loggerFactory, IDataAccess dataAccess) : base(loggerFactory)
        {
            _dataAccess = dataAccess;
        }
    }
}
