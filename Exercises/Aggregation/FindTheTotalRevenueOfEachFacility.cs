using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class FindTheTotalRevenueOfEachFacility
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/facrev.html
            // Produce a list of facilities along with their total revenue.
            // The output table should consist of facility name and revenue, sorted by revenue.
            // Remember that there's a different cost for guests and members!
            //select facs.name, sum(slots * case
            //			when memid = 0 then facs.guestcost
            //			else facs.membercost
            //		end) as revenue
            //	from cd.bookings bks
            //	inner join cd.facilities facs
            //		on bks.facid = facs.facid
            //	group by facs.name
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
                        ORDER BY SUM(CASE
                            WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                            ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                        END)
                */

                var expectedResult = new[]
                {
                    new { Name = "Table Tennis", TotalRevenue =     180M },
                    new { Name = "Snooker Table", TotalRevenue =    240M },
                    new { Name = "Pool Table", TotalRevenue =   270M },
                    new { Name = "Badminton Court", TotalRevenue =  1906.5M },
                    new { Name = "Squash Court", TotalRevenue =     13468.0M },
                    new { Name = "Tennis Court 1", TotalRevenue =   13860M },
                    new { Name = "Tennis Court 2", TotalRevenue =   14310M },
                    new { Name = "Massage Room 2", TotalRevenue =   15810M },
                    new { Name = "Massage Room 1", TotalRevenue =   72540M }
                };

                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}