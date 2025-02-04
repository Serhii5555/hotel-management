namespace HotelManagement.Models.StatisticsModels
{
    public class CountryGuestPercentage
    {
        public string nationality { get; set; }
        public int guest_count { get; set; }
        public decimal percentage { get; set; }
    }
}
