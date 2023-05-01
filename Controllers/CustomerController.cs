using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Interfaces;
using ShoppingApp.Models;
using ShoppingApp.Models.Dto;
using ShoppingApp.Repository;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _mapper.Map<List<CustomerDto>>(_customerRepository.GetCustomers());

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            if (!_customerRepository.CustomerExists(id))
            {
                return NotFound();
            }

            var customer = _mapper.Map<CustomerDto>(_customerRepository.GetCustomer(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(customer);
        }

        [HttpGet("order/{customerId}")]
        public IActionResult GetOrdersByCustomerId(int customerId)
        {
            var orders = _mapper.Map<List<OrderDto>>(_customerRepository.GetOrdersByCustomerId(customerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(orders);
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerCreate)
        {
            if (customerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var customer = _customerRepository.GetCustomers().FirstOrDefault(c => c.Email.Trim().ToUpper() == customerCreate.Email.Trim().ToUpper());

            if (customer != null)
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customerMap = _mapper.Map<Customer>(customerCreate);

            if (!_customerRepository.CreateCustomer(customerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{customerId}")]
        public IActionResult UpdateCustomer(int customerId, [FromBody] CustomerDto updatedCustomer)
        {
            if (updatedCustomer == null)
            {
                return BadRequest(ModelState);
            }

            if (customerId != updatedCustomer.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerMap = _mapper.Map<Customer>(updatedCustomer);

            if (!_customerRepository.UpdateCustomer(customerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(int customerId)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerToDelete = _customerRepository.GetCustomer(customerId);

            if (!_customerRepository.DeleteCustomer(customerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
