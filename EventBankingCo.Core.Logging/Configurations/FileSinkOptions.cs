namespace EventBankingCo.Core.Logging.Configurations
{
    public class FileSinkOptions
    {
        public bool Enabled { get; set; } = true;
        public string MinLevel { get; set; } = "Information";
        public string Path { get; set; } = "logs/log-.txt";
    }

}
