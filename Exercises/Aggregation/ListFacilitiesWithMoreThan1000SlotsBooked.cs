using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ListFacilitiesWithMoreThan1000SlotsBooked
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/fachours1a.html
            // Produce a list of facilities with more than 1000 slots booked.
            // Produce an output table consisting of facility id and hours, sorted by facility id.
            // select facid, sum(slots) as "Total Slots"
            // from cd.bookings
            // group by facid
            // having sum(slots) > 1000
            // order by facid

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Bookings
                                    .GroupBy(booking => booking.FacId)
                                    .Select(group => new
                                    {
                                        FacId = group.Key,
                                        TotalSlots = group.Sum(booking => booking.Slots)
                                    })
                                    .Where(result => result.TotalSlots > 1000)
                                    .OrderBy(result => result.FacId)
                                    .ToList();
                /*
                    SELECT [b].[FacId], SUM([b].[Slots]) AS [TotalSlots]
                        FROM [Bookings] AS [b]
                        GROUP BY [b].[FacId]
                        HAVING SUM([b].[Slots]) > 1000
                        ORDER BY [b].[FacId]
                */
                var expectedResult = new[]
                {
                    new { FacId = 0, TotalSlots =   1320 },
                    new { FacId = 1, TotalSlots =   1278 },
                    new { FacId = 2, TotalSlots =   1209 },
                    new { FacId = 4, TotalSlots =   1404 },
                    new { FacId = 6, TotalSlots =   1104 }
                };

                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}