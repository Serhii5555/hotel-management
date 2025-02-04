using HotelManagement.Models;
using HotelManagement.Models.ViewModels;
using HotelManagement.Repositories;
using HotelManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace HotelManagement.Controllers
{
    public class GuestsController : Controller
    {
        private readonly IGuestRepository _guests;

        public GuestsController(IGuestRepository guests)
        {
            _guests = guests;
        }

        public async Task<IActionResult> Index()
        {
            var guests = await _guests.GetAllGuestsAsync();

            return View(guests);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guest guest)
        {
            if (!ModelState.IsValid)
            {
                return View(guest); 
            }

            _guests.CreateGuestAsync(guest);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var guest = await _guests.GetGuestByIdAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guest guest)
        {
            if (ModelState.IsValid)
            {
                await _guests.UpdateGuestAsync(guest);
                return RedirectToAction("Index");
            }

            return View(guest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            await _guests.DeleteGuestAsync(id);
            return RedirectToAction("Index");
        }
    }
}
