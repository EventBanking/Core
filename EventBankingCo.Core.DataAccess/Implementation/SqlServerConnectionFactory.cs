using EventBankingCo.Core.DataAccess.Abstraction;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EventBankingCo.Core.DataAccess.Implementation
{
    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlServerConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection NewConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
