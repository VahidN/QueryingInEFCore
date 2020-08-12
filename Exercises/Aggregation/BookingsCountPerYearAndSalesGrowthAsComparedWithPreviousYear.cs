using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class BookingsCountPerYearAndSalesGrowthAsComparedWithPreviousYear
    {
        [FullyQualifiedTestMethod]
        public void Test_Method1()
        {
            // Using
            // https://github.com/linq2db/linq2db.EntityFrameworkCore
            // https://github.com/linq2db/linq2db/wiki/Window-Functions-(Analytic-Functions)

            EFServiceProvider.RunInContext(context =>
            {
                // Bookings count per year and sales growth as compared with the previous year
                //
                //SELECT [Year],
                //       SoldItemsCount,
                //       ISNULL(LAG(SoldItemsCount) OVER (ORDER BY [Year]), 0) AS PreviousSoldItemsCount,
                //       ISNULL(((SoldItemsCount - LAG(SoldItemsCount) OVER (ORDER BY [Year])) * 100) /
                //                   LAG(SoldItemsCount) OVER (ORDER BY [Year]), 0) AS 'Growth(%)'
                //FROM   (SELECT   YEAR(StartTime) AS [Year],
                //                 COUNT(*) AS SoldItemsCount
                //        FROM     Bookings
                //        GROUP BY YEAR(StartTime)) AS t;

                var salesGrowth = context.Bookings
                                .Select(booking => new { booking.StartTime.Year })
                                .GroupBy(result => result.Year)
                                .Select(group => new
                                {
                                    Year = group.Key,
                                    SoldItemsCount = group.Count(),
                                    PreviousSoldItemsCount = Sql.Ext.Lag((int?)group.Count(), Sql.Nulls.None)
                                                                    .Over()
                                                                    .OrderBy(group.Key)
                                                                    .ToValue() ?? 0
                                })
                                .Select(result => new
                                {
                                    result.Year,
                                    result.SoldItemsCount,
                                    result.PreviousSoldItemsCount,
                                    GrowthPercent =
                                    result.PreviousSoldItemsCount == 0 ? 0 :
                                        (result.SoldItemsCount - result.PreviousSoldItemsCount) * 100 / result.PreviousSoldItemsCount
                                })
                                .ToLinqToDB()
                                .ToList();

                /*
                    SELECT
                        [result_1].[Year_1],
                        Count(*),
                        IIF(LAG(Count(*)) OVER(ORDER BY [result_1].[Year_1]) IS NULL, 0, LAG(Count(*)) OVER(ORDER BY [result_1].[Year_1]))
                    FROM
                        (
                            SELECT
                                DatePart(year, [booking].[StartTime]) as [Year_1]
                            FROM
                                [Bookings] [booking]
                        ) [result_1]
                    GROUP BY
                        [result_1].[Year_1]
                */

                var expectedResult = new[]
                {
                    new { Year = 2012, SoldItemsCount = 4043, PreviousSoldItemsCount = 0, GrowthPercent = 0 },
                    new { Year = 2013, SoldItemsCount = 1, PreviousSoldItemsCount = 4043, GrowthPercent = -99 }
                };

                salesGrowth.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}