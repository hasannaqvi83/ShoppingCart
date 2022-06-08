namespace ShoppingCart.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}