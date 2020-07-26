using System;
using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Entities;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class MatchingAgainstMultiplePossibleValues
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/where4.html
            // How can you retrieve the details of facilities with ID 1 and 5? Try to do it without using the OR operator.
            // select *	from cd.facilities where facid in (1,5);

            EFServiceProvider.RunInContext(context =>
            {
                int[] ids = { 1, 5 };
                var facilities = context.Facilities.Where(x => ids.Contains(x.FacId)).ToList();
                /*
                    SELECT [f].[FacId], [f].[GuestCost], [f].[InitialOutlay], [f].[MemberCost], [f].[MonthlyMaintenance], [f].[Name]
                    FROM [Facilities] AS [f]
                    WHERE [f].[FacId] IN (1, 5)
                */
                var expectedResult = new[]
                {
                    new Facility { FacId = 1, Name = "Tennis Court 2", MemberCost = 5, GuestCost = 25, InitialOutlay = 8000, MonthlyMaintenance = 200 },
                    new Facility { FacId = 5, Name = "Massage Room 2", MemberCost = 35, GuestCost = 80, InitialOutlay = 4000, MonthlyMaintenance = 3000 }
                };
                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}