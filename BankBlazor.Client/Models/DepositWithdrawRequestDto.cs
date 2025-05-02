using System;
using System.ComponentModel.DataAnnotations;

namespace BankBlazor.Client.Models
{
    public class DepositWithdrawRequestDto
    {
        [Required(ErrorMessage = "Account ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid account number")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Operation type is required")]
        [RegularExpression("Deposit|Withdraw", ErrorMessage = "Operation must be either 'Deposit' or 'Withdraw'")]
        public string Operation { get; set; } = "Deposit";

        [StringLength(50, ErrorMessage = "Symbol cannot exceed 50 characters")]
        public string? Symbol { get; set; }

        [StringLength(50, ErrorMessage = "Bank name cannot exceed 50 characters")]
        public string? Bank { get; set; }

        // For transfers between accounts
        public int? AccountTo { get; set; }

        // Optional description/note for the transaction
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        // Timestamp for when the request was created (set automatically)
        public DateTime RequestDate { get; set; } = DateTime.Now;

        // Optional customer ID for additional verification
        public int? CustomerId { get; set; }
    }
}