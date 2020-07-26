using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.JoinsAndSubqueries
{
    [FullyQualifiedTestClass]
    public class ProduceListOfAllMembersWhoHaveUsedTennisCourt
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/joins/threejoin.html
            // How can you produce a list of all members who have used a tennis court?
            // Include in your output the name of the court, and the name of the member formatted
            // as a single column. Ensure no duplicate data, and order by the member name.
            //select distinct mems.firstname || ' ' || mems.surname as member, facs.name as facility
            //	from
            //		cd.members mems
            //		inner join cd.bookings bks
            //			on mems.memid = bks.memid
            //		inner join cd.facilities facs
            //			on bks.facid = facs.facid
            //	where
            //		bks.facid in (0,1)
            //order by member

            EFServiceProvider.RunInContext(context =>
            {
                int[] tennisCourts = { 0, 1 };
                var members = context.Members
                        .SelectMany(x => x.Bookings)
                        .Where(booking => tennisCourts.Contains(booking.Facility.FacId))
                        .Select(booking => new
                        {
                            Member = booking.Member.FirstName + " " + booking.Member.Surname,
                            Facility = booking.Facility.Name
                        })
                        .Distinct()
                        .OrderBy(x => x.Member)
                        .ToList();
                /*
                    SELECT [t].[c] AS [Member], [t].[Name] AS [Facility]
                    FROM (
                        SELECT DISTINCT ([m0].[FirstName] + N' ') + [m0].[Surname] AS [c], [f].[Name]
                        FROM [Members] AS [m]
                        INNER JOIN [Bookings] AS [b] ON [m].[MemId] = [b].[MemId]
                        INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                        INNER JOIN [Members] AS [m0] ON [b].[MemId] = [m0].[MemId]
                        WHERE [f].[FacId] IN (0, 1)
                    ) AS [t]
                    ORDER BY [t].[c]
                */
                var expectedResult = new[]
                {
                    new { Member ="Anne Baker", Facility = "Tennis Court 2" },
                    new { Member ="Anne Baker", Facility = "Tennis Court 1" },
                    new { Member ="Burton Tracy", Facility = "Tennis Court 2" },
                    new { Member ="Burton Tracy", Facility = "Tennis Court 1" },
                    new { Member ="Charles Owen", Facility = "Tennis Court 2" },
                    new { Member ="Charles Owen", Facility = "Tennis Court 1" },
                    new { Member ="Darren Smith", Facility = "Tennis Court 2" },
                    new { Member ="David Farrell", Facility = "Tennis Court 2" },
                    new { Member ="David Farrell", Facility = "Tennis Court 1" },
                    new { Member ="David Jones", Facility = "Tennis Court 1" },
                    new { Member ="David Jones", Facility = "Tennis Court 2" },
                    new { Member ="David Pinker", Facility = "Tennis Court 1" },
                    new { Member ="Douglas Jones", Facility = "Tennis Court 1" },
                    new { Member ="Erica Crumpet", Facility = "Tennis Court 1" },
                    new { Member ="Florence Bader", Facility = "Tennis Court 1" },
                    new { Member ="Florence Bader", Facility = "Tennis Court 2" },
                    new { Member ="GUEST GUEST", Facility = "Tennis Court 2" },
                    new { Member ="GUEST GUEST", Facility = "Tennis Court 1" },
                    new { Member ="Gerald Butters", Facility = "Tennis Court 1" },
                    new { Member ="Gerald Butters", Facility = "Tennis Court 2" },
                    new { Member ="Henrietta Rumney", Facility = "Tennis Court 2" },
                    new { Member ="Jack Smith", Facility = "Tennis Court 1" },
                    new { Member ="Jack Smith", Facility = "Tennis Court 2" },
                    new { Member ="Janice Joplette", Facility = "Tennis Court 1" },
                    new { Member ="Janice Joplette", Facility = "Tennis Court 2" },
                    new { Member ="Jemima Farrell", Facility = "Tennis Court 2" },
                    new { Member ="Jemima Farrell", Facility = "Tennis Court 1" },
                    new { Member ="Joan Coplin", Facility = "Tennis Court 1" },
                    new { Member ="John Hunt", Facility = "Tennis Court 1" },
                    new { Member ="John Hunt", Facility = "Tennis Court 2" },
                    new { Member ="Matthew Genting", Facility = "Tennis Court 1" },
                    new { Member ="Millicent Purview", Facility = "Tennis Court 2" },
                    new { Member ="Nancy Dare", Facility = "Tennis Court 2" },
                    new { Member ="Nancy Dare", Facility = "Tennis Court 1" },
                    new { Member ="Ponder Stibbons", Facility = "Tennis Court 2" },
                    new { Member ="Ponder Stibbons", Facility = "Tennis Court 1" },
                    new { Member ="Ramnaresh Sarwin", Facility = "Tennis Court 2" },
                    new { Member ="Ramnaresh Sarwin", Facility = "Tennis Court 1" },
                    new { Member ="Tim Boothe", Facility = "Tennis Court 1" },
                    new { Member ="Tim Boothe", Facility = "Tennis Court 2" },
                    new { Member ="Tim Rownam", Facility = "Tennis Court 1" },
                    new { Member ="Tim Rownam", Facility = "Tennis Court 2" },
                    new { Member ="Timothy Baker", Facility = "Tennis Court 2" },
                    new { Member ="Timothy Baker", Facility = "Tennis Court 1" },
                    new { Member ="Tracy Smith", Facility = "Tennis Court 2" },
                    new { Member ="Tracy Smith", Facility = "Tennis Court 1" },
                };

                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}