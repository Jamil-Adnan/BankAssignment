using BankBlazor.API.Data;
using BankBlazor.API.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly BankBlazorContext _context;

        public CustomersController(BankBlazorContext context)
        {
            _context = context;
        }

        // GET: api/customers  It is goin to get all the customers from the database
        [HttpGet("accounts")]
        public async Task<ActionResult<IEnumerable<CustomerAccountAllDto>>> GetAllCustomersWithAccounts()
        {
            var customerAccounts = await _context.Dispositions
                .Include(d => d.Customer)
                .Include(d => d.Account)
                .Where(d => d.Account != null && d.Customer != null)
                .Select(d => new CustomerAccountAllDto
                {
                    FullName = d.Customer.Givenname + " " + d.Customer.Surname,
                    AccountId = d.Account.AccountId,
                    Balance = d.Account.Balance
                })
                .ToListAsync();

            return Ok(customerAccounts);
        }

        // GET: api/customers/{id}  will fetch a specific customer with the id
        [HttpGet("{id}/accounts")]
        public async Task<ActionResult<CustomerAccountInfoDto>> GetCustomerWithAccounts(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Dispositions)
                    .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            var dto = new CustomerAccountInfoDto
            {
                FullName = $"{customer.Givenname} {customer.Surname}",
                Accounts = customer.Dispositions
                    .Where(d => d.Account != null)
                    .Select(d => new AccountDto
                    {
                        AccountId = d.Account.AccountId,
                        Balance = d.Account.Balance
                    }).ToList()
            };

            return Ok(dto);
        }
    }
}
