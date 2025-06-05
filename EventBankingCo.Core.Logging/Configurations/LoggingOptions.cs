namespace EventBankingCo.Core.Logging.Configurations
{
    public class LoggingOptions
    {
        public ConsoleSinkOptions Console { get; set; } = new();

        public FileSinkOptions File { get; set; } = new();
        
        public DatabaseSinkOptions Database { get; set; } = new();
    }
}
