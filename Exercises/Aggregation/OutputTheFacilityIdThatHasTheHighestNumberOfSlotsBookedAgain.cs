using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class OutputTheFacilityIdThatHasTheHighestNumberOfSlotsBookedAgain
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/fachours4.html
            // Output the facility id that has the highest number of slots booked.
            // Ensure that in the event of a tie, all tieing results get output.
            //select facid, total from (
            //	select facid, sum(slots) total, rank() over (order by sum(slots) desc) rank
            //        	from cd.bookings
            //		group by facid
            //	) as ranked
            //	where rank = 1
            //
            //
            //select facid, sum(slots) as totalslots
            //	from cd.bookings
            //	group by facid
            //	having sum(slots) = (select max(sum2.totalslots) from
            //		(select sum(slots) as totalslots
            //		from cd.bookings
            //		group by facid
            //		) as sum2);
            //
            //
            //select facid, total from (
            //	select facid, total, rank() over (order by total desc) rank from (
            //		select facid, sum(slots) total
            //			from cd.bookings
            //			group by facid
            //		) as sumslots
            //	) as ranked
            //where rank = 1

            EFServiceProvider.RunInContext(context =>
            {
                var item = context.Bookings
                                    .GroupBy(booking => booking.FacId)
                                    .Select(group => new
                                    {
                                        FacId = group.Key,
                                        TotalSlots = group.Sum(booking => booking.Slots)
                                    })
                                    .OrderByDescending(result => result.TotalSlots)
                                    .FirstOrDefault();
                /*
                    SELECT TOP(1) [b].[FacId], SUM([b].[Slots]) AS [TotalSlots]
                    FROM [Bookings] AS [b]
                    GROUP BY [b].[FacId]
                    ORDER BY SUM([b].[Slots]) DESC
                */
                var expectedResult = new { FacId = 4, TotalSlots = 1404 };
                item.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}