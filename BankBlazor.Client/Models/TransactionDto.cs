namespace BankBlazor.Client.Models
{
    public class TransactionDto
    {
        public DateTime Date { get; set; }
        public string Operation { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
