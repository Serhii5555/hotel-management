using HotelManagement.Models;
using HotelManagement.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagement.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int room_id);
        Task<int> CreateRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(Room room);
        Task<bool> DeleteRoomAsync(int room_id);
        Task<IEnumerable<AvailableRoom>> FindAvailableRoomsAsync(string roomType, DateTime checkInDate, DateTime checkOutDate, int peopleCount);
    }
}
