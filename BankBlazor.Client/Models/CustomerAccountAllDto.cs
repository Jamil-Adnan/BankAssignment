namespace BankBlazor.Client.Models
{
    public class CustomerAccountAllDto
    {
        public string FullName { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
    }

    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
