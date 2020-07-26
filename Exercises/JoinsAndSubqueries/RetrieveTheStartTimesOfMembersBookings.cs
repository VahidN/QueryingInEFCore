using System;
using System.Linq;
using EFCorePgExercises.DataLayer;
using System.Globalization;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.JoinsAndSubqueries
{
    [FullyQualifiedTestClass]
    public class RetrieveTheStartTimesOfMembersBookings
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/joins/simplejoin.html
            //  How can you produce a list of the start times for bookings by members named 'David Farrell'?
            // select bks.starttime
            // from
            //    cd.bookings bks
            //    inner join cd.members mems
            //     on mems.memid = bks.memid
            // where
            //    mems.firstname='David' and mems.surname='Farrell';

            EFServiceProvider.RunInContext(context =>
            {
                var startTimes = context.Bookings
                        .Where(booking => booking.Member.FirstName == "David"
                                            && booking.Member.Surname == "Farrell")
                        .Select(booking => new { booking.StartTime })
                        .ToList();
                /*
                    SELECT [b].[StartTime]
                    FROM [Bookings] AS [b]
                        INNER JOIN [Members] AS [m] ON [b].[MemId] = [m].[MemId]
                    WHERE ([m].[FirstName] = N'David') AND ([m].[Surname] = N'Farrell')
                */
                var expectedResult = new[]
                {
                    new { StartTime = DateTime.Parse("2012-09-18 09:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-18 17:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-18 13:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-18 20:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-19 09:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-19 15:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-19 12:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-20 15:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-20 11:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-20 14:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-21 10:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-21 14:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-22 08:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-22 17:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-23 08:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-23 17:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-23 19:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-24 08:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-24 16:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-24 12:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-25 15:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-25 17:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-26 13:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-26 17:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-27 08:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-28 11:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-28 09:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-28 13:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-29 16:00:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-29 10:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-29 13:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-29 14:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-29 17:30:00", CultureInfo.InvariantCulture) },
                    new { StartTime = DateTime.Parse("2012-09-30 14:30:00", CultureInfo.InvariantCulture) }
                };

                startTimes.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}