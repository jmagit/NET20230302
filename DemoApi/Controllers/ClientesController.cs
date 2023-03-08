using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoApi.Models;

namespace DemoApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase {
        private readonly AdventureWorksLT2019Context _context;

        public ClientesController(AdventureWorksLT2019Context context) {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers() {
            if(_context.Customers == null) {
                return NotFound();
            }
            return await _context.Customers
                .Where(f => f.LastName.Length > 3)
                .OrderBy(f => f.LastName)
                .ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id) {
            if(_context.Customers == null) {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);

            if(customer == null) {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer) {
            if(id != customer.CustomerId) {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!CustomerExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Clientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer) {
            if(_context.Customers == null) {
                return Problem("Entity set 'AdventureWorksLT2019Context.Customers'  is null.");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id) {
            if(_context.Customers == null) {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if(customer == null) {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id) {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
