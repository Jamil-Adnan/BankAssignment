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
        
        [HttpGet("Show_all_accounts")]
        public async Task<ActionResult<PagedResult<CustomerAccountAllDto>>> GetAllCustomersWithAccounts(
    int page = 1, int pageSize = 10)
        {
            var query = _context.Dispositions
                .Include(d => d.Customer)
                .Include(d => d.Account)
                .Where(d => d.Account != null && d.Customer != null)
                .Select(d => new CustomerAccountAllDto
                {
                    FullName = d.Customer.Givenname + " " + d.Customer.Surname,
                    AccountId = d.Account.AccountId,
                    Balance = d.Account.Balance
                });

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<CustomerAccountAllDto>
            {
                TotalCount = totalCount,
                Items = items
            });
        }



        // GET: api/customers/{id}  will fetch a specific customer with the id
        [HttpGet("{id}/info_by_account_number")]
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

    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
