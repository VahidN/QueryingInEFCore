using System.Linq;
using System;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ListTheTotalSlotsBookedPerFacilityPerMonthPart2
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/fachoursbymonth3.html
            // Produce a list of the total number of slots booked per facility per month in the year of 2012.
            // In this version, include output rows containing totals for all months per facility,
            // and a total for all months for all facilities. The output table should consist of facility id,
            // month and slots, sorted by the id and month. When calculating the aggregated values for
            // all months and all facids, return null values in the month and facid columns.
            // `rollup` means having The SubTotal() and GrandTotal().
            //
            //select facid, extract(month from starttime) as month, sum(slots) as slots
            //	from cd.bookings
            //	where
            //		starttime >= '2012-01-01'
            //		and starttime < '2013-01-01'
            //	group by rollup(facid, month)
            //order by facid, month;
            //
            // OR
            //
            //select facid, extract(month from starttime) as month, sum(slots) as slots
            //    from cd.bookings
            //    where
            //        starttime >= '2012-01-01'
            //        and starttime < '2013-01-01'
            //    group by facid, month
            //union all
            //select facid, null, sum(slots) as slots
            //    from cd.bookings
            //    where
            //        starttime >= '2012-01-01'
            //        and starttime < '2013-01-01'
            //    group by facid
            //union all
            //select null, null, sum(slots) as slots
            //    from cd.bookings
            //    where
            //        starttime >= '2012-01-01'
            //        and starttime < '2013-01-01'
            //order by facid, month;

            EFServiceProvider.RunInContext(context =>
            {
                var date1 = new DateTime(2012, 01, 01);
                var date2 = new DateTime(2013, 01, 01);

                var facilities = context.Bookings
                                    .Where(booking => booking.StartTime >= date1
                                                        && booking.StartTime < date2)
                                    .GroupBy(booking => new { booking.FacId, booking.StartTime.Month })
                                    .Select(group => new
                                    {
                                        group.Key.FacId,
                                        group.Key.Month,
                                        TotalSlots = group.Sum(booking => booking.Slots)
                                    })
                                    .OrderBy(result => result.FacId)
                                        .ThenBy(result => result.Month)
                                    .ToList()
                            //This is new
                            .GroupByWithRollup(
                                item => item.FacId,
                                item => item.Month,
                                (primaryGrouping, secondaryGrouping) => new
                                {
                                    FacId = primaryGrouping.Key,
                                    Month = secondaryGrouping.Key,
                                    TotalSlots = secondaryGrouping.Sum(item => item.TotalSlots)
                                },
                                item => new
                                {
                                    FacId = item.Key,
                                    Month = -1,
                                    TotalSlots = item.SubTotal(subItem => subItem.TotalSlots)
                                },
                                items => new
                                {
                                    FacId = -1,
                                    Month = -1,
                                    TotalSlots = items.GrandTotal(subItem => subItem.TotalSlots)
                                });
                /*
                    SELECT [b].[FacId], DATEPART(month, [b].[StartTime]) AS [Month], SUM([b].[Slots]) AS [TotalSlots]
                        FROM [Bookings] AS [b]
                        WHERE ([b].[StartTime] >= @__date1_0) AND ([b].[StartTime] < @__date2_1)
                        GROUP BY [b].[FacId], DATEPART(month, [b].[StartTime])
                        ORDER BY [b].[FacId], DATEPART(month, [b].[StartTime])
                        +
                        Client-side evaluation
                */

                var expectedResult = new[]
                {
                    new { FacId = 0, Month =    7, TotalSlots =     270 },
                    new { FacId = 0, Month =    8, TotalSlots =     459 },
                    new { FacId = 0, Month =    9, TotalSlots =     591 },

                    new { FacId = 0, Month =    -1, TotalSlots =     1320 },

                    new { FacId = 1, Month =    7, TotalSlots =     207 },
                    new { FacId = 1, Month =    8, TotalSlots =     483 },
                    new { FacId = 1, Month =    9, TotalSlots =     588 },

                    new { FacId = 1, Month =    -1, TotalSlots =     1278 },

                    new { FacId = 2, Month =    7, TotalSlots =     180 },
                    new { FacId = 2, Month =    8, TotalSlots =     459 },
                    new { FacId = 2, Month =    9, TotalSlots =     570 },

                    new { FacId = 2, Month =    -1, TotalSlots =     1209 },

                    new { FacId = 3, Month =    7, TotalSlots =     104 },
                    new { FacId = 3, Month =    8, TotalSlots =     304 },
                    new { FacId = 3, Month =    9, TotalSlots =     422 },

                    new { FacId = 3, Month =    -1, TotalSlots =     830 },

                    new { FacId = 4, Month =    7, TotalSlots =     264 },
                    new { FacId = 4, Month =    8, TotalSlots =     492 },
                    new { FacId = 4, Month =    9, TotalSlots =     648 },

                    new { FacId = 4, Month =    -1, TotalSlots =     1404 },

                    new { FacId = 5, Month =    7, TotalSlots =     24 },
                    new { FacId = 5, Month =    8, TotalSlots =     82 },
                    new { FacId = 5, Month =    9, TotalSlots =     122 },

                    new { FacId = 5, Month =    -1, TotalSlots =     228 },

                    new { FacId = 6, Month =    7, TotalSlots =     164 },
                    new { FacId = 6, Month =    8, TotalSlots =     400 },
                    new { FacId = 6, Month =    9, TotalSlots =     540 },

                    new { FacId = 6, Month =    -1, TotalSlots =     1104 },

                    new { FacId = 7, Month =    7, TotalSlots =     156 },
                    new { FacId = 7, Month =    8, TotalSlots =     326 },
                    new { FacId = 7, Month =    9, TotalSlots =     426 },

                    new { FacId = 7, Month =    -1, TotalSlots =     908 },

                    new { FacId = 8, Month =    7, TotalSlots =     117 },
                    new { FacId = 8, Month =    8, TotalSlots =     322 },
                    new { FacId = 8, Month =    9, TotalSlots =     471 },

                    new { FacId = 8, Month =    -1, TotalSlots =     910 },
                    new { FacId = -1, Month =    -1, TotalSlots =     9191 }
                };

                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}