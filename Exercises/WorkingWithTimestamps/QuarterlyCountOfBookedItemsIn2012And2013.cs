using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class QuarterlyCountOfBookedItemsIn2012And2013
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            EFServiceProvider.RunInContext(context =>
            {
                // Quarterly count of booked items in 2012 and 2013.
                //
                //SELECT   YEAR(StartTime) AS [Year],
                //         SUM(CASE WHEN DATEPART(QUARTER, StartTime) = 1 THEN 1 ELSE 0 END) AS FirstQuarter,
                //         SUM(CASE WHEN DATEPART(QUARTER, StartTime) = 2 THEN 1 ELSE 0 END) AS SecondQuarter,
                //         SUM(CASE WHEN DATEPART(QUARTER, StartTime) = 3 THEN 1 ELSE 0 END) AS ThirdQuarter,
                //         SUM(CASE WHEN DATEPART(QUARTER, StartTime) = 4 THEN 1 ELSE 0 END) AS ForthQuarter
                //FROM     Bookings
                //WHERE    YEAR(StartTime) BETWEEN 2012 AND 2013
                //GROUP BY YEAR(StartTime);

                var quarterlyCountOfBookedItems = context.Bookings
                                .Select(booking => new
                                {
                                    booking.StartTime.Year,
                                    Quarter = SqlDbFunctionsExtensions.SqlDatePart(SqlDatePart.Quarter, booking.StartTime)
                                })
                                .Where(result => result.Year >= 2012 && result.Year <= 2013)
                                .GroupBy(result => new
                                {
                                    result.Year
                                })
                                .Select(group => new
                                {
                                    group.Key.Year,
                                    FirstQuarter = group.Sum(result => result.Quarter == 1 ? 1 : 0),
                                    SecondQuarter = group.Sum(result => result.Quarter == 2 ? 1 : 0),
                                    ThirdQuarter = group.Sum(result => result.Quarter == 3 ? 1 : 0),
                                    ForthQuarter = group.Sum(result => result.Quarter == 4 ? 1 : 0),
                                })
                                .OrderBy(result => result.Year)
                                .ToList();
                /*
                    SELECT  DATEPART(year, [b].[StartTime]) AS [Year],
                            SUM(CASE WHEN DATEPART(Quarter, [b].[StartTime]) = 1 THEN 1 ELSE 0 END) AS [FirstQuarter],
                            SUM(CASE WHEN DATEPART(Quarter, [b].[StartTime]) = 2 THEN 1 ELSE 0 END) AS [SecondQuarter],
                            SUM(CASE WHEN DATEPART(Quarter, [b].[StartTime]) = 3 THEN 1 ELSE 0 END) AS [ThirdQuarter],
                            SUM(CASE WHEN DATEPART(Quarter, [b].[StartTime]) = 4 THEN 1 ELSE 0 END) AS [ForthQuarter]
                    FROM     [Bookings] AS [b]
                    WHERE    (DATEPART(year, [b].[StartTime]) >= 2012)
                            AND (DATEPART(year, [b].[StartTime]) <= 2013)
                    GROUP BY DATEPART(year, [b].[StartTime])
                    ORDER BY DATEPART(year, [b].[StartTime]);
                */

                var expectedResult = new[]
                {
                    new { Year = 2012 ,  FirstQuarter = 0, SecondQuarter = 0, ThirdQuarter = 4043 , ForthQuarter = 0},
                    new { Year = 2013 ,  FirstQuarter = 1, SecondQuarter = 0, ThirdQuarter = 0 , ForthQuarter = 0}
                };

                quarterlyCountOfBookedItems.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}