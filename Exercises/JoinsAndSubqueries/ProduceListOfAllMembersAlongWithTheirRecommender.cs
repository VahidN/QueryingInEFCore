using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.JoinsAndSubqueries
{
    [FullyQualifiedTestClass]
    public class ProduceListOfAllMembersAlongWithTheirRecommender
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/joins/self2.html
            // How can you output a list of all members, including the individual
            // who recommended them (if any)? Ensure that results are ordered by (surname, firstname).
            //select mems.firstname as memfname, mems.surname as memsname, recs.firstname as recfname, recs.surname as recsname
            //	from
            //		cd.members mems
            //		left outer join cd.members recs
            //			on recs.memid = mems.recommendedby
            //order by memsname, memfname;

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members
                        .Select(member => new
                        {
                            memFName = member.FirstName,
                            memSName = member.Surname,
                            recFName = member.Recommender.FirstName ?? "",
                            recSName = member.Recommender.Surname ?? ""
                        })
                        .OrderBy(member => member.memSName).ThenBy(member => member.memFName)
                        .ToList();
                /*
                    SELECT [m].[FirstName] AS [memFName], [m].[Surname] AS [memSName],
                        COALESCE([m0].[FirstName], N'') AS [recFName], COALESCE([m0].[Surname], N'') AS [recSName]
                        FROM [Members] AS [m]
                        LEFT JOIN [Members] AS [m0] ON [m].[RecommendedBy] = [m0].[MemId]
                        ORDER BY [m].[Surname], [m].[FirstName]
                */
                var expectedResult = new[]
                {
                    new { memFName = "Florence", memSName="Bader", recFName ="Ponder", recSName ="Stibbons" },
                    new { memFName = "Anne", memSName="Baker", recFName ="Ponder", recSName ="Stibbons" },
                    new { memFName = "Timothy", memSName="Baker", recFName ="Jemima", recSName ="Farrell" },
                    new { memFName = "Tim", memSName="Boothe", recFName ="Tim", recSName ="Rownam" },
                    new { memFName = "Gerald", memSName="Butters", recFName ="Darren", recSName ="Smith" },
                    new { memFName = "Joan", memSName="Coplin", recFName ="Timothy", recSName ="Baker" },
                    new { memFName = "Erica", memSName="Crumpet", recFName ="Tracy", recSName ="Smith" },
                    new { memFName = "Nancy", memSName="Dare", recFName ="Janice", recSName ="Joplette" },
                    new { memFName = "David", memSName="Farrell", recFName ="", recSName ="" },
                    new { memFName = "Jemima", memSName="Farrell", recFName ="", recSName ="" },
                    new { memFName = "GUEST", memSName="GUEST", recFName ="", recSName ="" },
                    new { memFName = "Matthew", memSName="Genting", recFName ="Gerald", recSName ="Butters" },
                    new { memFName = "John", memSName="Hunt", recFName ="Millicent", recSName ="Purview" },
                    new { memFName = "David", memSName="Jones", recFName ="Janice", recSName ="Joplette" },
                    new { memFName = "Douglas", memSName="Jones", recFName ="David", recSName ="Jones" },
                    new { memFName = "Janice", memSName="Joplette", recFName ="Darren", recSName ="Smith" },
                    new { memFName = "Anna", memSName="Mackenzie", recFName ="Darren", recSName ="Smith" },
                    new { memFName = "Charles", memSName="Owen", recFName ="Darren", recSName ="Smith" },
                    new { memFName = "David", memSName="Pinker", recFName ="Jemima", recSName ="Farrell" },
                    new { memFName = "Millicent", memSName="Purview", recFName ="Tracy", recSName ="Smith" },
                    new { memFName = "Tim", memSName="Rownam", recFName ="", recSName ="" },
                    new { memFName = "Henrietta", memSName="Rumney", recFName ="Matthew", recSName ="Genting" },
                    new { memFName = "Ramnaresh", memSName="Sarwin", recFName ="Florence", recSName ="Bader" },
                    new { memFName = "Darren", memSName="Smith", recFName ="", recSName ="" },
                    new { memFName = "Darren", memSName="Smith", recFName ="", recSName ="" },
                    new { memFName = "Jack", memSName="Smith", recFName ="Darren", recSName ="Smith" },
                    new { memFName = "Tracy", memSName="Smith", recFName ="", recSName ="" },
                    new { memFName = "Ponder", memSName="Stibbons", recFName ="Burton", recSName ="Tracy" },
                    new { memFName = "Burton", memSName="Tracy", recFName ="", recSName ="" },
                    new { memFName = "Hyacinth", memSName="Tupperware", recFName ="", recSName ="" },
                    new { memFName = "Henry", memSName="Worthington-Smyth", recFName ="Tracy", recSName ="Smith" },
                };

                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}