using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class OutputTheFacilityIdThatHasTheHighestNumberOfSlotsBooked
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/fachours2.html
            // Output the facility id that has the highest number of slots booked.
            //select facid, sum(slots) as "Total Slots"
            //	from cd.bookings
            //	group by facid
            //order by sum(slots) desc
            //LIMIT 1;

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