using EventBankingCo.Core.DataAccess.Abstraction;
using EventBankingCo.Core.Logging.Abstraction;
using System.Data;
using Dapper;

namespace EventBankingCo.Core.DataAccess.Implementation
{
    internal class DataAccess : IDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        private readonly ICoreLogger _logger;

        public DataAccess(IDbConnectionFactory dbConnectionFactory, ICoreLogger logger)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int Execute(IDataExecute request) => 
            HandleRequest(request, dapper => 
                dapper.Execute(request.GetSql(), request.GetParameters(), commandType: request.GetCommandType())
            );

        public TResponse? Fetch<TResponse>(IDataFetch<TResponse> request) =>
            HandleRequest(request, dapper => 
                dapper.QueryFirstOrDefault<TResponse>(request.GetSql(), request.GetParameters(), commandType: request.GetCommandType())
            );

        public IEnumerable<TResponse> FetchList<TResponse>(IDataFetchList<TResponse> request) =>
            HandleRequest(request, dapper => 
                dapper.Query<TResponse>(request.GetSql(), request.GetParameters(), commandType: request.GetCommandType())
            ) ?? [];

        public async Task<int> ExecuteAsync(IDataExecute request) =>
            await HandleRequestAsync(request, async dapper => 
                await dapper.ExecuteAsync(request.GetSql(), request.GetParameters(), commandType: request.GetCommandType())
            );

        public async Task<TResponse?> FetchAsync<TResponse>(IDataFetch<TResponse> request) =>
            await HandleRequestAsync(request, async dapper => 
                await dapper.QueryFirstOrDefaultAsync<TResponse?>(request.GetSql(), request.GetParameters(), commandType: request.GetCommandType())
            );

        public async Task<IEnumerable<TResponse>> FetchListAsync<TResponse>(IDataFetchList<TResponse> request) =>
            await HandleRequestAsync(request, async dapper => 
                await dapper.QueryAsync<TResponse>(request.GetSql(), request.GetParameters())) ?? [];

        private TResponse? HandleRequest<TResponse>(IDataRequest<TResponse> request, Func<IDbConnection, TResponse?> func)
        {
            if (request == null)
            {
                _logger.LogError("NULL request sent to DataAccess.");

                throw new ArgumentNullException(nameof(request));
            }

            var requestName = request.GetType().Name;

            _logger.LogInformation($"Handling IDataRequest: {requestName}.");

            try
            {
                using var connection = _dbConnectionFactory.NewConnection();

                connection.Open();
                
                _logger.LogDebug($"Database Connection Opened. Preparing to send IDataRequest: {requestName}", 
                    new
                    {
                        ConnectionString = connection.ConnectionString,
                        RequestType = requestName,
                        Sql = request.GetSql(),
                        Parameters = request.GetParameters(),
                        CommandType = request.GetCommandType()
                    }
                );

                var result = func.Invoke(connection);

                var responseType = typeof(TResponse);

                _logger.LogInformation("Received IDataRequest Result", responseType.Name);

                _logger.LogDebug("IDataRequest Result Details", new
                {
                    Type = responseType.Name,
                    Value = result
                });

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Caught When Handling IDataRequest", e, new
                {
                    RequestType = requestName,
                    Sql = request.GetSql(),
                    Parameters = request.GetParameters(),
                    CommandType = request.GetCommandType(),
                });

                throw;
            }
        }

        private async Task<TResponse?> HandleRequestAsync<TResponse>(IDataRequest<TResponse> request, Func<IDbConnection, Task<TResponse?>> func)
        {
            if (request == null)
            {
                _logger.LogError("NULL request sent to DataAccess.");

                throw new ArgumentNullException(nameof(request));
            }

            var requestName = request.GetType().Name;

            _logger.LogInformation($"Handling IDataRequest: {requestName}.");

            try
            {
                using var connection = _dbConnectionFactory.NewConnection();

                connection.Open();

                _logger.LogDebug($"Database Connection Opened. Preparing to send IDataRequest: {requestName}",
                    new
                    {
                        ConnectionString = connection.ConnectionString,
                        RequestType = requestName,
                        Sql = request.GetSql(),
                        Parameters = request.GetParameters(),
                        CommandType = request.GetCommandType()
                    }
                );

                var result = await func.Invoke(connection);

                var responseType = typeof(TResponse);

                _logger.LogInformation("Received IDataRequest Result", responseType.Name);

                _logger.LogDebug("IDataRequest Result Details", new
                {
                    Type = responseType.Name,
                    Value = result
                });

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Caught When Handling IDataRequest", e, new
                {
                    RequestType = requestName,
                    Sql = request.GetSql(),
                    Parameters = request.GetParameters(),
                    CommandType = request.GetCommandType(),
                });

                throw;
            }
        }
    }
}
