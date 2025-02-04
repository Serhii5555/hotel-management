using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace HotelManagement.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DatabaseContext _database;

        public BookingRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT 
                        b.booking_id, 
                        b.guest_id, 
                        b.room_id, 
                        b.check_in_date, 
                        b.check_out_date, 
                        b.status, 
                        b.total_price, 
                        b.payment_status, 
                        r.room_number AS display_room_name, 
                        CONCAT(g.first_name, ' ', g.last_name) AS display_guest_name
                    FROM 
                        Bookings b
                    LEFT JOIN Rooms r ON b.room_id = r.room_id
                    LEFT JOIN Guests g ON b.guest_id = g.guest_id";

                var bookings = await connection.QueryAsync<Booking>(query);
                return bookings;
            }
        }


        public async Task<Booking> GetBookingByIdAsync(int booking_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT 
                        b.booking_id, 
                        b.guest_id, 
                        b.room_id, 
                        b.check_in_date, 
                        b.check_out_date, 
                        b.status, 
                        b.total_price, 
                        b.payment_status, 
                        r.room_number AS display_room_name, 
                        CONCAT(g.first_name, ' ', g.last_name) AS display_guest_name
                    FROM 
                        Bookings b
                    LEFT JOIN Rooms r ON b.room_id = r.room_id
                    LEFT JOIN Guests g ON b.guest_id = g.guest_id
                    WHERE 
                        b.booking_id = @booking_id";

                return await connection.QueryFirstOrDefaultAsync<Booking>(query, new { booking_id });
            }
        }


        public async Task<int> CreateBookingAsync(Booking booking)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"INSERT INTO Bookings 
                              (guest_id, room_id, check_in_date, check_out_date, status, total_price, payment_status)
                              VALUES 
                              (@guest_id, @room_id, @check_in_date, @check_out_date, @status, @total_price, @payment_status); 
                              SELECT CAST(SCOPE_IDENTITY() AS INT)";
                return await connection.ExecuteScalarAsync<int>(query, booking);
            }
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"UPDATE Bookings 
                              SET guest_id = @guest_id, room_id = @room_id, 
                                  check_in_date = @check_in_date, check_out_date = @check_out_date, 
                                  status = @status, total_price = @total_price, 
                                  payment_status = @payment_status 
                              WHERE booking_id = @booking_id";
                var rowsAffected = await connection.ExecuteAsync(query, booking);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteBookingAsync(int booking_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "DELETE FROM Bookings WHERE booking_id = @booking_id";
                var rowsAffected = await connection.ExecuteAsync(query, new { booking_id });
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByPaymentStatusAsync(string paymentStatus)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT 
                        b.booking_id, 
                        b.guest_id, 
                        b.room_id, 
                        b.check_in_date, 
                        b.check_out_date, 
                        b.status, 
                        b.total_price, 
                        b.payment_status, 
                        r.room_number AS display_room_name, 
                        CONCAT(g.first_name, ' ', g.last_name) AS display_guest_name
                    FROM 
                        Bookings b
                    LEFT JOIN Rooms r ON b.room_id = r.room_id
                    LEFT JOIN Guests g ON b.guest_id = g.guest_id
                    WHERE 
                        b.payment_status = @paymentStatus";

                return await connection.QueryAsync<Booking>(query, new { paymentStatus });
            }
        }


        public async Task<IEnumerable<string>> GetBookedDatesAsync(int roomId)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT check_in_date, check_out_date 
                    FROM Bookings 
                    WHERE room_id = @roomId AND status IN ('Booked', 'Pending')";


                var bookings = await connection.QueryAsync<dynamic>(query, new { roomId });

                var bookedDates = new List<string>();

                foreach (var booking in bookings)
                {
                    DateTime checkInDate = booking.check_in_date;
                    DateTime checkOutDate = booking.check_out_date;

                    for (var date = checkInDate; date <= checkOutDate; date = date.AddDays(1))
                    {
                        bookedDates.Add(date.ToString("yyyy-MM-dd"));
                    }
                }

                return bookedDates;
            }
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync(string status)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
            SELECT 
                b.booking_id, 
                b.guest_id, 
                b.room_id, 
                b.check_in_date, 
                b.check_out_date, 
                b.status, 
                b.total_price, 
                b.payment_status, 
                r.room_number AS display_room_name, 
                CONCAT(g.first_name, ' ', g.last_name) AS display_guest_name
            FROM 
                Bookings b
            LEFT JOIN Rooms r ON b.room_id = r.room_id
            LEFT JOIN Guests g ON b.guest_id = g.guest_id
            WHERE 
                b.status = @status";

                return await connection.QueryAsync<Booking>(query, new { status });
            }
        }
    }
}
