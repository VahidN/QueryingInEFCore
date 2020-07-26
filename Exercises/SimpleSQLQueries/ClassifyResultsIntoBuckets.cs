using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class ClassifyResultsIntoBuckets
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/classify.html
            // How can you produce a list of facilities, with each labelled as 'cheap' or 'expensive'
            // depending on if their monthly maintenance cost is more than $100? Return the name and
            // monthly maintenance of the facilities in question.
            // select name,
            // case when (monthlymaintenance > 100) then 'expensive' else 'cheap' end as cost
            // from cd.facilities;

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Facilities
                        .Select(x =>
                                    new
                                    {
                                        x.Name,
                                        Cost = x.MonthlyMaintenance > 100 ? "expensive" : "cheap"
                                    }).ToList();
                /*
                    SELECT [f].[Name], CASE
                        WHEN [f].[MonthlyMaintenance] > 100.0 THEN N'expensive'
                        ELSE N'cheap'
                        END AS [Cost]
                    FROM [Facilities] AS [f]
                */
                var expectedResult = new[]
                {
                    new { Name = "Tennis Court 1", Cost="expensive" } ,
                    new { Name = "Tennis Court 2", Cost="expensive" } ,
                    new { Name = "Badminton Court", Cost="cheap" } ,
                    new { Name = "Table Tennis", Cost="cheap" } ,
                    new { Name = "Massage Room 1", Cost="expensive" } ,
                    new { Name = "Massage Room 2", Cost="expensive" } ,
                    new { Name = "Squash Court", Cost="cheap" } ,
                    new { Name = "Snooker Table", Cost="cheap" } ,
                    new { Name = "Pool Table", Cost="cheap" }
                };
                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}