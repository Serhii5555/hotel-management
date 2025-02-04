using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;

namespace HotelManagement.Repositories
{
    public class PaymentServiceRepository : IPaymentServiceRepository
    {
        private readonly DatabaseContext _database;

        public PaymentServiceRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<PaymentService>> GetAllPaymentServicesAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Payment_Services";
                return await connection.QueryAsync<PaymentService>(query);
            }
        }

        public async Task<PaymentService> GetPaymentServiceByIdAsync(int paymentServiceId)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Payment_Services WHERE payment_service_id = @paymentServiceId";
                return await connection.QueryFirstOrDefaultAsync<PaymentService>(query, new { paymentServiceId });
            }
        }

        public async Task<int> CreatePaymentServiceAsync(PaymentService paymentService)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"INSERT INTO Payment_Services 
                              (payment_id, service_id, service_amount)
                              VALUES 
                              (@payment_id, @service_id, @service_amount); 
                              SELECT CAST(SCOPE_IDENTITY() AS INT)";
                return await connection.ExecuteScalarAsync<int>(query, paymentService);
            }
        }

        public async Task<bool> UpdatePaymentServiceAsync(PaymentService paymentService)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"UPDATE Payment_Services 
                              SET payment_id = @payment_id, service_id = @service_id, 
                                  service_amount = @service_amount 
                              WHERE payment_service_id = @payment_service_id";
                var rowsAffected = await connection.ExecuteAsync(query, paymentService);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeletePaymentServiceAsync(int paymentServiceId)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "DELETE FROM Payment_Services WHERE payment_service_id = @paymentServiceId";
                var rowsAffected = await connection.ExecuteAsync(query, new { paymentServiceId });
                return rowsAffected > 0;
            }
        }
    }
}
