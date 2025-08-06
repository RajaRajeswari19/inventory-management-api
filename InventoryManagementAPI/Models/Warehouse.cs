using System.ComponentModel.DataAnnotations;

namespace InventoryManagementAPI.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(255)]
        public string? Location { get; set; }
        [Range(0, int.MaxValue)]
        public int Capacity { get; set; }
    }
}