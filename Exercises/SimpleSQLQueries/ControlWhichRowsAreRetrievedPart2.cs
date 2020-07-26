using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class ControlWhichRowsAreRetrievedPart2
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/where2.html
            // How can you produce a list of facilities that charge a fee to members, and that fee is less than 1/50th of the monthly maintenance cost?
            // Return the facid, facility name, member cost, and monthly maintenance of the facilities in question.
            // select facid, name, membercost, monthlymaintenance
            // from cd.facilities where membercost > 0 and (membercost < monthlymaintenance/50.0);

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Facilities.Where(x => x.MemberCost > 0
                                                            && x.MemberCost < (x.MonthlyMaintenance / 50))
                                                    .Select(x =>
                                                        new
                                                        {
                                                            x.FacId,
                                                            x.Name,
                                                            x.MemberCost,
                                                            x.MonthlyMaintenance
                                                        }).ToList();
                /*
                    SELECT [f].[FacId], [f].[Name], [f].[MemberCost], [f].[MonthlyMaintenance]
                    FROM [Facilities] AS [f]
                    WHERE ([f].[MemberCost] > 0.0) AND ([f].[MemberCost] < ([f].[MonthlyMaintenance] / 50.0))
                */
                var expectedResult = new[]
                {
                    new { FacId = 4, Name = "Massage Room 1", MemberCost = 35M, MonthlyMaintenance = 3000M },
                    new { FacId = 5, Name = "Massage Room 2", MemberCost = 35M, MonthlyMaintenance = 3000M },
                };
                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}