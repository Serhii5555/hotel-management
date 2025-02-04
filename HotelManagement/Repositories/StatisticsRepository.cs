using HotelManagement.Models;
using HotelManagement.Repositories.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagement.Models.StatisticsModels;

namespace HotelManagement.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly DatabaseContext _database;

        public StatisticsRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<decimal> GetTotalBookingRevenueAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT SUM(total_price) AS total_booking_revenue
                    FROM Bookings";

                var result = await connection.QuerySingleOrDefaultAsync<decimal?>(query);
                return result ?? 0m;
            }
        }

        public async Task<decimal> GetRevenueForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT SUM(total_price) AS total_revenue
                    FROM Bookings
                    WHERE check_in_date >= @StartDate AND check_out_date <= @EndDate";

                var result = await connection.QuerySingleOrDefaultAsync<decimal?>(query, new { StartDate = startDate, EndDate = endDate });
                return result ?? 0m;
            }
        }

        public async Task<IEnumerable<ServiceIncome>> GetTop3ServicesByIncomeAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT TOP 3 
                           s.service_name, 
                           COALESCE(SUM(ps.service_amount), 0) AS total_service_income
                    FROM Payment_Services ps
                    JOIN Services s ON ps.service_id = s.service_id
                    GROUP BY s.service_name
                    ORDER BY total_service_income DESC";

                return await connection.QueryAsync<ServiceIncome>(query);
            }
        }

        public async Task<IEnumerable<AgeRangeStatistics>> GetGuestCountByAgeRangeAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT 
                        CASE 
                            WHEN DATEDIFF(YEAR, g.date_of_birth, GETDATE()) < 20 THEN 'Under 20' 
                            WHEN DATEDIFF(YEAR, g.date_of_birth, GETDATE()) BETWEEN 21 AND 30 THEN '21-30' 
                            WHEN DATEDIFF(YEAR, g.date_of_birth, GETDATE()) BETWEEN 31 AND 40 THEN '31-40' 
                            WHEN DATEDIFF(YEAR, g.date_of_birth, GETDATE()) BETWEEN 41 AND 50 THEN '41-50' 
                            ELSE 'Above 50' 
                        END AS age_range, 
                        COUNT(g.guest_id) AS guest_count 
                    FROM Guests g
                    GROUP BY 
                        CASE 
                            WHEN DATEDIFF(YEAR, g.date_of_birth, GETDATE()) < 20 THEN 'Under 20' 
                            WHEN DATEDIFF(YEAR, g.date_of_birth, GETDATE()) BETWEEN 21 AND 30 THEN '21-30' 
                            WHEN DATEDIFF(YEAR, g.date_of_birth, GETDATE()) BETWEEN 31 AND 40 THEN '31-40' 
                            WHEN DATEDIFF(YEAR, g.date_of_birth, GETDATE()) BETWEEN 41 AND 50 THEN '41-50' 
                            ELSE 'Above 50' 
                        END
                    ORDER BY guest_count DESC";

                var result = await connection.QueryAsync<AgeRangeStatistics>(query);

                var sortedResult = result.OrderBy(a => GetAgeRangeOrder(a.age_range)).ToList();
                return sortedResult;
            }
        }

        public async Task<IEnumerable<CountryGuestPercentage>> GetGuestPercentageByCountryAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    SELECT 
                        g.nationality,
                        COUNT(g.guest_id) AS guest_count,
                        (COUNT(g.guest_id) * 100.0 / (SELECT COUNT(*) FROM Guests)) AS percentage
                    FROM Guests g
                    GROUP BY g.nationality
                    ORDER BY guest_count DESC";

                return await connection.QueryAsync<CountryGuestPercentage>(query);
            }
        }

        public async Task<IEnumerable<RoomTypeBookingPercentage>> GetTop3RoomTypesByBookingsAsync()
        {
            using (var connection = _database.CreateConnection())
            {
                var query = @"
                    WITH RoomBookingCounts AS (
                        SELECT 
                            rt.name AS room_type,
                            COUNT(b.booking_id) AS booking_count,
                            COUNT(b.booking_id) * 100.0 / (SELECT COUNT(*) FROM Bookings) AS booking_percentage
                        FROM Room_Types rt
                        LEFT JOIN Rooms r ON rt.room_type_id = r.room_type_id
                        LEFT JOIN Bookings b ON r.room_id = b.room_id
                        GROUP BY rt.name
                    )
                    SELECT TOP 3 room_type, booking_percentage
                    FROM RoomBookingCounts
                    ORDER BY booking_percentage DESC";

                return await connection.QueryAsync<RoomTypeBookingPercentage>(query);
            }
        }

        private int GetAgeRangeOrder(string ageRange)
        {
            switch (ageRange)
            {
                case "Under 20":
                    return 1;
                case "21-30":
                    return 2;
                case "31-40":
                    return 3;
                case "41-50":
                    return 4;
                case "Above 50":
                    return 5;
                default:
                    return int.MaxValue;
            }
        }
    }
}
