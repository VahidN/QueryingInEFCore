using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Entities;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class RetrieveEverythingFromATable
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/selectall.html
            // How can you retrieve all the information from the cd.facilities table?
            // select * from cd.facilities;

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Facilities.ToList();
                /*
                    SELECT [f].[FacId], [f].[GuestCost], [f].[InitialOutlay], [f].[MemberCost], [f].[MonthlyMaintenance], [f].[Name]
                    FROM [Facilities] AS [f]
                */
                var expectedResult = new[]
                {
                    new Facility { FacId = 0, Name = "Tennis Court 1", MemberCost = 5, GuestCost = 25, InitialOutlay = 10000, MonthlyMaintenance = 200 },
                    new Facility { FacId = 1, Name = "Tennis Court 2", MemberCost = 5, GuestCost = 25, InitialOutlay = 8000, MonthlyMaintenance = 200 },
                    new Facility { FacId = 2, Name = "Badminton Court", MemberCost = 0, GuestCost = 15.5M, InitialOutlay = 4000, MonthlyMaintenance = 50 },
                    new Facility { FacId = 3, Name = "Table Tennis", MemberCost = 0, GuestCost = 5, InitialOutlay = 320, MonthlyMaintenance = 10 },
                    new Facility { FacId = 4, Name = "Massage Room 1", MemberCost = 35, GuestCost = 80, InitialOutlay = 4000, MonthlyMaintenance = 3000 },
                    new Facility { FacId = 5, Name = "Massage Room 2", MemberCost = 35, GuestCost = 80, InitialOutlay = 4000, MonthlyMaintenance = 3000 },
                    new Facility { FacId = 6, Name = "Squash Court", MemberCost = 3.5M, GuestCost = 17.5M, InitialOutlay = 5000, MonthlyMaintenance = 80 },
                    new Facility { FacId = 7, Name = "Snooker Table", MemberCost = 0, GuestCost = 5, InitialOutlay = 450, MonthlyMaintenance = 15 },
                    new Facility { FacId = 8, Name = "Pool Table", MemberCost = 0, GuestCost = 5, InitialOutlay = 400, MonthlyMaintenance = 15 }
                };
                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}