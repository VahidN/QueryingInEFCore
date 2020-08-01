using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class WorkoutTheNumberOfDaysInEachMonthOf2012
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/daysinmonth.html
            // For each month of the year in 2012, output the number of days in that month.
            // Format the output as an integer column containing the month of the year, and a
            // second column containing an interval data type.
            //
            //select 	extract(month from cal.month) as month,
            //	(cal.month + interval '1 month') - cal.month as length
            //	from
            //	(
            //		select generate_series(timestamp '2012-01-01', timestamp '2012-12-01', interval '1 month') as month
            //	) cal
            //order by month;

            EFServiceProvider.RunInContext(context =>
            {
                var items = context.Bookings
                    .Where(booking => booking.StartTime.Year == 2012)
                    .Select(booking => new
                    {
                        booking.StartTime.Year,
                        booking.StartTime.Month,
                        DaysInMonth = EF.Functions.DateDiffDay(
                                        booking.StartTime.Date.AddDays(1 - booking.StartTime.Date.Day),
                                        booking.StartTime.Date.AddDays(1 - booking.StartTime.Date.Day).AddMonths(1)
                                        )
                    })
                    .Distinct()
                    .OrderBy(r => r.Year)
                        .ThenBy(r => r.Month)
                    .ToList();
                /*
                    SELECT   [t].[c] AS [Year],
                            [t].[c0] AS [Month],
                            [t].[c1] AS [DaysInMonth]
                    FROM     (SELECT DISTINCT DATEPART(year, [b].[StartTime]) AS [c],
                                            DATEPART(month, [b].[StartTime]) AS [c0],
                                            DATEDIFF(DAY, DATEADD(day, CAST (CAST ((1 - DATEPART(day, CONVERT (DATE, [b].[StartTime]))) AS FLOAT) AS INT), CONVERT (DATE, [b].[StartTime])), DATEADD(month, CAST (1 AS INT), DATEADD(day, CAST (CAST ((1 - DATEPART(day, CONVERT (DATE, [b].[StartTime]))) AS FLOAT) AS INT), CONVERT (DATE, [b].[StartTime])))) AS [c1]
                            FROM   [Bookings] AS [b]
                            WHERE  DATEPART(year, [b].[StartTime]) = 2012) AS [t]
                    ORDER BY [t].[c], [t].[c0];
                */

                var expectedResult = new[]
                {
                    new { Year = 2012, Month = 7, DaysInMonth = 31 },
                    new { Year = 2012, Month = 8, DaysInMonth = 31 },
                    new { Year = 2012, Month = 9, DaysInMonth = 30 }
                };
                items.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}