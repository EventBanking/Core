namespace EventBankingCo.Core.Logging.Configurations
{
    public class ConsoleSinkOptions
    {
        public bool Enabled { get; set; } = true;
        public string MinLevel { get; set; } = "Information";
    }

}
