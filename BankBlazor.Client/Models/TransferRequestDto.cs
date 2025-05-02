using System.ComponentModel.DataAnnotations;

namespace BankBlazor.Client.Models
{
    public class TransferRequestDto
    {
        public int SenderAccountId { get; set; }
        public string ReceiverBank { get; set; } = string.Empty;
        public int ReceiverAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
