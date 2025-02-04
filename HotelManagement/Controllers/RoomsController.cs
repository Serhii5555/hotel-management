using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace HotelManagement.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomRepository _rooms;
        private readonly IRoomTypeRepository _roomTypes;

        public RoomsController(IRoomRepository rooms, IRoomTypeRepository roomTypes)
        {
            _rooms = rooms;
            _roomTypes = roomTypes;
        }

        public async Task<IActionResult> Index()
        {
            var roomTypes = await _roomTypes.GetAllRoomTypesAsync();
            ViewBag.RoomTypes = roomTypes;
            var rooms = await _rooms.GetAllRoomsAsync();
            return View(rooms);
        }

        public async Task<IActionResult> Create()
        {
            var roomTypes = await _roomTypes.GetAllRoomTypesAsync();
            ViewBag.RoomTypes = roomTypes;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (!ModelState.IsValid)
            {
                var roomTypes = await _roomTypes.GetAllRoomTypesAsync();
                ViewBag.RoomTypes = roomTypes;
                return View(room);
            }

            await _rooms.CreateRoomAsync(room);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var roomTypes = await _roomTypes.GetAllRoomTypesAsync();
            ViewBag.RoomTypes = roomTypes;

            var room = await _rooms.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                await _rooms.UpdateRoomAsync(room);
                return RedirectToAction("Index");
            }

            var roomTypes = await _roomTypes.GetAllRoomTypesAsync();
            ViewBag.RoomTypes = roomTypes;

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            await _rooms.DeleteRoomAsync(id);
            return RedirectToAction("Index");
        }
    }
}
