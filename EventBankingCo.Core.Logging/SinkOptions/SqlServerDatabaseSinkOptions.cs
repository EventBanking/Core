using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace EventBankingCo.Core.Logging.Configurations
{
    public class SqlServerDatabaseSinkOptions : BaseSinkOption
    {
        private readonly string _connectionString;

        private readonly string _tableName;

        private readonly int _batchPostingLimit;

        private readonly TimeSpan _batchPeriod;

        private readonly List<SqlColumn> _additionalColumns;

        public SqlServerDatabaseSinkOptions(string connectionString, string tableName = "Logs", string minLevel = "Information", bool enabled = true, int batchPostingLimit = 1000, int secondsBetweenBatches = 5, List<SqlColumn>? additionalColumns = null) : base(minLevel, enabled)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString :
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");

            _tableName = !string.IsNullOrWhiteSpace(tableName) ? tableName :
                throw new ArgumentNullException(nameof(tableName), "Table name cannot be null or empty.");

            _batchPostingLimit = batchPostingLimit > 0 ? batchPostingLimit :
                throw new ArgumentOutOfRangeException(nameof(batchPostingLimit), "Batch posting limit must be greater than zero.");

            _batchPeriod = secondsBetweenBatches > 0 ? TimeSpan.FromSeconds(secondsBetweenBatches) :
                throw new ArgumentOutOfRangeException(nameof(secondsBetweenBatches), "Seconds between batches must be greater than zero.");

            _additionalColumns = new List<SqlColumn>()
            {
                new SqlColumn("TraceId",SqlDbType.NVarChar),
                new SqlColumn("MethodName", SqlDbType.NVarChar),
                new SqlColumn("CorrelationId", SqlDbType.NVarChar),
                new SqlColumn("ExceptionType", SqlDbType.NVarChar),
                new SqlColumn("ExceptionMessage", SqlDbType.NVarChar),
                new SqlColumn("StackTrace", SqlDbType.NVarChar),
                new SqlColumn("ServiceName", SqlDbType.NVarChar),
                new SqlColumn("SourceContext", SqlDbType.NVarChar),
            };

            if (additionalColumns != null && additionalColumns.Count > 0)
            {
                _additionalColumns.AddRange(additionalColumns);
            }
        }

        public override LoggerConfiguration ConfigureSink(LoggerConfiguration loggerConfig) =>
            loggerConfig.WriteTo.MSSqlServer(_connectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = _tableName,
                    AutoCreateSqlTable = true,
                    BatchPostingLimit = _batchPostingLimit,
                    BatchPeriod = _batchPeriod,                    
                },
                columnOptions: new ColumnOptions()
                {
                    AdditionalColumns = _additionalColumns,
                });
    }
}
