using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Service
    {
        [Key]
        public int service_id { get; set; }

        [Required(ErrorMessage = "Service name is required.")]
        [StringLength(50, ErrorMessage = "Service name cannot exceed 50 characters.")]
        public string service_name { get; set; }

        [StringLength(int.MaxValue, ErrorMessage = "Description is too long.")]
        public string? description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal price { get; set; }
    }
}
