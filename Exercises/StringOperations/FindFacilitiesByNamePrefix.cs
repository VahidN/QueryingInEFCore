using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;
using EFCorePgExercises.Entities;

namespace EFCorePgExercises.Exercises.StringOperations
{
    [FullyQualifiedTestClass]
    public class FindFacilitiesByNamePrefix
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/string/like.html
            // Find all facilities whose name begins with 'Tennis'. Retrieve all columns.
            //
            // select * from cd.facilities where name like 'Tennis%';

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Facilities
                                        .Where(facility => facility.Name.StartsWith("Tennis"))
                                        .ToList();
                /*
                SELECT [f].[FacId], [f].[GuestCost], [f].[InitialOutlay], [f].[MemberCost], [f].[MonthlyMaintenance], [f].[Name]
                    FROM [Facilities] AS [f]
                    WHERE [f].[Name] LIKE N'Tennis%'
                */
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