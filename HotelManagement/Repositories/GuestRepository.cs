using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;

namespace HotelManagement.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly DatabaseContext _database;

        public GuestRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Guest>> GetAllGuestsAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Guests";
                return await connection.QueryAsync<Guest>(query);
            }
        }

        public async Task<Guest> GetGuestByIdAsync(int guest_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Guests WHERE guest_id = @guest_id";
                return await connection.QueryFirstOrDefaultAsync<Guest>(query, new { guest_id });
            }
        }

        public async Task<int> CreateGuestAsync(Guest guest)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"INSERT INTO Guests 
                              (first_name, last_name, email, phone_number, passport_code, date_of_birth, nationality) 
                              VALUES 
                              (@first_name, @last_name, @email, @phone_number, @passport_code, @date_of_birth, @nationality); 
                              SELECT CAST(SCOPE_IDENTITY() AS INT)";
                return await connection.ExecuteScalarAsync<int>(query, guest);
            }
        }

        public async Task<bool> UpdateGuestAsync(Guest guest)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"UPDATE Guests 
                              SET first_name = @first_name, last_name = @last_name, email = @email, 
                                  phone_number = @phone_number, passport_code = @passport_code, 
                                  date_of_birth = @date_of_birth, nationality = @nationality 
                              WHERE guest_id = @guest_id";
                var rowsAffected = await connection.ExecuteAsync(query, guest);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteGuestAsync(int guest_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "DELETE FROM Guests WHERE guest_id = @guest_id";
                var rowsAffected = await connection.ExecuteAsync(query, new { guest_id });
                return rowsAffected > 0;
            }
        }
    }
}
