using System.Linq;
using EFCorePgExercises.DataLayer;
using System.Collections.Generic;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.RecursiveQueries
{
    [FullyQualifiedTestClass]
    public class ProduceCTEThatCanReturnTheUpwardRecommendationChainForAnyMember
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/recursive/getupwardall.html
            // Produce a CTE that can return the upward recommendation chain for any member.
            // You should be able to select recommender from recommenders where member=x.
            // Demonstrate it by getting the chains for members 12 and 22. Results table should have
            // member and recommender, ordered by member ascending, recommender descending.
            //
            //with recursive recommenders(recommender, member) as (
            //	select recommendedby, memid
            //		from cd.members
            //	union all
            //	select mems.recommendedby, recs.member
            //		from recommenders recs
            //		inner join cd.members mems
            //			on mems.memid = recs.recommender
            //)
            //select recs.member member, recs.recommender, mems.firstname, mems.surname
            //	from recommenders recs
            //	inner join cd.members mems
            //		on recs.recommender = mems.memid
            //	where recs.member = 22 or recs.member = 12
            //order by recs.member asc, recs.recommender desc

            EFServiceProvider.RunInContext(context =>
            {
                var id1 = 12;
                var id2 = 22;
                var entityWithAllOfItsParents =
                        context.Members
                            .Where(member => member.MemId == id1 || member.MemId == id2
                                            || member.Children.Any(m => member.MemId == m.RecommendedBy))
                            .ToList(); //It's a MUST - get all children from the database
                /*
                    SELECT [m].[MemId], [m].[Address], [m].[FirstName], [m].[JoinDate], [m].[RecommendedBy], [m].[Surname], [m].[Telephone], [m].[ZipCode]
                        FROM [Members] AS [m]
                        WHERE (([m].[MemId] = @__id1_0) OR ([m].[MemId] = @__id2_1)) OR EXISTS (
                            SELECT 1
                            FROM [Members] AS [m0]
                            WHERE ([m].[MemId] = [m0].[RecommendedBy]) AND ([m].[MemId] = [m0].[RecommendedBy]))
                */
                var expectedResultFor12 = new[]
                {
                    new { Recommender = 9, FirstName = "Ponder", Surname ="Stibbons" },
                    new { Recommender = 6, FirstName = "Burton", Surname ="Tracy" }
                };
                var actualResultFor12 = new List<dynamic>();
                RecursiveUtils.FindParents(entityWithAllOfItsParents.FirstOrDefault(x => x.MemId == id1), actualResultFor12);
                actualResultFor12.Should().BeEquivalentTo(expectedResultFor12);

                var expectedResultFor22 = new[]
                {
                    new { Recommender = 16, FirstName = "Timothy", Surname ="Baker" },
                    new { Recommender = 13, FirstName = "Jemima", Surname ="Farrell" }
                };
                var actualResultFor22 = new List<dynamic>();
                RecursiveUtils.FindParents(entityWithAllOfItsParents.FirstOrDefault(x => x.MemId == id2), actualResultFor22);
                actualResultFor22.Should().BeEquivalentTo(expectedResultFor22);
            });
        }
    }
}