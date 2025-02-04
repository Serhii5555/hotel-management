using HotelManagement.Models;
using HotelManagement.Models.StatisticsModels;
using HotelManagement.Repositories;
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
            var totalBookingRevenue = await _statistics.GetTotalBookingRevenueAsync();
            var serviceIncome = await _statistics.GetTop3ServicesByIncomeAsync();
            var ageRangeStatistics = await _statistics.GetGuestCountByAgeRangeAsync();
            var countryGuestPercentage = await _statistics.GetGuestPercentageByCountryAsync();
            var topRoomTypes = await _statistics.GetTop3RoomTypesByBookingsAsync();

            var model = new StatisticsViewModel
            {
                AgeRangeStatistics = ageRangeStatistics,
                TotalBookingRevenue = totalBookingRevenue,
                ServiceIncome = serviceIncome,
                CountryGuestPercentage = countryGuestPercentage.Take(5),
                TopRoomTypes = topRoomTypes
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetRevenueForPeriod(DateTime startDate, DateTime endDate)
        {
            var revenueForPeriod = await _statistics.GetRevenueForPeriodAsync(startDate, endDate);
            var totalBookingRevenue = await _statistics.GetTotalBookingRevenueAsync();
            var serviceIncome = await _statistics.GetTop3ServicesByIncomeAsync();
            var ageRangeStatistics = await _statistics.GetGuestCountByAgeRangeAsync();
            var countryGuestPercentage = await _statistics.GetGuestPercentageByCountryAsync();
            var topRoomTypes = await _statistics.GetTop3RoomTypesByBookingsAsync();

            var model = new StatisticsViewModel
            {
                AgeRangeStatistics = ageRangeStatistics,
                TotalBookingRevenue = totalBookingRevenue,
                ServiceIncome = serviceIncome,
                CountryGuestPercentage = countryGuestPercentage.Take(5),
                RevenueForPeriod = new RevenuePeriod
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalRevenue = revenueForPeriod
                },
                TopRoomTypes = topRoomTypes
            };


            return View("Index", model);
        }

    }
}
