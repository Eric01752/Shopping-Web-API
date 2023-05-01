using ShoppingApp.Interfaces;
using ShoppingApp.Models;
using ShoppingApp.Models.Dto;

namespace ShoppingApp.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDBContext _context;

        public OrderRepository(AppDBContext context)
        {
            _context = context;
        }

        public bool CreateOrder(Order order)
        {
            _context.Add(order);
            return Save();
        }

        public bool CreateOrderItems(IEnumerable<OrderItems> orderItems)
        {
            foreach (var item in orderItems)
            {
                _context.Add(item);
            }
            return Save();
        }

        public bool DeleteOrder(Order order)
        {
            _context.Remove(order);
            return Save();
        }

        public bool DeleteOrderItems(IEnumerable<OrderItems> orderItems)
        {
            foreach(var item in orderItems)
            {
                _context.Remove(item);
            }
            return Save();
        }

        public Order GetOrder(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<OrderItems> GetOrderItems(int id)
        {
            return _context.OrderItems.Where(oi => oi.OrderId == id).Select(oi => new OrderItems { Order = oi.Order, Product = oi.Product, Quantity = oi.Quantity, Price = oi.Price }).ToList();
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders.OrderBy(o => o.OrderDate).ToList();
        }

        public bool OrderExists(int id)
        {
            return _context.Orders.Any(o => o.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOrder(Order order)
        {
            _context.Update(order);
            return Save();
        }

        public bool UpdateOrderItems(IEnumerable<OrderItems> orderItems)
        {
            foreach (var item in orderItems)
            {
                _context.Update(item);
            }
            return Save();
        }
    }
}
