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

        [HttpGet("show_all_accounts")]
        public async Task<ActionResult<PagedResult<CustomerAccountAllDto>>> GetAllAccounts(int page = 1, int pageSize = 25)
        {
            var customerAccounts = await _context.Dispositions
                .Include(d => d.Customer)
                .Include(d => d.Account)
                .Where(d => d.Type == "OWNER") // Only include account owners, not all dispositions
                .Select(d => new CustomerAccountAllDto
                {
                    FullName = d.Customer.Givenname + " " + d.Customer.Surname,
                    AccountId = d.Account.AccountId,
                    Balance = d.Account.Balance
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.Dispositions
                .Where(d => d.Type == "OWNER")
                .CountAsync();

            return Ok(new PagedResult<CustomerAccountAllDto>
            {
                TotalCount = totalCount,
                Items = customerAccounts
            });
        }
        
        public class PagedResult<T>
        {
            public int TotalCount { get; set; }
            public List<T> Items { get; set; } = new();
        }
    }
}
