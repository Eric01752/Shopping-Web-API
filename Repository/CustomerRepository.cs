using ShoppingApp.Interfaces;
using ShoppingApp.Models;

namespace ShoppingApp.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDBContext _context;

        public CustomerRepository(AppDBContext context)
        {
            _context = context;
        }

        public bool CreateCustomer(Customer customer)
        {
            _context.Add(customer);
            return Save();
        }

        public bool CustomerExists(int id)
        {
            return _context.Customers.Any(c => c.Id == id);
        }

        public bool DeleteCustomer(Customer customer)
        {
            _context.Remove(customer);
            return Save();
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.OrderBy(c => c.LastName).ToList();
        }

        public IEnumerable<Order> GetOrdersByCustomerId(int customerId)
        {
            return _context.Orders.Where(o => o.Customer.Id == customerId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            return Save();
        }
    }
}
