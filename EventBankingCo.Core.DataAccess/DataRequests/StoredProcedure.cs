using EventBankingCo.Core.DataAccess.Abstraction;
using System.Data;

namespace EventBankingCo.Core.DataAccess.DataRequests
{
    public abstract class StoredProcedure<TResponse> : IDataRequest<TResponse>
    {
        public CommandType GetCommandType() => CommandType.StoredProcedure;

        public abstract object? GetParameters();

        public abstract string GetSql();
    }
}
