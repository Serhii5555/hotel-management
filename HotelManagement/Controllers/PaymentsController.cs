using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace HotelManagement.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentRepository _payments;
        private readonly IBookingRepository _bookings;
        private readonly IRoomTypeRepository _roomTypes;
        private readonly IServiceRepository _services;
        private readonly IPaymentServiceRepository _paymentServices;

        public PaymentsController(IPaymentRepository payments, IBookingRepository bookings, IRoomTypeRepository roomTypes, IServiceRepository serviceRepository, IPaymentServiceRepository paymentServices)
        {
            _payments = payments;
            _bookings = bookings;
            _roomTypes = roomTypes;
            _services = serviceRepository;
            _paymentServices = paymentServices;
        }

        public async Task<ActionResult> Index()
        {
            var payments = await _payments.GetAllPaymentsAsync();
            ViewBag.Bookings = await _bookings.GetAllBookingsAsync();
            return View(payments);
        }

        public async Task<ActionResult> ServicePayments()
        {
            var payments = await _payments.GetAllPaymentsAsync();
            ViewBag.Bookings = await _bookings.GetAllBookingsAsync();
            return View(payments);
        }

        public async Task<ActionResult> HotelPayments()
        {
            var payments = await _payments.GetAllPaymentsAsync();
            ViewBag.Bookings = await _bookings.GetAllBookingsAsync();
            return View(payments);
        }

        public async Task<IActionResult> HotelPaymentCreate()
        {
            ViewBag.Bookings = await _bookings.GetBookingsByPaymentStatusAsync("Pending");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> HotelPaymentCreate(Payment model)
        {
            ViewBag.Bookings = await _bookings.GetBookingsByPaymentStatusAsync("Pending");

            if (ModelState.IsValid)
            {
                var totalPrice = await _payments.CalculateTotalPriceAsync(model.booking_id);
                var payment = new Payment
                {
                    booking_id = model.booking_id,
                    payment_amount = totalPrice,
                    payment_date = DateTime.Now,
                    payment_type = "Hotel"
                };

                await _payments.CreatePaymentAsync(payment);

                return RedirectToAction("HotelPayments", "Payments");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            await _payments.DeletePaymentAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetTotalPrice(int bookingId)
        {
            if (bookingId == 0)
            {
                return BadRequest("Invalid booking ID.");
            }

            try
            {
                var totalPrice = await _payments.CalculateTotalPriceAsync(bookingId);
                return Json(new { success = true, totalPrice });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> ServicePaymentCreate()
        {
            var model = new ServicePaymentViewModel();
            await PrepareServicePaymentViewModel(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ServicePaymentCreate(ServicePaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PrepareServicePaymentViewModel(model);
                return View(model);
            }

            var service = await _services.GetServiceByIdAsync(model.service_id);
            if (service == null)
            {
                ModelState.AddModelError("", "Service not found.");
                await PrepareServicePaymentViewModel(model);
                return View(model);
            }

            var payment = await CreatePaymentAsync(model.booking_id, service.price);
            await CreatePaymentServiceAsync(payment, model.service_id, service.price);

            return RedirectToAction("ServicePayments", "Payments");
        }

        private async Task<Payment> CreatePaymentAsync(int bookingId, decimal amount)
        {
            var payment = new Payment
            {
                booking_id = bookingId,
                payment_amount = amount,
                payment_date = DateTime.Now,
                payment_type = "Service"
            };

            payment.payment_id = await _payments.CreatePaymentAsync(payment);
            return payment;
        }

        private async Task CreatePaymentServiceAsync(Payment payment, int serviceId, decimal amount)
        {
            var paymentService = new PaymentService
            {
                payment_id = payment.payment_id,
                service_id = serviceId,
                service_amount = amount
            };

            await _paymentServices.CreatePaymentServiceAsync(paymentService);
        }

        private async Task PrepareServicePaymentViewModel(ServicePaymentViewModel model)
        {
            model.Services = new SelectList(await _services.GetAllServicesAsync(), "service_id", "service_name");
            model.Bookings = await _bookings.GetBookingsByStatusAsync("Pending");
        }

        [HttpPost]
        public async Task<IActionResult> GetServicePrice(int serviceId)
        {
            var service = await _services.GetServiceByIdAsync(serviceId);
            if (service != null)
            {
                return Json(new { success = true, servicePrice = service.price });
            }
            return Json(new { success = false, message = "Service not found." });
        }


    }
}
