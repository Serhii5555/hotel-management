using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace HotelManagement.Validations
{
    public class UniquePhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Phone number is required.");
            }

            var dbContext = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));
            if (dbContext == null)
            {
                throw new InvalidOperationException("DatabaseContext is not available.");
            }

            var currentGuest = (HotelManagement.Models.Guest)validationContext.ObjectInstance;

            using (var connection = dbContext.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT COUNT(1)
                    FROM Guests
                    WHERE phone_number = @PhoneNumber AND guest_id != @GuestId";

                var command = connection.CreateCommand();
                command.CommandText = query;

                var phoneParam = command.CreateParameter();
                phoneParam.ParameterName = "@PhoneNumber";
                phoneParam.Value = value.ToString();
                command.Parameters.Add(phoneParam);

                var guestIdParam = command.CreateParameter();
                guestIdParam.ParameterName = "@GuestId";
                guestIdParam.Value = currentGuest.guest_id;
                command.Parameters.Add(guestIdParam);

                var result = (int)command.ExecuteScalar();

                if (result > 0)
                {
                    return new ValidationResult("This phone number is already in use.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
