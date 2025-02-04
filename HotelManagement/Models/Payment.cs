using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Payment
    {
        [Key]
        public int payment_id { get; set; }

        [Required(ErrorMessage = "Booking is required.")]
        public int booking_id { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Payment amount must be a positive value.")]
        public decimal payment_amount { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        [DataType(DataType.Date)]
        public DateTime payment_date { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("Hotel|Service", ErrorMessage = "Invalid payment type.")]
        public string payment_type { get; set; }

        public string? display_service_name { get; set; }
    }
}
