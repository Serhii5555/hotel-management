using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagement.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DatabaseContext _database;

        public ServiceRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Services";
                return await connection.QueryAsync<Service>(query);
            }
        }

        public async Task<Service> GetServiceByIdAsync(int service_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Services WHERE service_id = @service_id";
                return await connection.QueryFirstOrDefaultAsync<Service>(query, new { service_id });
            }
        }

        public async Task<int> CreateServiceAsync(Service service)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"INSERT INTO Services (service_name, description, price) 
                              VALUES (@service_name, @description, @price); 
                              SELECT CAST(SCOPE_IDENTITY() AS INT)";
                return await connection.ExecuteScalarAsync<int>(query, service);
            }
        }

        public async Task<bool> UpdateServiceAsync(Service service)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"UPDATE Services 
                              SET service_name = @service_name, description = @description, price = @price 
                              WHERE service_id = @service_id";
                var rowsAffected = await connection.ExecuteAsync(query, service);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteServiceAsync(int service_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "DELETE FROM Services WHERE service_id = @service_id";
                var rowsAffected = await connection.ExecuteAsync(query, new { service_id });
                return rowsAffected > 0;
            }
        }
    }
}
