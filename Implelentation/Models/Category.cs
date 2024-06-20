using System.Collections;

namespace aqay_apis;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    //One to many relationship with products
    public ICollection<Product> Products{ get; set; }
}
