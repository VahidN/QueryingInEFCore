using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.JoinsAndSubqueries
{
    [FullyQualifiedTestClass]
    public class ProduceListOfAllMembersWhoHaveRecommendedAnotherMember
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/joins/self.html
            // How can you output a list of all members who have recommended another member?
            // Ensure that there are no duplicates in the list, and that results are ordered by (surname, firstname).
            //select distinct recs.firstname as firstname, recs.surname as surname
            //	from
            //		cd.members mems
            //		inner join cd.members recs
            //			on recs.memid = mems.recommendedby
            //order by surname, firstname;

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members
                        .Where(member => member.Recommender != null)
                        .Select(member => new { member.Recommender.FirstName, member.Recommender.Surname })
                        .Distinct()
                        .OrderBy(member => member.Surname).ThenBy(member => member.FirstName)
                        .ToList();
                /*
                    SELECT [t].[FirstName], [t].[Surname]
                    FROM (
                        SELECT DISTINCT [m0].[FirstName], [m0].[Surname]
                        FROM [Members] AS [m]
                        LEFT JOIN [Members] AS [m0] ON [m].[RecommendedBy] = [m0].[MemId]
                        WHERE [m0].[MemId] IS NOT NULL
                    ) AS [t]
                    ORDER BY [t].[Surname], [t].[FirstName]
                */

                var expectedResult = new[]
                {
                    new { FirstName = "Florence", Surname = "Bader" },
                    new { FirstName = "Timothy", Surname = "Baker" },
                    new { FirstName = "Gerald", Surname = "Butters" },
                    new { FirstName = "Jemima", Surname = "Farrell" },
                    new { FirstName = "Matthew", Surname = "Genting" },
                    new { FirstName = "David", Surname = "Jones" },
                    new { FirstName = "Janice", Surname = "Joplette" },
                    new { FirstName = "Millicent", Surname = "Purview" },
                    new { FirstName = "Tim", Surname = "Rownam" },
                    new { FirstName = "Darren", Surname = "Smith" },
                    new { FirstName = "Tracy", Surname = "Smith" },
                    new { FirstName = "Ponder", Surname = "Stibbons" },
                    new { FirstName = "Burton", Surname = "Tracy" }
                };

                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}