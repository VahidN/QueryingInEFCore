using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class RetrieveSpecificColumnsFromATable
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/selectspecific.html
            // You want to print out a list of all of the facilities and their cost to members. How would you retrieve a list of only facility names and costs?
            // select name, membercost from cd.facilities;

            EFServiceProvider.RunInContext(context =>
            {
                var facilities = context.Facilities.Select(x =>
                    new
                    {
                        x.Name,
                        x.MemberCost
                    }).ToList();
                /*
                    SELECT [f].[Name], [f].[MemberCost]
                    FROM [Facilities] AS [f]
                */
                var expectedResult = new[]
                {
                    new {  Name = "Tennis Court 1", MemberCost = 5M },
                    new {  Name = "Tennis Court 2", MemberCost = 5M },
                    new {  Name = "Badminton Court", MemberCost = 0M },
                    new {  Name = "Table Tennis", MemberCost = 0M },
                    new {  Name = "Massage Room 1", MemberCost = 35M },
                    new {  Name = "Massage Room 2", MemberCost = 35M },
                    new {  Name = "Squash Court", MemberCost = 3.5M },
                    new {  Name = "Snooker Table", MemberCost = 0M },
                    new {  Name = "Pool Table", MemberCost = 0M }
                };
                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}