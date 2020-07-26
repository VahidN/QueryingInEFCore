using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class FindFacilitiesWithTotalRevenueLessThan1000
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/facrev2.html
            // Produce a list of facilities with a total revenue less than 1000.
            // Produce an output table consisting of facility name and revenue, sorted by revenue.
            // Remember that there's a different cost for guests and members!
            //select name, revenue from (
            //	select facs.name, sum(case
            //				when memid = 0 then slots * facs.guestcost
            //				else slots * membercost
            //			end) as revenue
            //		from cd.bookings bks
            //		inner join cd.facilities facs
            //			on bks.facid = facs.facid
            //		group by facs.name
            //	) as agg where revenue < 1000
            //order by revenue;

            EFServiceProvider.RunInContext(context =>
            {
                var facilities =
                            context.Bookings.Select(booking =>
                                new
                                {
                                    booking.Facility.Name,
                                    Revenue = booking.MemId == 0 ?
                                            booking.Slots * booking.Facility.GuestCost
                                            : booking.Slots * booking.Facility.MemberCost
                                })
                                .GroupBy(b => b.Name)
                                .Select(group => new
                                {
                                    Name = group.Key,
                                    TotalRevenue = group.Sum(b => b.Revenue)
                                })
                                .Where(result => result.TotalRevenue < 1000)
                                .OrderBy(result => result.TotalRevenue)
                                .ToList();
                /*
                    SELECT [f].[Name], SUM(CASE
                            WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                            ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                        END) AS [TotalRevenue]
                        FROM [Bookings] AS [b]
                        INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                        GROUP BY [f].[Name]
                        HAVING SUM(CASE
                            WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                            ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                        END) < 1000.0
                        ORDER BY SUM(CASE
                            WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                            ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                        END)
                */
                var expectedResult = new[]
                {
                    new { Name = "Table Tennis", TotalRevenue =     180M },
                    new { Name = "Snooker Table", TotalRevenue =    240M },
                    new { Name = "Pool Table", TotalRevenue =   270M }
                };

                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}