using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;
using EFCorePgExercises.Entities;

namespace EFCorePgExercises.Exercises.StringOperations
{
    [FullyQualifiedTestClass]
    public class PerformCaseInsensitiveSearch
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/string/case.html
            // Perform a case-insensitive search to find all facilities whose name begins with 'tennis'.
            // Retrieve all columns.
            //
            // select * from cd.facilities where upper(name) like 'TENNIS%';

            EFServiceProvider.RunInContext(context =>
            {
                // `case-insensitive` search is default in SQL-Server's selected collation.
                var facilities = context.Facilities
                                        .Where(facility => facility.Name.StartsWith("TENNIS"))
                                        .ToList();
                var expectedResult = new[]
                {
                    new Facility { FacId = 0, Name = "Tennis Court 1", MemberCost = 5, GuestCost = 25, InitialOutlay = 10000, MonthlyMaintenance = 200 },
                    new Facility { FacId = 1, Name = "Tennis Court 2", MemberCost = 5, GuestCost = 25, InitialOutlay = 8000, MonthlyMaintenance = 200 }
                };
                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}