using HotelManagement.Models.StatisticsModels;

namespace HotelManagement.Repositories.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<decimal> GetTotalBookingRevenueAsync();
        Task<IEnumerable<ServiceIncome>> GetTop3ServicesByIncomeAsync();
        Task<IEnumerable<AgeRangeStatistics>> GetGuestCountByAgeRangeAsync();
        Task<IEnumerable<CountryGuestPercentage>> GetGuestPercentageByCountryAsync();
        Task<decimal> GetRevenueForPeriodAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<RoomTypeBookingPercentage>> GetTop3RoomTypesByBookingsAsync();
    }

}
