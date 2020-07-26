using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class CountTheNumberOfRecommendationsEachMemberMakes
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/count3.html
            // Produce a count of the number of recommendations each member has made. Order by member ID.
            //select recommendedby, count(*)
            //	from cd.members
            //	where recommendedby is not null
            //	group by recommendedby
            //order by recommendedby;

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members
                        .Where(member => member.RecommendedBy != null)
                        .GroupBy(member => member.RecommendedBy)
                        .Select(group => new
                        {
                            RecommendedBy = group.Key,
                            Count = group.Count()
                        })
                        .OrderBy(result => result.RecommendedBy)
                        .ToList();
                /*
                    SELECT [m].[RecommendedBy], COUNT(*) AS [Count]
                    FROM [Members] AS [m]
                    WHERE [m].[RecommendedBy] IS NOT NULL
                    GROUP BY [m].[RecommendedBy]
                    ORDER BY [m].[RecommendedBy]
                */

                var expectedResult = new[]
                {
                    new { RecommendedBy = 1, Count=     5},
                    new { RecommendedBy = 2, Count=     3},
                    new { RecommendedBy = 3, Count=     1},
                    new { RecommendedBy = 4, Count=     2},
                    new { RecommendedBy = 5, Count=     1},
                    new { RecommendedBy = 6, Count=     1},
                    new { RecommendedBy = 9, Count=     2},
                    new { RecommendedBy = 11, Count=    1},
                    new { RecommendedBy = 13, Count=    2},
                    new { RecommendedBy = 15, Count=    1},
                    new { RecommendedBy = 16, Count=    1},
                    new { RecommendedBy = 20, Count=    1},
                    new { RecommendedBy = 30, Count=    1}
                };
                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}