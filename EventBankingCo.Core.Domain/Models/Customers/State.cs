namespace EventBankingCo.Core.Domain.Models.Customers
{
    public class State
    {
        public State()
        {
            
        }

        public State(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public string Name { get; set; } = string.Empty;

        public string Abbreviation { get; set; } = string.Empty;
    }
}
