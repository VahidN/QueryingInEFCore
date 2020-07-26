using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ClassifyFacilitiesByValue
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/classify.html
            // Classify facilities into equally sized groups of high, average, and low
            // based on their revenue. Order by classification and facility name.
            //
            //select name, case when class=1 then 'high'
            //		when class=2 then 'average'
            //		else 'low'
            //		end revenue
            //	from (
            //		select facs.name as name, ntile(3) over (order by sum(case
            //				when memid = 0 then slots * facs.guestcost
            //				else slots * membercost
            //			end) desc) as class
            //		from cd.bookings bks
            //		inner join cd.facilities facs
            //			on bks.facid = facs.facid
            //		group by facs.name
            //	) as subq
            //order by class, name;

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
                                .OrderByDescending(result => result.TotalRevenue)
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
                        END) DESC
                */
                // Then using LINQ to Objects
                var n = 3;
                var tiledFacilities = facilities.Select((item, index) =>
                                        new
                                        {
                                            Item = item,
                                            Index = (index / n) + 1
                                        })
                                        .GroupBy(x => x.Index)
                                        .Select(g =>
                                            g.Select(z =>
                                                new
                                                {
                                                    z.Item.Name,
                                                    z.Item.TotalRevenue,
                                                    Tile = g.Key,
                                                    GroupName = g.Key == 1 ? "High" : (g.Key == 2 ? "Average" : "Low")
                                                })
                                                .OrderBy(x => x.GroupName)
                                                    .ThenBy(x => x.Name)
                                        )
                                        .ToList();

                var flatTiledFacilities = tiledFacilities.SelectMany(group => group)
                                        .Select(tile => new { tile.Name, Revenue = tile.GroupName })
                                        .ToList();

                var expectedResult = new[]
                {
                    new { Name = "Massage Room 1", Revenue= "High" },
                    new { Name = "Massage Room 2", Revenue= "High" },
                    new { Name = "Tennis Court 2", Revenue= "High" },
                    new { Name = "Badminton Court", Revenue= "Average" },
                    new { Name = "Squash Court", Revenue= "Average" },
                    new { Name = "Tennis Court 1", Revenue= "Average" },
                    new { Name = "Pool Table", Revenue= "Low" },
                    new { Name = "Snooker Table", Revenue= "Low" },
                    new { Name = "Table Tennis", Revenue= "Low" }
                };

                flatTiledFacilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}