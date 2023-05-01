using ShoppingApp.Models;

namespace ShoppingApp.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomer(int id);
        IEnumerable<Order> GetOrdersByCustomerId(int customerId);
        bool CustomerExists(int id);
        bool CreateCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(Customer customer);
        bool Save();
    }
}
