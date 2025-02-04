using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagement.Models.ViewModels;

namespace HotelManagement.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DatabaseContext _database;

        public RoomRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"SELECT room_id, room_number, room_type_id, availability, bed_count, features 
                              FROM Rooms";

                return await connection.QueryAsync<Room>(query);
            }
        }

        public async Task<Room> GetRoomByIdAsync(int room_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"SELECT room_id, room_number, room_type_id, availability, bed_count, features 
                              FROM Rooms
                              WHERE room_id = @room_id";

                return await connection.QueryFirstOrDefaultAsync<Room>(query, new { room_id });
            }
        }

        public async Task<int> CreateRoomAsync(Room room)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"INSERT INTO Rooms (room_number, room_type_id, availability, bed_count, features)
                              VALUES (@room_number, @room_type_id, @availability, @bed_count, @features);
                              SELECT CAST(SCOPE_IDENTITY() AS INT)";
                return await connection.ExecuteScalarAsync<int>(query, room);
            }
        }

        public async Task<bool> UpdateRoomAsync(Room room)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"UPDATE Rooms
                              SET room_number = @room_number, 
                                  room_type_id = @room_type_id, 
                                  availability = @availability, 
                                  bed_count = @bed_count, 
                                  features = @features
                              WHERE room_id = @room_id";
                var rowsAffected = await connection.ExecuteAsync(query, room);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteRoomAsync(int room_id)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "DELETE FROM Rooms WHERE room_id = @room_id";
                var rowsAffected = await connection.ExecuteAsync(query, new { room_id });
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<AvailableRoom>> FindAvailableRoomsAsync(string roomType, DateTime checkInDate, DateTime checkOutDate, int peopleCount)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = "FindAvailableRooms";
                var parameters = new
                {
                    RoomType = roomType,
                    CheckInDate = checkInDate,
                    CheckOutDate = checkOutDate,
                    PeopleCount = peopleCount
                };

                return await connection.QueryAsync<AvailableRoom>(
                    query,
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
