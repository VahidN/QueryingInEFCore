using System;
using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using System.Globalization;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class MoreAggregation
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/agg2.html
            // You'd like to get the first and last name of the last member(s) who signed up - not just the date. How can you do that?
            // select firstname, surname, joindate from cd.members
            // where joindate =	(select max(joindate) from cd.members);

            EFServiceProvider.RunInContext(context =>
            {
                var lastMember = context.Members.OrderByDescending(m => m.JoinDate)
                            .Select(x => new { x.FirstName, x.Surname, x.JoinDate })
                            .FirstOrDefault();
                /*
                    SELECT TOP(1) [m].[FirstName], [m].[Surname], [m].[JoinDate]
                    FROM [Members] AS [m]
                    ORDER BY [m].[JoinDate] DESC
                */

                var expectedResult = new[]
                {
                    new { FirstName = "Darren", Surname = "Smith", JoinDate = DateTime.Parse("2012-09-26 18:08:45", CultureInfo.InvariantCulture) }
                };

                lastMember.Should().BeEquivalentTo(expectedResult[0]);

                var members = context.Members.Select(x => new { x.FirstName, x.Surname, x.JoinDate })
                                    .Where(x => x.JoinDate == context.Members.Max(x => x.JoinDate))
                                    .ToList();
                /*
                    SELECT [m].[FirstName], [m].[Surname], [m].[JoinDate]
                    FROM [Members] AS [m]
                    WHERE [m].[JoinDate] = (SELECT MAX([m0].[JoinDate]) FROM [Members] AS [m0])
                */
                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}