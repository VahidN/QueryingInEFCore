using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class ReturnCountOfBookingsForEachMonth
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/bookingspermonth.html
            // Return a count of bookings for each month, sorted by month
            //
            //select date_trunc('month', starttime) as month, count(*)
            //	from cd.bookings
            //	group by month
            //	order by month

            EFServiceProvider.RunInContext(context =>
            {
                var items = context.Bookings
                    .GroupBy(x => new { x.StartTime.Year, x.StartTime.Month })
                    .Select(x => new
                    {
                        x.Key.Year,
                        x.Key.Month,
                        Count = x.Count()
                    })
                    .OrderBy(x => x.Year)
                        .ThenBy(x => x.Month)
                    .ToList();
                /*
                SELECT DATEPART(year, [b].[StartTime]) AS [Year],
                        DATEPART(month, [b].[StartTime]) AS [Month],
                        COUNT(*) AS [Count]
                        FROM [Bookings] AS [b]
                        GROUP BY DATEPART(year, [b].[StartTime]), DATEPART(month, [b].[StartTime])
                        ORDER BY DATEPART(year, [b].[StartTime]), DATEPART(month, [b].[StartTime])
                */
                var expectedResult = new[]
                {
                    new { Year = 2012, Month = 07, Count = 658 } ,
                    new { Year = 2012, Month = 08, Count = 1472 } ,
                    new { Year = 2012, Month = 09, Count = 1913 } ,
                    new { Year = 2013, Month = 01, Count = 1 }
                };
                items.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}