using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Entities;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class ControlWhichRowsAreRetrieved
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/where.html
            // How can you produce a list of facilities that charge a fee to members?
            // select * from cd.facilities where membercost > 0;

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Facilities.Where(x => x.MemberCost > 0).ToList();
                /*
                    SELECT [f].[FacId], [f].[GuestCost], [f].[InitialOutlay], [f].[MemberCost], [f].[MonthlyMaintenance], [f].[Name]
                    FROM [Facilities] AS [f]
                    WHERE [f].[MemberCost] > 0.0
                */
                var expectedResult = new[]
                {
                    new Facility { FacId = 0, Name = "Tennis Court 1", MemberCost = 5, GuestCost = 25, InitialOutlay = 10000, MonthlyMaintenance = 200 },
                    new Facility { FacId = 1, Name = "Tennis Court 2", MemberCost = 5, GuestCost = 25, InitialOutlay = 8000, MonthlyMaintenance = 200 },
                    new Facility { FacId = 4, Name = "Massage Room 1", MemberCost = 35, GuestCost = 80, InitialOutlay = 4000, MonthlyMaintenance = 3000 },
                    new Facility { FacId = 5, Name = "Massage Room 2", MemberCost = 35, GuestCost = 80, InitialOutlay = 4000, MonthlyMaintenance = 3000 },
                    new Facility { FacId = 6, Name = "Squash Court", MemberCost = 3.5M, GuestCost = 17.5M, InitialOutlay = 5000, MonthlyMaintenance = 80 }
                };
                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}