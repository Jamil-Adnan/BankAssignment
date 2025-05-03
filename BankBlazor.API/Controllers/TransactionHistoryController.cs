using BankBlazor.API.Data;
using BankBlazor.API.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly BankBlazorContext _context;

        public TransactionHistoryController(BankBlazorContext context)
        {
            _context = context;
        }

        [HttpGet("{accountId}")]
        public async Task<ActionResult<List<Transaction>>> GetLastFiveTransactions(int accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .Take(5)
                .ToListAsync();

            if (transactions == null || !transactions.Any())
            {
                return NotFound("No transactions found for the specified account.");
            }

            return Ok(transactions);
        }
    }
}
