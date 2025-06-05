namespace EventBankingCo.Core.Logging.Configurations
{
    public class DatabaseSinkOptions
    {
        public bool Enabled { get; set; } = false;
        public string MinLevel { get; set; } = "Error";
        public string ConnectionString { get; set; } = string.Empty;
    }

}
