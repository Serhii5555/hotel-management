using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class PaymentService
    {
        [Key]
        public int payment_service_id { get; set; }

        [Required]
        public int payment_id { get; set; }

        [Required]
        public int service_id { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Service amount must be a positive value.")]
        public decimal service_amount { get; set; }
    }
}
