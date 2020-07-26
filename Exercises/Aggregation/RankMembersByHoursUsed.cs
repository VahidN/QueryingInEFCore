using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class RankMembersByHoursUsed
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/rankmembers.html
            // Produce a list of members, along with the number of hours they've booked in facilities,
            // rounded to the nearest ten hours. Rank them by this rounded figure, producing output of
            // first name, surname, rounded hours, rank. Sort by rank, surname, and first name.
            //
            //
            //select firstname, surname,
            //	((sum(bks.slots)+10)/20)*10 as hours,
            //	rank() over (order by ((sum(bks.slots)+10)/20)*10 desc) as rank
            //
            //	from cd.bookings bks
            //	inner join cd.members mems
            //		on bks.memid = mems.memid
            //	group by mems.memid
            //order by rank, surname, firstname;
            //
            //
            //select firstname, surname, hours, rank() over (order by hours desc) from
            //	(select firstname, surname,
            //		((sum(bks.slots)+10)/20)*10 as hours
            //
            //		from cd.bookings bks
            //		inner join cd.members mems
            //			on bks.memid = mems.memid
            //		group by mems.memid
            //	) as subq
            //order by rank, surname, firstname;

            EFServiceProvider.RunInContext(context =>
            {
                var itemsQuery = context.Bookings
                                    .GroupBy(booking => new
                                    {
                                        booking.Member.FirstName,
                                        booking.Member.Surname
                                    })
                                    .Select(group => new
                                    {
                                        group.Key.FirstName,
                                        group.Key.Surname,
                                        Hours = (group.Sum(booking => booking.Slots) + 10) / 20 * 10
                                    })
                                    .OrderByDescending(result => result.Hours)
                                        .ThenBy(result => result.Surname)
                                        .ThenBy(result => result.FirstName);
                var rankedItems = itemsQuery.Select(thisItem => new
                {
                    thisItem.FirstName,
                    thisItem.Surname,
                    thisItem.Hours,
                    Rank = itemsQuery.Count(mainItem => mainItem.Hours > thisItem.Hours) + 1
                })
                .ToList();
                /*
                SELECT [m0].[FirstName], [m0].[Surname], ((SUM([b0].[Slots]) + 10) / 20) * 10 AS [Hours], (
                        SELECT COUNT(*)
                        FROM (
                            SELECT [m].[FirstName], [m].[Surname], ((SUM([b].[Slots]) + 10) / 20) * 10 AS [c]
                            FROM [Bookings] AS [b]
                            INNER JOIN [Members] AS [m] ON [b].[MemId] = [m].[MemId]
                            GROUP BY [m].[FirstName], [m].[Surname]
                        ) AS [t]
                        WHERE [t].[c] > (((SUM([b0].[Slots]) + 10) / 20) * 10)) + 1 AS [Rank]
                    FROM [Bookings] AS [b0]
                    INNER JOIN [Members] AS [m0] ON [b0].[MemId] = [m0].[MemId]
                    GROUP BY [m0].[FirstName], [m0].[Surname]
                    ORDER BY ((SUM([b0].[Slots]) + 10) / 20) * 10 DESC, [m0].[Surname], [m0].[FirstName]
                */
                var expectedResult = new[]
                {
                    new { FirstName = "GUEST", Surname ="GUEST", Hours =    1200, Rank=     1 } ,
                    new { FirstName = "Darren", Surname ="Smith", Hours =   340, Rank=  2 } ,
                    new { FirstName = "Tim", Surname ="Rownam", Hours =     330, Rank=  3 } ,
                    new { FirstName = "Tim", Surname ="Boothe", Hours =     220, Rank=  4 } ,
                    new { FirstName = "Tracy", Surname ="Smith", Hours =    220, Rank=  4 } ,
                    new { FirstName = "Gerald", Surname ="Butters", Hours =     210, Rank=  6 } ,
                    new { FirstName = "Burton", Surname ="Tracy", Hours =   180, Rank=  7 } ,
                    new { FirstName = "Charles", Surname ="Owen", Hours =   170, Rank=  8 } ,
                    new { FirstName = "Janice", Surname ="Joplette", Hours =    160, Rank=  9 } ,
                    new { FirstName = "Anne", Surname ="Baker", Hours =     150, Rank=  10 } ,
                    new { FirstName = "Timothy", Surname ="Baker", Hours =  150, Rank=  10 } ,
                    new { FirstName = "David", Surname ="Jones", Hours =    150, Rank=  10 } ,
                    new { FirstName = "Nancy", Surname ="Dare", Hours =     130, Rank=  13 } ,
                    new { FirstName = "Florence", Surname ="Bader", Hours =     120, Rank=  14 } ,
                    new { FirstName = "Anna", Surname ="Mackenzie", Hours =     120, Rank=  14 } ,
                    new { FirstName = "Ponder", Surname ="Stibbons", Hours =    120, Rank=  14 } ,
                    new { FirstName = "Jack", Surname ="Smith", Hours =     110, Rank=  17 } ,
                    new { FirstName = "Jemima", Surname ="Farrell", Hours =     90, Rank=   18 } ,
                    new { FirstName = "David", Surname ="Pinker", Hours =   80, Rank=   19 } ,
                    new { FirstName = "Ramnaresh", Surname ="Sarwin", Hours =   80, Rank=   19 } ,
                    new { FirstName = "Matthew", Surname ="Genting", Hours =    70, Rank=   21 } ,
                    new { FirstName = "Joan", Surname ="Coplin", Hours =    50, Rank=   22 } ,
                    new { FirstName = "David", Surname ="Farrell", Hours =  30, Rank=   23 } ,
                    new { FirstName = "Henry", Surname ="Worthington-Smyth", Hours =    30, Rank=   23 } ,
                    new { FirstName = "John", Surname ="Hunt", Hours =  20, Rank=   25 } ,
                    new { FirstName = "Douglas", Surname ="Jones", Hours =  20, Rank=   25 } ,
                    new { FirstName = "Millicent", Surname ="Purview", Hours =  20, Rank=   25 } ,
                    new { FirstName = "Henrietta", Surname ="Rumney", Hours =   20, Rank=   25 } ,
                    new { FirstName = "Erica", Surname ="Crumpet", Hours =  10, Rank=   29 } ,
                    new { FirstName = "Hyacinth", Surname ="Tupperware", Hours =    10, Rank=   29 } ,
                };

                rankedItems.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}