using System.Linq;
using System;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ListTheTotalSlotsBookedPerFacilityInGivenMonth
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/fachoursbymonth.html
            // Produce a list of the total number of slots booked per facility in the month of September 2012.
            // Produce an output table consisting of facility id and slots, sorted by the number of slots.
            //select facid, sum(slots) as "Total Slots"
            //	from cd.bookings
            //	where
            //		starttime >= '2012-09-01'
            //		and starttime < '2012-10-01'
            //	group by facid
            //order by sum(slots);

            EFServiceProvider.RunInContext(context =>
            {
                var date1 = new DateTime(2012, 09, 01);
                var date2 = new DateTime(2012, 10, 01);

                var facilities = context.Bookings
                                    .Where(booking => booking.StartTime >= date1
                                                        && booking.StartTime < date2)
                                    .GroupBy(booking => booking.FacId)
                                    .Select(group => new
                                    {
                                        FacId = group.Key,
                                        TotalSlots = group.Sum(booking => booking.Slots)
                                    })
                                    .OrderBy(result => result.TotalSlots)
                                    .ToList();
                /*
                    SELECT [b].[FacId], SUM([b].[Slots]) AS [TotalSlots]
                    FROM [Bookings] AS [b]
                    WHERE ([b].[StartTime] >= @__date1_0) AND ([b].[StartTime] < @__date2_1)
                    GROUP BY [b].[FacId]
                    ORDER BY SUM([b].[Slots])
                */

                var expectedResult = new[]
                {
                    new { FacId = 5, TotalSlots =   122 },
                    new { FacId = 3, TotalSlots =   422 },
                    new { FacId = 7, TotalSlots =   426 },
                    new { FacId = 8, TotalSlots =   471 },
                    new { FacId = 6, TotalSlots =   540 },
                    new { FacId = 2, TotalSlots =   570 },
                    new { FacId = 1, TotalSlots =   588 },
                    new { FacId = 0, TotalSlots =   591 },
                    new { FacId = 4, TotalSlots =   648 }
                };

                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}