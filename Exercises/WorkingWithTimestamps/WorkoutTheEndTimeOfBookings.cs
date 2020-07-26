using System.Linq;
using System;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;
using System.Globalization;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class WorkoutTheEndTimeOfBookings
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/endtimes.html
            // Return a list of the start and end time of the last 10 bookings (ordered by the time at which
            // they end, followed by the time at which they start) in the system.
            //
            // select starttime, starttime + slots*(interval '30 minutes') endtime
            //	from cd.bookings
            //	order by endtime desc, starttime desc
            //	limit 10

            EFServiceProvider.RunInContext(context =>
            {
                var items = context.Bookings
                    .Select(x => new { x.StartTime, EndTime = x.StartTime.AddMinutes(x.Slots * 30) })
                    .OrderByDescending(x => x.EndTime)
                        .ThenByDescending(x => x.StartTime)
                    .Take(10)
                    .ToList();
                /*
                SELECT TOP(@__p_0) [b].[StartTime], DATEADD(minute, CAST(CAST(([b].[Slots] * 30) AS float) AS int), [b].[StartTime]) AS [EndTime]
                    FROM [Bookings] AS [b]
                    ORDER BY DATEADD(minute, CAST(CAST(([b].[Slots] * 30) AS float) AS int), [b].[StartTime]) DESC, [b].[StartTime] DESC
                */
                var expectedResult = new[]
                {
                    new { StartTime = DateTime.Parse("2013-01-01 15:30:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2013-01-01 16:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 19:30:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 20:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 19:00:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 20:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 19:30:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 20:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 19:00:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 20:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 19:00:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 20:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 18:30:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 20:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 18:30:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 20:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 19:00:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 19:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 18:30:00", CultureInfo.InvariantCulture), EndTime = DateTime.Parse("2012-09-30 19:30:00", CultureInfo.InvariantCulture) }
                };
                items.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}