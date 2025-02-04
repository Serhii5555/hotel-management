using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    public class RoomTypesController : Controller
    {
        private readonly IRoomTypeRepository _roomTypes;

        public RoomTypesController(IRoomTypeRepository roomTypes)
        {
            _roomTypes = roomTypes;
        }

        public async Task<IActionResult> Index()
        {
            var roomTypes = await _roomTypes.GetAllRoomTypesAsync();
            return View(roomTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomType roomType)
        {
            if (!ModelState.IsValid)
            {
                return View(roomType);
            }

            await _roomTypes.CreateRoomTypeAsync(roomType);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var roomType = await _roomTypes.GetRoomTypeByIdAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomType roomType)
        {
            if (!ModelState.IsValid)
            {
                return View(roomType);
            }

            await _roomTypes.UpdateRoomTypeAsync(roomType);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            await _roomTypes.DeleteRoomTypeAsync(id);
            return RedirectToAction("Index");
        }
    }
}
