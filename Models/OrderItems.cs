using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Models
{
    [PrimaryKey("OrderId", "ProductId")]
    public class OrderItems
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
    }
}
