namespace aqay_apis.Models
{
    public class Consumer : User
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string role { get; set; } = "Consumer";
        public int WishListId { get; set; }

        // Relationships
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }

    }
}
