using EventBankingCo.Core.DataAccess.Abstraction;
using System.Data;

namespace EventBankingCo.Core.DataAccess.DataRequests
{
    public abstract class SqlFetch<TResponse> : IDataFetch<TResponse>
    {
        public CommandType GetCommandType() => CommandType.Text;

        public abstract object? GetParameters();
        
        public abstract string GetSql();
    }
}
