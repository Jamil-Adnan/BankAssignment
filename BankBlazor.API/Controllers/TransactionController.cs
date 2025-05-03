using BankBlazor.API.Data;
using BankBlazor.API.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankBlazor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly BankBlazorContext _context;

        public TransactionsController(BankBlazorContext context)
        {
            _context = context;
        }

        [HttpPost("deposit_withdraw")]
        public async Task<ActionResult<decimal>> ProcessTransaction(DepositWithdrawRequestDto request)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == request.AccountId);
            if (account == null)
                return NotFound("Account not found");

            // Check withdrawal validity
            if (request.Operation == "withdraw" && account.Balance < request.Amount)
                return BadRequest("Insufficient funds");

            // Update balance
            if (request.Operation == "deposit")
                account.Balance += request.Amount;
            else if (request.Operation == "withdraw")
                account.Balance -= request.Amount;
            else
                return BadRequest("Invalid operation");

            // Save transaction
            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = request.Operation == "deposit" ? "Credit" : "Debit",
                Operation = request.Operation,
                Amount = request.Amount,
                Balance = account.Balance,
                Symbol = request.Symbol ?? ""
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(account.Balance);
        }


        [HttpPost("account_transfer")]
        public async Task<ActionResult<decimal>> TransferMoney(TransferRequestDto request)
        {
            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than 0");

            var sender = await _context.Accounts.FindAsync(request.SenderAccountId);
            if (sender == null) return NotFound("Sender account not found");

            if (sender.Balance < request.Amount)
                return BadRequest("Insufficient funds");

            // Deduct from sender
            sender.Balance -= request.Amount;
            _context.Transactions.Add(new Transaction
            {
                AccountId = sender.AccountId,
                Date = DateOnly.FromDateTime(DateTime.Now),

                Type = "Debit",
                Operation = "transfer",
                Amount = request.Amount,
                Balance = sender.Balance,
                Bank = request.ReceiverBank,
                Account = request.ReceiverAccountId.ToString()
            });

            // Optional: try to credit the receiver if account exists
            var receiver = await _context.Accounts.FindAsync(request.ReceiverAccountId);
            if (receiver != null)
            {
                receiver.Balance += request.Amount;
                _context.Transactions.Add(new Transaction
                {
                    AccountId = receiver.AccountId,
                    Date = DateOnly.FromDateTime(DateTime.Now),

                    Type = "Credit",
                    Operation = "receive",
                    Amount = request.Amount,
                    Balance = receiver.Balance,
                    Bank = "Internal", // or sender bank if needed
                    Account = request.SenderAccountId.ToString()
                });
            }

            await _context.SaveChangesAsync();

            return Ok("Transfer completed successfully."); 
        }

    }
}
