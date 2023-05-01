using ShoppingApp.Models;
using ShoppingApp.Models.Dto;

namespace ShoppingApp.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        Order GetOrder(int id);
        IEnumerable<OrderItems> GetOrderItems(int id);
        bool OrderExists(int id);
        bool CreateOrder(Order order);
        bool CreateOrderItems(IEnumerable<OrderItems> orderItems);
        bool UpdateOrder(Order order);
        bool UpdateOrderItems(IEnumerable<OrderItems> orderItems);
        bool DeleteOrder(Order order);
        bool DeleteOrderItems(IEnumerable<OrderItems> orderItems);
        bool Save();
    }
}
