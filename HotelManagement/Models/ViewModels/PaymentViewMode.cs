using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.ViewModels
{
    public class ServicePaymentViewModel
    {
        [Required(ErrorMessage="Booking is required.")]
        public int booking_id { get; set; }

        [Required(ErrorMessage = "Service is required.")]
        public int service_id { get; set; } 

        public IEnumerable<SelectListItem>? Services { get; set; } 

        public IEnumerable<Booking>? Bookings { get; set; } 
    }
}