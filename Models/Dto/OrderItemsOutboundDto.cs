namespace ShoppingApp.Models.Dto
{
    public class OrderItemsOutboundDto
    {
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
    }
}
