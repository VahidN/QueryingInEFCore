using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ListTheTotalSlotsBookedPerFacility
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/fachours.html
            // Produce a list of the total number of slots booked per facility.
            // For now, just produce an output table consisting of facility id and slots, sorted by facility id.
            //select facid, sum(slots) as "Total Slots"
            //	from cd.bookings
            //	group by facid
            //order by facid;

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Bookings
                                    .GroupBy(booking => booking.FacId)
                                    .Select(group => new
                                    {
                                        FacId = group.Key,
                                        TotalSlots = group.Sum(booking => booking.Slots)
                                    })
                                    .OrderBy(result => result.FacId)
                                    .ToList();
                /*
                    SELECT [b].[FacId], SUM([b].[Slots]) AS [TotalSlots]
                        FROM [Bookings] AS [b]
                        GROUP BY [b].[FacId]
                        ORDER BY [b].[FacId]
                */

                var expectedResult = new[]
                {
                    new { FacId = 0, TotalSlots= 1320 },
                    new { FacId = 1, TotalSlots=    1278 },
                    new { FacId = 2, TotalSlots=    1209 },
                    new { FacId = 3, TotalSlots=    830 },
                    new { FacId = 4, TotalSlots=    1404 },
                    new { FacId = 5, TotalSlots=    228 },
                    new { FacId = 6, TotalSlots=    1104 },
                    new { FacId = 7, TotalSlots=    908 },
                    new { FacId = 8, TotalSlots=    911 }
                };

                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}