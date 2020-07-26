using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class RemovingDuplicatesAndOrderingResults
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/unique.html
            // How can you produce an ordered list of the first 10 surnames in the members table?
            // The list must not contain duplicates.
            // select distinct surname 	from cd.members order by surname limit 10;

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members.OrderBy(x => x.Surname)
                                            .Select(x =>
                                                        new
                                                        {
                                                            x.Surname
                                                        })
                                            .Distinct()
                                            .Take(10)
                                            .ToList();
                /*
                    SELECT DISTINCT TOP(@__p_0) [m].[Surname]
                    FROM [Members] AS [m]
                */
                var expectedResult = new[]
                {
                    new { Surname = "Bader" } ,
                    new { Surname = "Baker" } ,
                    new { Surname = "Boothe" } ,
                    new { Surname = "Butters" } ,
                    new { Surname = "Coplin" } ,
                    new { Surname = "Crumpet" } ,
                    new { Surname = "Dare" } ,
                    new { Surname = "Farrell" } ,
                    new { Surname = "GUEST" } ,
                    new { Surname = "Genting" }
                };
                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}