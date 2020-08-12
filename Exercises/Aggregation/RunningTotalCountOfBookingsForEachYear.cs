using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class RunningTotalCountOfBookingsForEachYear
    {
        [FullyQualifiedTestMethod]
        public void Test_Method1()
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

        [FullyQualifiedTestMethod]
        public void Test_Method2()
        {
            // Using
            // https://github.com/linq2db/linq2db.EntityFrameworkCore
            // https://github.com/linq2db/linq2db/wiki/Window-Functions-(Analytic-Functions)

            EFServiceProvider.RunInContext(context =>
            {
                // Running total count of bookings for each year.
                //
                //SELECT DISTINCT YEAR(StartTime) AS [Year],
                //                COUNT(StartTime) OVER (ORDER BY YEAR(StartTime)) AS RunningTotalCountOfBookings
                //FROM   Bookings;

                var runningTotalCountForEachYear = context.Bookings
                                .Select(booking => new
                                {
                                    booking.StartTime.Year,
                                    RunningTotalCount =
                                        Sql.Ext.Count(booking.StartTime)
                                                .Over()
                                                .OrderBy(booking.StartTime.Year)
                                                .ToValue()
                                })
                                .OrderBy(result => result.Year)
                                .Distinct()
                                .ToLinqToDB()
                                .ToList();

                /*
                    SELECT DISTINCT
                        DatePart(year, [booking].[StartTime]),
                        COUNT([booking].[StartTime]) OVER(ORDER BY DatePart(year, [booking].[StartTime]))
                    FROM
                        [Bookings] [booking]
                    ORDER BY
                        DatePart(year, [booking].[StartTime])
                */

                var expectedResult = new[]
                {
                    new { Year = 2012, RunningTotalCount = 4043 },
                    new { Year = 2013, RunningTotalCount = 4044 }
                };

                runningTotalCountForEachYear.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}