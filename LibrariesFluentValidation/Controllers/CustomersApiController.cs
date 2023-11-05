﻿using AutoMapper;
using FluentValidation;
using LibrariesFluentValidation.DTOs;
using LibrariesFluentValidation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LibrariesFluentValidation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<Customer> _customerValidator; //Api Hata Mesajları için bir instance oluşturduk.
        private readonly IMapper _mapper;
        public CustomersApiController(AppDbContext context, IValidator<Customer> customerValidator,IMapper mapper)
        {
            _context = context;
            _customerValidator = customerValidator;
            _mapper = mapper;
        }
        //[Route("MappingOrnek")]-->/[action] yazmadan böyle yapabilirdik
        [HttpGet]
        public IActionResult MappingOrnek()
        {
            Customer customer = new Customer { ID = 9, CustomerName = "Taylan", CustomerMail = "uozen972@gmail.com", CustomerAge = 27 ,CreditCard=new CreditCard { Number = "123456789", ValidDate = DateTime.Now.AddDays(-100) } };
            return Ok(_mapper.Map<CustomerDto>(customer));
        }
        // GET: api/CustomersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            List<Customer> customers = await _context.Customers.ToListAsync();
            if (_context.Customers == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<CustomerDto>>(customers); //mapper instance'ının Map metodunu kullanarak önce bir hedef belirttik(CustomerDto), kaynak olarakta customers belirtmiş olduk.
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDtoTurkce>>> GetCustomersTurkce()
        {
            List<Customer> customers = await _context.Customers.ToListAsync();
            if (_context.Customers == null)
            {
                return NotFound();
            }
            return _mapper.Map<List<CustomerDtoTurkce>>(customers); //mapper instance'ının Map metodunu kullanarak önce bir hedef belirttik(CustomerDto), kaynak olarakta customers belirtmiş olduk.
        }
        // GET: api/CustomersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        // PUT: api/CustomersApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.ID)
            {
                return BadRequest();
            }
            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        // POST: api/CustomersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            var result = _customerValidator.Validate(customer);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(x => new { property = x.PropertyName, error = x.ErrorMessage }));
            }
            if (_context.Customers == null)
            {
                return Problem("Entity set 'AppDbContext.Customers'  is null.");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCustomer", new { id = customer.ID }, customer);
        }
        // DELETE: api/CustomersApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}