namespace HotelManagement.Models.StatisticsModels
{
    public class StatisticsViewModel
    {
        public decimal TotalBookingRevenue { get; set; }
        public IEnumerable<ServiceIncome> ServiceIncome { get; set; }
        public IEnumerable<AgeRangeStatistics> AgeRangeStatistics { get; set; }
        public IEnumerable<CountryGuestPercentage> CountryGuestPercentage { get; set; }
        public RevenuePeriod RevenueForPeriod { get; set; }
        public IEnumerable<RoomTypeBookingPercentage> TopRoomTypes { get; set; } 
    }
}
