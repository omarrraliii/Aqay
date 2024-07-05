namespace aqay_apis.Models
{
    public class Consumer : User
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string role { get; set; } = "Consumer";


        // Relationships
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public WishList WishList { get; set; }

    }
}
