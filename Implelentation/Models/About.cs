using aqay_apis.Models;

namespace aqay_apis;

public class About
{
    public int Id { get; set; }
    public string Info { get; set; }
    public string Creator { get; set; }
    // one to one
    public Brand Brand { get; set; }
}
