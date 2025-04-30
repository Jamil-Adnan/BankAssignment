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

        [HttpPost("process")]
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
    }

}
