namespace EventBankingCo.Core.Domain.Models.Customers
{
    public class CustomerAddress
    {
        public CustomerAddress()
        {
            
        }

        public CustomerAddress(string memberId, string street1, string street2, string city, State state, string? postalCode)
        {
            MemberId = memberId;
            Street1 = street1;
            Street2 = street2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public string MemberId { get; set; } = string.Empty;

        public string Street1 { get; set; } = string.Empty;

        public string Street2 { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public State State { get; set; } = new();

        public string? PostalCode { get; set; }
    }
}
