using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class RunningTotalCountOfBookingsForEachYear
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            EFServiceProvider.RunInContext(context =>
            {
                // Running total count of bookings for each year.
                //
                //SELECT DISTINCT YEAR(StartTime) AS [Year],
                //                COUNT(StartTime) OVER (ORDER BY YEAR(StartTime)) AS RunningTotalCountOfBookings
                //FROM   Bookings;

                var bookingsCountForEachYear = context.Bookings
                                .Select(booking => new { booking.StartTime.Year })
                                .GroupBy(result => result.Year)
                                .Select(group => new
                                {
                                    Year = group.Key,
                                    SoldItemsCount = group.Count()
                                })
                                .OrderBy(result => result.Year)
                                .ToList();
                /*
                    SELECT DATEPART(year, [b].[StartTime]) AS [Year], COUNT(*) AS [SoldItemsCount]
                        FROM [Bookings] AS [b]
                        GROUP BY DATEPART(year, [b].[StartTime])
                        ORDER BY DATEPART(year, [b].[StartTime])
                */

                // Now using LINQ to Objects
                var runningTotal = 0;
                var runningTotalCountForEachYear = bookingsCountForEachYear
                                        .Select(bookingsCount => new
                                        {
                                            bookingsCount.Year,
                                            bookingsCount.SoldItemsCount,
                                            RunningTotalCount = runningTotal += bookingsCount.SoldItemsCount
                                        })
                                        .ToList();

                var expectedResult = new[]
                {
                    new { Year = 2012, SoldItemsCount = 4043 ,  RunningTotalCount = 4043 },
                    new { Year = 2013, SoldItemsCount = 1 ,  RunningTotalCount = 4044 }
                };

                runningTotalCountForEachYear.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}