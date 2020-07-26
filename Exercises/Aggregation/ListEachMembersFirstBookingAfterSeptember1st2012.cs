using System.Linq;
using System;
using System.Globalization;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ListEachMembersFirstBookingAfterSeptember1st2012
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/nbooking.html
            // Produce a list of each member name, id, and their first booking after September 1st 2012.
            // Order by member ID.
            //
            //select mems.surname, mems.firstname, mems.memid, min(bks.starttime) as starttime
            //	from cd.bookings bks
            //	inner join cd.members mems on
            //		mems.memid = bks.memid
            //	where starttime >= '2012-09-01'
            //	group by mems.surname, mems.firstname, mems.memid
            //order by mems.memid;

            EFServiceProvider.RunInContext(context =>
            {
                var date1 = new DateTime(2012, 09, 01);
                var items = context.Bookings
                                    .Where(booking => booking.StartTime >= date1)
                                    .GroupBy(booking => new
                                    {
                                        booking.Member.Surname,
                                        booking.Member.FirstName,
                                        booking.Member.MemId
                                    })
                                    .Select(group => new
                                    {
                                        group.Key.Surname,
                                        group.Key.FirstName,
                                        group.Key.MemId,
                                        StartTime = group.Min(booking => booking.StartTime)
                                    })
                                    .OrderBy(result => result.MemId)
                                    .ToList();
                /*
                    SELECT [m].[Surname], [m].[FirstName], [m].[MemId], MIN([b].[StartTime]) AS [StartTime]
                        FROM [Bookings] AS [b]
                        INNER JOIN [Members] AS [m] ON [b].[MemId] = [m].[MemId]
                        WHERE [b].[StartTime] >= @__date1_0
                        GROUP BY [m].[Surname], [m].[FirstName], [m].[MemId]
                        ORDER BY [m].[MemId]
                */

                var expectedResult = new[]
                {
                    new { Surname = "GUEST", FirstName ="GUEST", MemId = 0, StartTime = DateTime.Parse("2012-09-01 08:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Smith", FirstName ="Darren", MemId =   1, StartTime = DateTime.Parse("2012-09-01 09:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Smith", FirstName ="Tracy", MemId =    2, StartTime = DateTime.Parse("2012-09-01 11:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Rownam", FirstName ="Tim", MemId =     3, StartTime = DateTime.Parse("2012-09-01 16:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Joplette", FirstName ="Janice", MemId =    4, StartTime = DateTime.Parse("2012-09-01 15:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Butters", FirstName ="Gerald", MemId =     5, StartTime = DateTime.Parse("2012-09-02 12:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Tracy", FirstName ="Burton", MemId =   6, StartTime = DateTime.Parse("2012-09-01 15:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Dare", FirstName ="Nancy", MemId =     7, StartTime = DateTime.Parse("2012-09-01 12:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Boothe", FirstName ="Tim", MemId =     8, StartTime = DateTime.Parse("2012-09-01 08:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Stibbons", FirstName ="Ponder", MemId =    9, StartTime = DateTime.Parse("2012-09-01 11:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Owen", FirstName ="Charles", MemId =   10, StartTime = DateTime.Parse("2012-09-01 11:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Jones", FirstName ="David", MemId =    11, StartTime = DateTime.Parse("2012-09-01 09:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Baker", FirstName ="Anne", MemId =     12, StartTime = DateTime.Parse("2012-09-01 14:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Farrell", FirstName ="Jemima", MemId =     13, StartTime = DateTime.Parse("2012-09-01 09:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Smith", FirstName ="Jack", MemId =     14, StartTime = DateTime.Parse("2012-09-01 11:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Bader", FirstName ="Florence", MemId =     15, StartTime = DateTime.Parse("2012-09-01 10:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Baker", FirstName ="Timothy", MemId =      16, StartTime = DateTime.Parse("2012-09-01 15:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Pinker", FirstName ="David", MemId =   17, StartTime = DateTime.Parse("2012-09-01 08:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Genting", FirstName ="Matthew", MemId =    20, StartTime = DateTime.Parse("2012-09-01 18:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Mackenzie", FirstName ="Anna", MemId =     21, StartTime = DateTime.Parse("2012-09-01 08:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Coplin", FirstName ="Joan", MemId =    22, StartTime = DateTime.Parse("2012-09-02 11:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Sarwin", FirstName ="Ramnaresh", MemId =   24, StartTime = DateTime.Parse("2012-09-04 11:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Jones", FirstName ="Douglas", MemId =      26, StartTime = DateTime.Parse("2012-09-08 13:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Rumney", FirstName ="Henrietta", MemId =   27, StartTime = DateTime.Parse("2012-09-16 13:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Farrell", FirstName ="David", MemId =      28, StartTime = DateTime.Parse("2012-09-18 09:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Worthington-Smyth", FirstName ="Henry", MemId =    29, StartTime = DateTime.Parse("2012-09-19 09:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Purview", FirstName ="Millicent", MemId =      30, StartTime = DateTime.Parse("2012-09-19 11:30:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Tupperware", FirstName ="Hyacinth", MemId =    33, StartTime = DateTime.Parse("2012-09-20 08:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Hunt", FirstName ="John", MemId =      35, StartTime = DateTime.Parse("2012-09-23 14:00:00", CultureInfo.InvariantCulture) },
                    new { Surname = "Crumpet", FirstName ="Erica", MemId =      36, StartTime = DateTime.Parse("2012-09-27 11:30:00", CultureInfo.InvariantCulture) }
                };
                items.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}