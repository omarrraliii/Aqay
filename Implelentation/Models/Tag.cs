﻿namespace aqay_apis;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    //many to many relationship with products
    public List<Product> Products { get; set; }

}
