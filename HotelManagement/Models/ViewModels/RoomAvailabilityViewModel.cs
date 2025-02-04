using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.ViewModels
{
    public class RoomAvailabilityViewModel
    {
        public string? RoomType { get; set; }

        [Required]
        public DateTime check_in_date { get; set; }

        [Required]
        public DateTime check_out_date { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "People count must be between 1 and 10.")]
        public int PeopleCount { get; set; }

        public IEnumerable<AvailableRoom>? AvailableRooms { get; set; }
    }

    public class AvailableRoom
    {
        public int room_id { get; set; }
        public string room_number { get; set; }
        public string room_type { get; set; }
        public int bed_count { get; set; }
        public string features { get; set; }
    }

    public class BookingViewModel
    {
        [Required(ErrorMessage ="Room is required.")]
        public int room_id { get; set; }

        [Required(ErrorMessage = "Guest is required.")]
        public int guest_id { get; set; }

        [Required(ErrorMessage = "Check in Date is required.")]
        public string check_in_date { get; set; }

        [Required(ErrorMessage = "Check out Date is required.")]
        public string check_out_date { get; set; }
        public string payment_status { get; set; }
        public decimal? total_price { get; set; }
        public string? status { get; set; }
    }

}
