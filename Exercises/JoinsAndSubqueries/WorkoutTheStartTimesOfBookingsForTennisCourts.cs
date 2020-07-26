using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCorePgExercises.DataLayer;
using System.Globalization;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.JoinsAndSubqueries
{
    [FullyQualifiedTestClass]
    public class WorkoutTheStartTimesOfBookingsForTennisCourts
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/joins/simplejoin2.html
            // How can you produce a list of the start times for bookings for tennis courts,
            // for the date '2012-09-21'? Return a list of start time and facility name pairings,
            // ordered by the time.
            //select bks.starttime as start, facs.name as name
            //	from
            //		cd.facilities facs
            //		inner join cd.bookings bks
            //			on facs.facid = bks.facid
            //	where
            //		facs.facid in (0,1) and
            //		bks.starttime >= '2012-09-21' and
            //		bks.starttime < '2012-09-22'
            //order by bks.starttime;
            EFServiceProvider.RunInContext(context =>
            {
                int[] tennisCourts = { 0, 1 };
                var date1 = new DateTime(2012, 09, 21);
                var date2 = new DateTime(2012, 09, 22);
                var startTimes = context.Bookings
                        .Where(booking => tennisCourts.Contains(booking.Facility.FacId)
                                && booking.StartTime >= date1
                                && booking.StartTime < date2)
                        .Select(booking => new { booking.StartTime, booking.Facility.Name })
                        .ToList();
                /*
                    SELECT [b].[StartTime], [f].[Name]
                    FROM [Bookings] AS [b]
                    INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                    WHERE ([f].[FacId] IN (0, 1) AND ([b].[StartTime] >= @__date1_1)) AND ([b].[StartTime] < @__date2_2)
                */

                var expectedResult = new[]
                {
                    new { StartTime = DateTime.Parse("2012-09-21 08:00:00", CultureInfo.InvariantCulture), Name ="Tennis Court 1" },
                    new { StartTime = DateTime.Parse("2012-09-21 08:00:00", CultureInfo.InvariantCulture), Name ="Tennis Court 2" },
                    new { StartTime = DateTime.Parse("2012-09-21 09:30:00", CultureInfo.InvariantCulture), Name ="Tennis Court 1" },
                    new { StartTime = DateTime.Parse("2012-09-21 10:00:00", CultureInfo.InvariantCulture), Name ="Tennis Court 2" },
                    new { StartTime = DateTime.Parse("2012-09-21 11:30:00", CultureInfo.InvariantCulture), Name ="Tennis Court 2" },
                    new { StartTime = DateTime.Parse("2012-09-21 12:00:00", CultureInfo.InvariantCulture), Name ="Tennis Court 1" },
                    new { StartTime = DateTime.Parse("2012-09-21 13:30:00", CultureInfo.InvariantCulture), Name ="Tennis Court 1" },
                    new { StartTime = DateTime.Parse("2012-09-21 14:00:00", CultureInfo.InvariantCulture), Name ="Tennis Court 2" },
                    new { StartTime = DateTime.Parse("2012-09-21 15:30:00", CultureInfo.InvariantCulture), Name ="Tennis Court 1" },
                    new { StartTime = DateTime.Parse("2012-09-21 16:00:00", CultureInfo.InvariantCulture), Name ="Tennis Court 2" },
                    new { StartTime = DateTime.Parse("2012-09-21 17:00:00", CultureInfo.InvariantCulture), Name ="Tennis Court 1" },
                    new { StartTime = DateTime.Parse("2012-09-21 18:00:00", CultureInfo.InvariantCulture), Name ="Tennis Court 2" }
                };

                startTimes.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}