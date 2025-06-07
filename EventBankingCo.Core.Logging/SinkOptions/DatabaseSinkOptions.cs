using Serilog;

namespace EventBankingCo.Core.Logging.Configurations
{
    public class DatabaseSinkOptions : BaseSinkOption
    {
        private readonly string _connectionString;

        private readonly string _tableName;

        private readonly int _batchPostingLimit;

        private readonly TimeSpan _batchPeriod;

        public DatabaseSinkOptions(string connectionString, string tableName = "Logs", string minLevel = "Information", bool enabled = true, int batchPostingLimit = 1000, int secondsBetweenBatches = 5) : base(minLevel, enabled)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString :
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");

            _tableName = !string.IsNullOrWhiteSpace(tableName) ? tableName :
                throw new ArgumentNullException(nameof(tableName), "Table name cannot be null or empty.");

            _batchPostingLimit = batchPostingLimit > 0 ? batchPostingLimit :
                throw new ArgumentOutOfRangeException(nameof(batchPostingLimit), "Batch posting limit must be greater than zero.");

            _batchPeriod = secondsBetweenBatches > 0 ? TimeSpan.FromSeconds(secondsBetweenBatches) :
                throw new ArgumentOutOfRangeException(nameof(secondsBetweenBatches), "Seconds between batches must be greater than zero.");
        }

        public override LoggerConfiguration ConfigureSink(LoggerConfiguration loggerConfig) =>
            loggerConfig.WriteTo.MSSqlServer(_connectionString,
                sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
                {
                    TableName = _tableName,
                    AutoCreateSqlTable = true,
                    BatchPostingLimit = _batchPostingLimit,
                    BatchPeriod = _batchPeriod
                }
            );
    }
}
