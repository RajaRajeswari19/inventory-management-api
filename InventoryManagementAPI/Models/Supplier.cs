using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string? Contact { get; set; }
        [StringLength(255)]
        public string? Address { get; set; }
    }
}