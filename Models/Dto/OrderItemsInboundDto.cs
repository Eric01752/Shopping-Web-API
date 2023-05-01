namespace ShoppingApp.Models.Dto
{
    public class OrderItemsInboundDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
    }
}
