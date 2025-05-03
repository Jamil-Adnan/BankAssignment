using BankBlazor.API.Data;
using BankBlazor.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly BankBlazorContext _context;

        public AccountsController(BankBlazorContext context)
        {
            _context = context;
        }

        [HttpGet("{accountId}/customer-info")]
        public async Task<ActionResult<CustomerAccountInfoDto>> GetCustomerInfoByAccountId(int accountId)
        {
            // Check if the AccountId exists in the Accounts table
            var accountExists = await _context.Accounts.AnyAsync(a => a.AccountId == accountId);
            if (!accountExists)
            {
                return NotFound("Account ID does not exist in the Accounts table.");
            }

            // Query the Dispositions table to find the owner
            var disposition = await _context.Dispositions
                .Include(d => d.Customer)
                .Include(d => d.Account)
                .Where(d => d.AccountId == accountId && d.Type == "OWNER")
                .FirstOrDefaultAsync();

            if (disposition == null)
            {
                return NotFound("No disposition found for this Account ID with Type 'OWNER'.");
            }

            if (disposition.Customer == null)
            {
                return NotFound("Customer data not found for this disposition.");
            }

            if (disposition.Account == null)
            {
                return NotFound("Account data not found for this disposition.");
            }

            var dto = new CustomerAccountInfoDto
            {
                CustomerId = disposition.Customer.CustomerId,
                FullName = $"{disposition.Customer.Givenname} {disposition.Customer.Surname}",
                Balance = disposition.Account.Balance
            };

            return Ok(dto);
        }
    }
}
