using EventBankingCo.Core.DataAccess.Abstraction;
using System.Data;

namespace EventBankingCo.Core.DataAccess.DataRequests
{
    public abstract class SqlCommand : IDataExecute
    {
        public CommandType GetCommandType() => CommandType.Text;

        public abstract object? GetParameters();

        public abstract string GetSql();
    }
}
