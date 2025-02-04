namespace HotelManagement.Models.StatisticsModels
{
    public class RevenuePeriod
    {
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }   
        public decimal TotalRevenue { get; set; }   
    }
}
