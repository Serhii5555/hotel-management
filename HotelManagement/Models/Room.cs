using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Room
    {
        [Key]
        public int room_id { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        [StringLength(10, ErrorMessage = "Room number cannot exceed 10 characters.")]
        public string room_number { get; set; }

        [Required(ErrorMessage = "Room type is required.")]
        public int room_type_id { get; set; }

        [Required]
        public bool availability { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Bed count must be at least 1.")]
        public int? bed_count { get; set; }

        [StringLength(int.MaxValue, ErrorMessage = "Features description is too long.")]
        public string? features { get; set; }
    }
}
