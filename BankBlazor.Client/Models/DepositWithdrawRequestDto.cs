using System.ComponentModel.DataAnnotations;

namespace BankBlazor.Client.Models
{
    public class DepositWithdrawRequestDto
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("deposit|withdraw", ErrorMessage = "Must be 'deposit' or 'withdraw'")]
        public string Operation { get; set; }

        public string? Symbol { get; set; }
    }
}
