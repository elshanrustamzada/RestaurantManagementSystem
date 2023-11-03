using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public bool IsDeactive { get; set; }
        public ProductDetail ProductDetail { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

    }
}
