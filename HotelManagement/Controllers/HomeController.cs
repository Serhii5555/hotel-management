using HotelManagement.Models;
using HotelManagement.Models.StatisticsModels;
using HotelManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticsRepository _statistics;

        public HomeController(IStatisticsRepository statistics)
        {
            _statistics = statistics;
        }

        public async Task<IActionResult> Index()
        {
            var model = await GetStatisticsViewModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetRevenueForPeriod(DateTime startDate, DateTime endDate)
        {
            var model = await GetStatisticsViewModelAsync();
            model.RevenueForPeriod = new RevenuePeriod
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = await _statistics.GetRevenueForPeriodAsync(startDate, endDate)
            };

            return View("Index", model);
        }

        private async Task<StatisticsViewModel> GetStatisticsViewModelAsync()
        {
            return new StatisticsViewModel
            {
                AgeRangeStatistics = await _statistics.GetGuestCountByAgeRangeAsync(),
                TotalBookingRevenue = await _statistics.GetTotalBookingRevenueAsync(),
                ServiceIncome = await _statistics.GetTop3ServicesByIncomeAsync(),
                CountryGuestPercentage = (await _statistics.GetGuestPercentageByCountryAsync()).Take(5),
                TopRoomTypes = await _statistics.GetTop3RoomTypesByBookingsAsync()
            };
        }
    }
}
