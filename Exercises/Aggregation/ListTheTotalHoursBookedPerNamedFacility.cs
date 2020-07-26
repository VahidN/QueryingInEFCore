using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ListTheTotalHoursBookedPerNamedFacility
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/fachours3.html
            // Produce a list of the total number of hours booked per facility,
            // remembering that a slot lasts half an hour. The output table should
            // consist of the facility id, name, and hours booked, sorted by facility id.
            // Try formatting the hours to two decimal places.
            //
            //select facs.facid, facs.name,
            //	trim(to_char(sum(bks.slots)/2.0, '9999999999999999D99')) as "Total Hours"
            //	from cd.bookings bks
            //	inner join cd.facilities facs
            //		on facs.facid = bks.facid
            //	group by facs.facid, facs.name
            //order by facs.facid;

            EFServiceProvider.RunInContext(context =>
            {
                var items = context.Bookings
                                    .GroupBy(booking => new { booking.FacId, booking.Facility.Name })
                                    .Select(group => new
                                    {
                                        group.Key.FacId,
                                        group.Key.Name,
                                        TotalHours = group.Sum(booking => booking.Slots) / 2M
                                    })
                                    .OrderBy(result => result.FacId)
                                    .ToList();
                /*
                    SELECT [b].[FacId], [f].[Name], SUM([b].[Slots]) / 2 AS [TotalHours]
                        FROM [Bookings] AS [b]
                        INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                        GROUP BY [b].[FacId], [f].[Name]
                        ORDER BY [b].[FacId]
                */

                var expectedResult = new[]
                {
                    new { FacId = 0, Name="Tennis Court 1", TotalHours =    660.00M },
                    new { FacId = 1, Name="Tennis Court 2", TotalHours =    639.00M },
                    new { FacId = 2, Name="Badminton Court", TotalHours =   604.50M },
                    new { FacId = 3, Name="Table Tennis", TotalHours =  415.00M },
                    new { FacId = 4, Name="Massage Room 1", TotalHours =    702.00M },
                    new { FacId = 5, Name="Massage Room 2", TotalHours =    114.00M },
                    new { FacId = 6, Name="Squash Court", TotalHours =  552.00M },
                    new { FacId = 7, Name="Snooker Table", TotalHours =     454.00M },
                    new { FacId = 8, Name="Pool Table", TotalHours =    455.50M }
                };
                items.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}