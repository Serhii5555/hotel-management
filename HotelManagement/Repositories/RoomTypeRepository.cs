using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagement.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly DatabaseContext _database;

        public RoomTypeRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Room_Types";
                return await connection.QueryAsync<RoomType>(query);
            }
        }

        public async Task<RoomType> GetRoomTypeByIdAsync(int room_type_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "SELECT * FROM Room_Types WHERE room_type_id = @room_type_id";
                return await connection.QueryFirstOrDefaultAsync<RoomType>(query, new { room_type_id });
            }
        }

        public async Task<int> CreateRoomTypeAsync(RoomType roomType)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"INSERT INTO Room_Types (name, description, base_price) 
                              VALUES (@name, @description, @base_price); 
                              SELECT CAST(SCOPE_IDENTITY() AS INT)";
                return await connection.ExecuteScalarAsync<int>(query, roomType);
            }
        }

        public async Task<bool> UpdateRoomTypeAsync(RoomType roomType)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"UPDATE Room_Types 
                              SET name = @name, description = @description, base_price = @base_price 
                              WHERE room_type_id = @room_type_id";
                var rowsAffected = await connection.ExecuteAsync(query, roomType);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteRoomTypeAsync(int room_type_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "DELETE FROM Room_Types WHERE room_type_id = @room_type_id";
                var rowsAffected = await connection.ExecuteAsync(query, new { room_type_id });
                return rowsAffected > 0;
            }
        }
    }
}
