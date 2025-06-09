using EventBankingCo.Core.Domain.Enums;

namespace EventBankingCo.Core.Domain.Models.Customers
{
    public class Customer
    {
        public Customer()
        {
            
        }

        public Customer(string memberId, string firstName, string lastName, string email, string phoneNumber, VerificationStatus verificationStatus)
        {
            MemberId = memberId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            VerificationStatus = verificationStatus;
        }

        public string MemberId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string PhoneNumber { get; set; } = string.Empty;

        public VerificationStatus VerificationStatus { get; set; }
    }
}
