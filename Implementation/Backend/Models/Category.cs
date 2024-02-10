using System.ComponentModel.DataAnnotations;

namespace Aqay_v2.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required,MaxLength(150)]
        public string Name { get; set; }
    }
}
