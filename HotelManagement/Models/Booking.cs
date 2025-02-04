using System;
using System.ComponentModel.DataAnnotations;
using HotelManagement.Validations;

namespace HotelManagement.Models
{
    public class Booking
    {
        [Key]
        public int booking_id { get; set; }

        [Required(ErrorMessage = "Guest is required.")]
        public int guest_id { get; set; }

        [Required(ErrorMessage = "Room is required.")]
        public int room_id { get; set; }

        [Required(ErrorMessage = "Check-in date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid check-in date.")]
        public DateTime check_in_date { get; set; }

        [Required(ErrorMessage = "Check-out date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid check-out date.")]
        [DateGreaterThan(nameof(check_in_date), ErrorMessage = "Check-out date must be after check-in date.")]
        public DateTime check_out_date { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        [RegularExpression("Booked|Pending|Completed", ErrorMessage = "Invalid status.")]
        public string? status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal? total_price { get; set; }

        [Required]
        [RegularExpression("Pending|Completed", ErrorMessage = "Invalid payment status.")]
        public string payment_status { get; set; }

        public string? display_room_name { get; set; }
        public string? display_guest_name { get; set; }
        public string? full_booking_display => $"{booking_id} | {display_guest_name} | {display_room_name}";
    }
}
