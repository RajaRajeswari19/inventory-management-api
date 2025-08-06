using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
    }
}