using System;
using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.JoinsAndSubqueries
{
    [FullyQualifiedTestClass]
    public class ProduceListOfCostlyBookingsUsingSubquery
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/joins/tjsub.html
            // The Produce a list of costly bookings exercise contained some messy logic:
            // we had to calculate the booking cost in both the WHERE clause and the CASE statement.
            // Try to simplify this calculation using subqueries. For reference, the question was:
            // How can you produce a list of bookings on the day of 2012-09-14 which will cost the member (or guest) more than $30?
            // Remember that guests have different costs to members (the listed costs are per half-hour 'slot'), and the guest user is
            // always ID 0. Include in your output the name of the facility, the name of the member formatted as a single column, and
            // the cost. Order by descending cost, and do not use any subqueries.
            //select member, facility, cost from (
            //	select
            //		mems.firstname || ' ' || mems.surname as member,
            //		facs.name as facility,
            //		case
            //			when mems.memid = 0 then
            //				bks.slots*facs.guestcost
            //			else
            //				bks.slots*facs.membercost
            //		end as cost
            //		from
            //			cd.members mems
            //			inner join cd.bookings bks
            //				on mems.memid = bks.memid
            //			inner join cd.facilities facs
            //				on bks.facid = facs.facid
            //		where
            //			bks.starttime >= '2012-09-14' and
            //			bks.starttime < '2012-09-15'
            //	) as bookings
            //	where cost > 30
            //order by cost desc;

            EFServiceProvider.RunInContext(context =>
            {
                var date1 = new DateTime(2012, 09, 14);
                var date2 = new DateTime(2012, 09, 15);

                var items = context.Members
                        .SelectMany(x => x.Bookings)
                        .Where(booking => booking.StartTime >= date1 && booking.StartTime < date2)
                        .Select(booking => new
                        {
                            Member = booking.Member.FirstName + " " + booking.Member.Surname,
                            Facility = booking.Facility.Name,
                            Cost = booking.MemId == 0 ?
                                        booking.Slots * booking.Facility.GuestCost
                                        : booking.Slots * booking.Facility.MemberCost
                        })
                        .Where(x => x.Cost > 30)
                        .Distinct()
                        .OrderByDescending(x => x.Cost)
                        .ToList();
                /*
                    SELECT [t].[c] AS [Member], [t].[Name] AS [Facility], [t].[c0] AS [Cost]
                    FROM (
                        SELECT DISTINCT ([m0].[FirstName] + N' ') + [m0].[Surname] AS [c], [f].[Name], CASE
                            WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                            ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                        END AS [c0]
                        FROM [Members] AS [m]
                        INNER JOIN [Bookings] AS [b] ON [m].[MemId] = [b].[MemId]
                        INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                        INNER JOIN [Members] AS [m0] ON [b].[MemId] = [m0].[MemId]
                        WHERE (([b].[StartTime] >= @__date1_0) AND ([b].[StartTime] < @__date2_1)) AND (CASE
                            WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                            ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                        END > 30.0)
                    ) AS [t]
                    ORDER BY [t].[c0] DESC
                */
                var expectedResult = new[]
                {
                    new { Member = "GUEST GUEST", Facility ="Massage Room 2", Cost = 320M } ,
                    new { Member = "GUEST GUEST", Facility ="Massage Room 1", Cost = 160M } ,
                    new { Member = "GUEST GUEST", Facility ="Tennis Court 2", Cost = 150M } ,
                    new { Member = "Jemima Farrell", Facility ="Massage Room 1", Cost = 140M } ,
                    new { Member = "GUEST GUEST", Facility ="Tennis Court 1", Cost = 75M } ,
                    new { Member = "GUEST GUEST", Facility ="Tennis Court 2", Cost = 75M } ,
                    new { Member = "Matthew Genting", Facility ="Massage Room 1", Cost = 70M } ,
                    new { Member = "Florence Bader", Facility ="Massage Room 2", Cost = 70M } ,
                    new { Member = "GUEST GUEST", Facility ="Squash Court", Cost = 70.0M } ,
                    new { Member = "Jemima Farrell", Facility ="Massage Room 1", Cost = 70M } ,
                    new { Member = "Ponder Stibbons", Facility ="Massage Room 1", Cost = 70M } ,
                    new { Member = "Burton Tracy", Facility ="Massage Room 1", Cost = 70M } ,
                    new { Member = "Jack Smith", Facility ="Massage Room 1", Cost = 70M } ,
                    new { Member = "GUEST GUEST", Facility ="Squash Court", Cost = 35.0M }
                };

                items.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}