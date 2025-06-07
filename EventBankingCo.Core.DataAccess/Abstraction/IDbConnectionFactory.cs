using System.Data;

namespace EventBankingCo.Core.DataAccess.Abstraction
{
    public interface IDbConnectionFactory
    {
        IDbConnection NewConnection();
    }
}
