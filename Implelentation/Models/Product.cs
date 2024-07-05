using System.Collections;
using aqay_apis.Models;

namespace aqay_apis;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastEdit { get; set; }
    public int Rate { get; set; } = 0;
    public int ReviewCount { get; set; } = 0;
    public ICollection<Tag> Tags{ get; set; }
    //one to many relationship with Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    //one to many relationship with Review
    public ICollection<Review> Reviews{ get; set; }
    //Many to one relationship with Brand
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    //one to many relationship with product Variants
    public ICollection<ProductVariant> ProductVariants { get; set; }
}
