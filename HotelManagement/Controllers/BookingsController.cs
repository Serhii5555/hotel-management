using HotelManagement.Models;
using HotelManagement.Models.ViewModels;
using HotelManagement.Repositories;
using HotelManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelManagement.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingRepository _bookings;
        private readonly IGuestRepository _guests;
        private readonly IRoomRepository _rooms;
        private readonly IRoomTypeRepository _roomsTypes;

        public BookingsController(IBookingRepository bookings, IGuestRepository guest, IRoomRepository room, IRoomTypeRepository roomsTypes)
        {
            _bookings = bookings;
            _guests = guest;
            _rooms = room;
            _roomsTypes = roomsTypes;
        }

        private async Task PopulateViewBagAsync()
        {
            ViewBag.Guests = new SelectList(await _guests.GetAllGuestsAsync(), nameof(Guest.guest_id), nameof(Guest.guest_display));
            ViewBag.Rooms = new SelectList(await _rooms.GetAllRoomsAsync(), nameof(Room.room_id), nameof(Room.room_number));
        }


        public async Task<IActionResult> Index()
        {
            var bookings = await _bookings.GetAllBookingsAsync();
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {

            var model = new Booking
            {
                room_id = roomId,
                check_in_date = checkInDate,
                check_out_date = checkOutDate,
            };

            await PopulateViewBagAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagAsync();

                return View(booking);
            }


            await _bookings.CreateBookingAsync(booking);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            await PopulateViewBagAsync();

            var booking = await _bookings.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Booking booking)
        {
            if (ModelState.IsValid)
            {
                await _bookings.UpdateBookingAsync(booking);
                return RedirectToAction("Index");
            }

            await PopulateViewBagAsync();

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            await _bookings.DeleteBookingAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetBookedDates(int roomId)
        {
            var bookedDates = await _bookings.GetBookedDatesAsync(roomId);

            if (bookedDates != null && bookedDates.Any())
            {
                return Json(new { success = true, bookedDates });
            }
            return Json(new { success = false, message = "No booked dates found." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableRooms()
        {
            ViewBag.RoomTypes = new SelectList(await _roomsTypes.GetAllRoomTypesAsync(), "name", "name");

            return View(new RoomAvailabilityViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableRooms(RoomAvailabilityViewModel model)
        {
            ViewBag.RoomTypes = new SelectList(await _roomsTypes.GetAllRoomTypesAsync(), "name", "name");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.AvailableRooms = await _rooms.FindAvailableRoomsAsync(
                model.RoomType,
                model.check_in_date,
                model.check_out_date,
                model.PeopleCount
            );

            return View(model);
        }



    }
}


