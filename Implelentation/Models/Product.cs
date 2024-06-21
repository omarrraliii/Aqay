using System.Collections;
using aqay_apis.Models;

namespace aqay_apis;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Size  { get; set; }
    public string Describtion { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastEdit { get; set; }
    public int RED { get; set; }
    public int BLUE { get; set; }
    public int GREEN { get; set; }
    public int Quantity { get; set; }
    //many to many relationship with Tags
    public ICollection<Tag> Tags{ get; set; }
    //one to many relationship with Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    //one to many relationship with Review
    public ICollection<Review> Reviews{ get; set; }
    //Many to one relationship with Brand
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    public ICollection<ShoppingCart> ShoppingCarts { get; set;}
    public ICollection<WishList> WishLists { get; set; }

    // property to store the image URL
    public string ImageUrl { get; set; }
}
