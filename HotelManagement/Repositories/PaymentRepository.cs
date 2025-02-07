using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace HotelManagement.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DatabaseContext _database;

        public PaymentRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT 
                        p.payment_id,
                        p.booking_id,
                        p.payment_amount,
                        p.payment_date,
                        p.payment_type,
                        CASE 
                            WHEN p.payment_type = 'Service' THEN s.service_name
                            ELSE NULL
                        END AS display_service_name
                    FROM payments p
                    LEFT JOIN payment_services ps ON p.payment_id = ps.payment_id
                    LEFT JOIN services s ON ps.service_id = s.service_id";
                return await connection.QueryAsync<Payment>(query);
            }
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Payments WHERE payment_id = @paymentId";
                return await connection.QueryFirstOrDefaultAsync<Payment>(query, new { paymentId });
            }
        }

        public async Task<int> CreatePaymentAsync(Payment payment)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    INSERT INTO Payments (booking_id, payment_amount, payment_date, payment_type)
                    VALUES (@booking_id, @payment_amount, @payment_date, @payment_type);
                    SELECT CAST(SCOPE_IDENTITY() AS INT)";
                return await connection.ExecuteScalarAsync<int>(query, payment);
            }
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    UPDATE Payments 
                    SET booking_id = @booking_id, 
                        payment_amount = @payment_amount, 
                        payment_date = @payment_date, 
                        payment_type = @payment_type 
                    WHERE payment_id = @payment_id";
                var rowsAffected = await connection.ExecuteAsync(query, payment);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "DELETE FROM Payments WHERE payment_id = @paymentId";
                var rowsAffected = await connection.ExecuteAsync(query, new { paymentId });
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(int bookingId)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Payments WHERE booking_id = @bookingId";
                return await connection.QueryAsync<Payment>(query, new { bookingId });
            }
        }

        public async Task<decimal> CalculateTotalPriceAsync(int bookingId)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "CalculateBookingCost";
                var parameters = new DynamicParameters();
                parameters.Add("@BookingId", bookingId, DbType.Int32);

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(query, parameters, commandType: CommandType.StoredProcedure);

                if(result?.total_cost == null || result?.total_cost is DBNull)
                    throw new Exception("Total price calculation failed or booking not found.");

                return Convert.ToDecimal(result?.total_cost);
            }
        }
    }
}
