using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Entities;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class BasicStringSearches
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/where3.html
            // How can you produce a list of all facilities with the word 'Tennis' in their name?
            // select *	from cd.facilities 	where name like '%Tennis%';

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Facilities.Where(x => x.Name.Contains("Tennis")).ToList();
                /*
                    SELECT [f].[FacId], [f].[GuestCost], [f].[InitialOutlay], [f].[MemberCost], [f].[MonthlyMaintenance], [f].[Name]
                    FROM [Facilities] AS [f]
                    WHERE CHARINDEX(N'Tennis', [f].[Name]) > 0
                */
                var expectedResult = new[]
                {
                    new Facility { FacId = 0, Name = "Tennis Court 1", MemberCost = 5, GuestCost = 25, InitialOutlay = 10000, MonthlyMaintenance = 200 },
                    new Facility { FacId = 1, Name = "Tennis Court 2", MemberCost = 5, GuestCost = 25, InitialOutlay = 8000, MonthlyMaintenance = 200 },
                    new Facility { FacId = 3, Name = "Table Tennis", MemberCost = 0, GuestCost = 5, InitialOutlay = 320, MonthlyMaintenance = 10 }
                };
                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}