namespace aqay_apis.Models
{
    public class Consumer : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Review Review{ get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public WishList WishList { get; set; }
        public int? WishListId { get; set; } 
    }
}
