using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.JoinsAndSubqueries
{
    [FullyQualifiedTestClass]
    public class ProduceListOfAllMembersAlongWithTheirRecommenderUsingNoJoins
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/joins/sub.html
            // How can you output a list of all members, including the individual who recommended them (if any), without using any joins?
            // Ensure that there are no duplicates in the list, and that each firstname + surname pairing is formatted as a column and ordered.
            //select distinct mems.firstname || ' ' ||  mems.surname as member,
            //	(select recs.firstname || ' ' || recs.surname as recommender
            //		from cd.members recs
            //		where recs.memid = mems.recommendedby
            //	)
            //	from
            //		cd.members mems
            //order by member;

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members
                        .Select(member =>
                        new
                        {
                            Member = member.FirstName + " " + member.Surname,
                            Recommender = context.Members
                                .Where(recommender => recommender.MemId == member.RecommendedBy)
                                .Select(recommender => recommender.FirstName + " " + recommender.Surname)
                                .FirstOrDefault() ?? ""
                        })
                        .Distinct()
                        .OrderBy(member => member.Member)
                        .ToList();
                /*
                    SELECT [t].[c] AS [Member], [t].[c0] AS [Recommender]
                        FROM (
                            SELECT DISTINCT ([m0].[FirstName] + N' ') + [m0].[Surname] AS [c], COALESCE((
                                SELECT TOP(1) ([m].[FirstName] + N' ') + [m].[Surname]
                                FROM [Members] AS [m]
                                WHERE [m].[MemId] = [m0].[RecommendedBy]), N'') AS [c0]
                            FROM [Members] AS [m0]
                        ) AS [t]
                        ORDER BY [t].[c]
                */
                var expectedResult = new[]
                {
                    new { Member = "Anna Mackenzie", Recommender ="Darren Smith" },
                    new { Member = "Anne Baker", Recommender ="Ponder Stibbons" },
                    new { Member = "Burton Tracy", Recommender ="" },
                    new { Member = "Charles Owen", Recommender ="Darren Smith" },
                    new { Member = "Darren Smith", Recommender ="" },
                    new { Member = "David Farrell", Recommender ="" },
                    new { Member = "David Jones", Recommender ="Janice Joplette" },
                    new { Member = "David Pinker", Recommender ="Jemima Farrell" },
                    new { Member = "Douglas Jones", Recommender ="David Jones" },
                    new { Member = "Erica Crumpet", Recommender ="Tracy Smith" },
                    new { Member = "Florence Bader", Recommender ="Ponder Stibbons" },
                    new { Member = "GUEST GUEST", Recommender ="" },
                    new { Member = "Gerald Butters", Recommender ="Darren Smith" },
                    new { Member = "Henrietta Rumney", Recommender ="Matthew Genting" },
                    new { Member = "Henry Worthington-Smyth", Recommender ="Tracy Smith" },
                    new { Member = "Hyacinth Tupperware", Recommender ="" },
                    new { Member = "Jack Smith", Recommender ="Darren Smith" },
                    new { Member = "Janice Joplette", Recommender ="Darren Smith" },
                    new { Member = "Jemima Farrell", Recommender ="" },
                    new { Member = "Joan Coplin", Recommender ="Timothy Baker" },
                    new { Member = "John Hunt", Recommender ="Millicent Purview" },
                    new { Member = "Matthew Genting", Recommender ="Gerald Butters" },
                    new { Member = "Millicent Purview", Recommender ="Tracy Smith" },
                    new { Member = "Nancy Dare", Recommender ="Janice Joplette" },
                    new { Member = "Ponder Stibbons", Recommender ="Burton Tracy" },
                    new { Member = "Ramnaresh Sarwin", Recommender ="Florence Bader" },
                    new { Member = "Tim Boothe", Recommender ="Tim Rownam" },
                    new { Member = "Tim Rownam", Recommender ="" },
                    new { Member = "Timothy Baker", Recommender ="Jemima Farrell" },
                    new { Member = "Tracy Smith", Recommender ="" }
                };

                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}