using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class WorkoutTheUtilisationPercentageForEachFacilityByMonth
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/utilisationpermonth.html
            // Work out the utilisation percentage for each facility by month, sorted by name and month,
            // rounded to 1 decimal place. Opening time is 8am, closing time is 8.30pm. You can treat
            // every month as a full month, regardless of if there were some dates the club was not open.
            //
            //select name, month,
            //	round((100*slots)/
            //		cast(
            //			25*(cast((month + interval '1 month') as date)
            //			- cast (month as date)) as numeric),1) as utilisation
            //	from  (
            //		select facs.name as name, date_trunc('month', starttime) as month, sum(slots) as slots
            //			from cd.bookings bks
            //			inner join cd.facilities facs
            //				on bks.facid = facs.facid
            //			group by facs.facid, month
            //	) as inn
            //order by name, month

            EFServiceProvider.RunInContext(context =>
            {
                var items = context.Bookings
                    .Select(booking => new
                    {
                        booking.Facility.Name,
                        booking.StartTime.Year,
                        booking.StartTime.Month,
                        booking.Slots,
                        DaysInMonth = EF.Functions.DateDiffDay(
                                        booking.StartTime.Date.AddDays(1 - booking.StartTime.Date.Day),
                                        booking.StartTime.Date.AddDays(1 - booking.StartTime.Date.Day).AddMonths(1)
                                        )
                    })
                    .GroupBy(b => new { b.Name, b.Year, b.Month, b.DaysInMonth })
                    .Select(g => new
                    {
                        g.Key.Name,
                        g.Key.Year,
                        g.Key.Month,
                        Utilization = SqlDbFunctionsExtensions.SqlRound(
                                100 * g.Sum(b => b.Slots) / (decimal)(25 * g.Key.DaysInMonth),
                                1)
                    })
                    .OrderBy(r => r.Name)
                        .ThenBy(r => r.Year)
                            .ThenBy(r => r.Month)
                    .ToList();
                /*
                    SELECT [f].[Name],
                            DATEPART(year, [b].[StartTime]) AS [Year],
                            DATEPART(month, [b].[StartTime]) AS [Month],
                            ROUND(CAST((100 * SUM([b].[Slots])) AS decimal(18,2))
                                / CAST((25 * DATEDIFF(DAY,
                                DATEADD(day, CAST(CAST((1 - DATEPART(day, CONVERT(date, [b].[StartTime]))) AS float) AS int),
                                CONVERT(date, [b].[StartTime])),
                                DATEADD(month, CAST(1 AS int),
                                DATEADD(day, CAST(CAST((1 - DATEPART(day, CONVERT(date, [b].[StartTime]))) AS float) AS int),
                                CONVERT(date, [b].[StartTime]))))) AS decimal(18,2)), 1) AS [Utilization]
                        FROM [Bookings] AS [b]
                        INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                        GROUP BY [f].[Name],
                        DATEPART(year, [b].[StartTime]),
                        DATEPART(month, [b].[StartTime]),
                        DATEDIFF(DAY, DATEADD(day, CAST(CAST((1 - DATEPART(day, CONVERT(date, [b].[StartTime]))) AS float) AS int), CONVERT(date, [b].[StartTime])), DATEADD(month, CAST(1 AS int), DATEADD(day, CAST(CAST((1 - DATEPART(day, CONVERT(date, [b].[StartTime]))) AS float) AS int), CONVERT(date, [b].[StartTime]))))
                        ORDER BY [f].[Name], DATEPART(year, [b].[StartTime]), DATEPART(month, [b].[StartTime])
                */
                var expectedResult = new[]
                {
                    new { Name = "Badminton Court", Year = 2012, Month = 7, Utilization = 23.2M },
                    new { Name = "Badminton Court", Year = 2012, Month = 8, Utilization = 59.2M },
                    new { Name = "Badminton Court", Year = 2012, Month = 9, Utilization = 76.0M },
                    new { Name = "Massage Room 1", Year = 2012, Month = 7, Utilization = 34.1M },
                    new { Name = "Massage Room 1", Year = 2012, Month = 8, Utilization = 63.5M },
                    new { Name = "Massage Room 1", Year = 2012, Month = 9, Utilization = 86.4M },
                    new { Name = "Massage Room 2", Year = 2012, Month = 7, Utilization = 3.1M },
                    new { Name = "Massage Room 2", Year = 2012, Month = 8, Utilization = 10.6M },
                    new { Name = "Massage Room 2", Year = 2012, Month = 9, Utilization = 16.3M },
                    new { Name = "Pool Table", Year = 2012, Month = 7, Utilization = 15.1M },
                    new { Name = "Pool Table", Year = 2012, Month = 8, Utilization = 41.5M },
                    new { Name = "Pool Table", Year = 2012, Month = 9, Utilization = 62.8M },
                    new { Name = "Pool Table", Year = 2013, Month = 1, Utilization = 0.1M },
                    new { Name = "Snooker Table", Year = 2012, Month = 7, Utilization = 20.1M },
                    new { Name = "Snooker Table", Year = 2012, Month = 8, Utilization = 42.1M },
                    new { Name = "Snooker Table", Year = 2012, Month = 9, Utilization = 56.8M },
                    new { Name = "Squash Court", Year = 2012, Month = 7, Utilization = 21.2M },
                    new { Name = "Squash Court", Year = 2012, Month = 8, Utilization = 51.6M },
                    new { Name = "Squash Court", Year = 2012, Month = 9, Utilization = 72.0M },
                    new { Name = "Table Tennis", Year = 2012, Month = 7, Utilization = 13.4M },
                    new { Name = "Table Tennis", Year = 2012, Month = 8, Utilization = 39.2M },
                    new { Name = "Table Tennis", Year = 2012, Month = 9, Utilization = 56.3M },
                    new { Name = "Tennis Court 1", Year = 2012, Month = 7, Utilization = 34.8M },
                    new { Name = "Tennis Court 1", Year = 2012, Month = 8, Utilization = 59.2M },
                    new { Name = "Tennis Court 1", Year = 2012, Month = 9, Utilization = 78.8M },
                    new { Name = "Tennis Court 2", Year = 2012, Month = 7, Utilization = 26.7M },
                    new { Name = "Tennis Court 2", Year = 2012, Month = 8, Utilization = 62.3M },
                    new { Name = "Tennis Court 2", Year = 2012, Month = 9, Utilization = 78.4M }
                };

                items.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}