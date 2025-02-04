using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class RoomType
    {
        [Key]
        public int room_type_id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
        public string name { get; set; }

        [StringLength(int.MaxValue, ErrorMessage = "Description is too long.")]
        public string? description { get; set; }

        [Required(ErrorMessage = "Base price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Base price must be a positive value.")]
        public decimal base_price { get; set; }
    }
}
