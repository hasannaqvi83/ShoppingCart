namespace ShoppingCart.DTOs
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
    }
}