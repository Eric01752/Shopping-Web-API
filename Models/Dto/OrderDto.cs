namespace ShoppingApp.Models.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
    }
}
