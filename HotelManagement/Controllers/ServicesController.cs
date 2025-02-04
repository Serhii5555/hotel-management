using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelManagement.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceRepository _servicesRepository;

        public ServicesController(IServiceRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _servicesRepository.GetAllServicesAsync();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                await _servicesRepository.CreateServiceAsync(service);
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var service = await _servicesRepository.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Service service)
        {
            if (id != service.service_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var success = await _servicesRepository.UpdateServiceAsync(service);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _servicesRepository.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            var success = await _servicesRepository.DeleteServiceAsync(id);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
