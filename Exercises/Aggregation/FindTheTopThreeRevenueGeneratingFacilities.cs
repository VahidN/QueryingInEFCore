using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public partial class FindTheTopThreeRevenueGeneratingFacilities
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/facrev3.html
            // Produce a list of the top three revenue generating facilities (including ties).
            // Output facility name and rank, sorted by rank and facility name.
            //
            //select name, rank from (
            //	select facs.name as name, rank() over (order by sum(case
            //				when memid = 0 then slots * facs.guestcost
            //				else slots * membercost
            //			end) desc) as rank
            //		from cd.bookings bks
            //		inner join cd.facilities facs
            //			on bks.facid = facs.facid
            //		group by facs.name
            //	) as subq
            //	where rank <= 3
            //order by rank;

            EFServiceProvider.RunInContext(context =>
            {
                var facilitiesQuery =
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
                                .OrderBy(result => result.TotalRevenue);

                var rankedFacilities = facilitiesQuery.Select(thisItem => new
                {
                    thisItem.Name,
                    thisItem.TotalRevenue,
                    Rank = facilitiesQuery.Count(mainItem => mainItem.TotalRevenue > thisItem.TotalRevenue) + 1
                })
                .Where(result => result.Rank <= 3)
                .OrderBy(result => result.Rank)
                .ToList();
                /*
                SELECT [f0].[Name], SUM(CASE
                    WHEN [b0].[MemId] = 0 THEN CAST([b0].[Slots] AS decimal(18,6)) * [f0].[GuestCost]
                    ELSE CAST([b0].[Slots] AS decimal(18,6)) * [f0].[MemberCost]
                END) AS [TotalRevenue], (
                    SELECT COUNT(*)
                    FROM (
                        SELECT [f].[Name], SUM(CASE
                            WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                            ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                        END) AS [c]
                        FROM [Bookings] AS [b]
                        INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                        GROUP BY [f].[Name]
                    ) AS [t]
                    WHERE [t].[c] > SUM(CASE
                        WHEN [b0].[MemId] = 0 THEN CAST([b0].[Slots] AS decimal(18,6)) * [f0].[GuestCost]
                        ELSE CAST([b0].[Slots] AS decimal(18,6)) * [f0].[MemberCost]
                    END)) + 1 AS [Rank]
                FROM [Bookings] AS [b0]
                INNER JOIN [Facilities] AS [f0] ON [b0].[FacId] = [f0].[FacId]
                GROUP BY [f0].[Name]
                HAVING ((
                    SELECT COUNT(*)
                    FROM (
                        SELECT [f1].[Name], SUM(CASE
                            WHEN [b1].[MemId] = 0 THEN CAST([b1].[Slots] AS decimal(18,6)) * [f1].[GuestCost]
                            ELSE CAST([b1].[Slots] AS decimal(18,6)) * [f1].[MemberCost]
                        END) AS [c]
                        FROM [Bookings] AS [b1]
                        INNER JOIN [Facilities] AS [f1] ON [b1].[FacId] = [f1].[FacId]
                        GROUP BY [f1].[Name]
                    ) AS [t0]
                    WHERE [t0].[c] > SUM(CASE
                        WHEN [b0].[MemId] = 0 THEN CAST([b0].[Slots] AS decimal(18,6)) * [f0].[GuestCost]
                        ELSE CAST([b0].[Slots] AS decimal(18,6)) * [f0].[MemberCost]
                    END)) + 1) <= 3
                ORDER BY (
                    SELECT COUNT(*)
                    FROM (
                        SELECT [f2].[Name], SUM(CASE
                            WHEN [b2].[MemId] = 0 THEN CAST([b2].[Slots] AS decimal(18,6)) * [f2].[GuestCost]
                            ELSE CAST([b2].[Slots] AS decimal(18,6)) * [f2].[MemberCost]
                        END) AS [c]
                        FROM [Bookings] AS [b2]
                        INNER JOIN [Facilities] AS [f2] ON [b2].[FacId] = [f2].[FacId]
                        GROUP BY [f2].[Name]
                    ) AS [t1]
                    WHERE [t1].[c] > SUM(CASE
                        WHEN [b0].[MemId] = 0 THEN CAST([b0].[Slots] AS decimal(18,6)) * [f0].[GuestCost]
                        ELSE CAST([b0].[Slots] AS decimal(18,6)) * [f0].[MemberCost]
                    END)) + 1
                */
                var expectedResult = new[]
                {
                    new { Name = "Massage Room 1", TotalRevenue = 72540.000000000000M, Rank = 1 } ,
                    new { Name = "Massage Room 2", TotalRevenue = 15810.000000000000M, Rank = 2 } ,
                    new { Name = "Tennis Court 2", TotalRevenue = 14310.000000000000M, Rank = 3 }
                };

                rankedFacilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}