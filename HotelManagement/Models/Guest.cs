using HotelManagement.Validations;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class Guest
    {
        [Key] 
        public int guest_id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string last_name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        [RegularExpression(@"^\+?[0-9\s-]+$", ErrorMessage = "Invalid phone number format.")]
        [UniquePhoneNumber(ErrorMessage = "This phone number is already in use.")]
        public string phone_number { get; set; }

        [Required(ErrorMessage = "Passport code is required.")]
        [StringLength(20, ErrorMessage = "Passport code cannot exceed 20 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Passport code can only contain letters and numbers.")]
        public string passport_code { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public DateTime date_of_birth { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters.")]
        public string nationality { get; set; }

        public string? guest_display => $"{first_name} {last_name} | {guest_id}";
    }
}
