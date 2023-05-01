using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
