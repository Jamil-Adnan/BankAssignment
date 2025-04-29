namespace BankBlazor.API.DTO
{
    public class CustomerAccountInfoDto
    {
        public string FullName { get; set; }
        public List<AccountDto> Accounts { get; set; }
    }

    public class AccountDto
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
    }
}
