using HotelManagement.Models;

namespace HotelManagement.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<int> CreateBookingAsync(Booking booking);
        Task<bool> UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByPaymentStatusAsync(string paymentStatus);
        Task<IEnumerable<string>> GetBookedDatesAsync(int roomId);
        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status);
    }
}
