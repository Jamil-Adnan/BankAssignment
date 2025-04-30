using System.ComponentModel.DataAnnotations;

namespace BankBlazor.API.DTO
{
    public class DepositWithdrawRequestDto
    {
        [Required]
        [Display(Name = "Account Number")]
        public int AccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be positive")]
        public decimal Amount { get; set; }

        [Required]
        [RegularExpression("deposit|withdraw", ErrorMessage = "Use 'deposit' or 'withdraw'")]
        public string Operation { get; set; }

        public string? Symbol { get; set; } // optional label
    }
}
