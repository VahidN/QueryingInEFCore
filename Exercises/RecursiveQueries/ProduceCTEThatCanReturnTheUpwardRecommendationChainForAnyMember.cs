using System.Linq;
using EFCorePgExercises.DataLayer;
using System.Collections.Generic;
using FluentAssertions;
using EFCorePgExercises.Utils;
using LinqToDB.EntityFrameworkCore;
using LinqToDB;

namespace EFCorePgExercises.Exercises.RecursiveQueries
{
    [FullyQualifiedTestClass]
    public class ProduceCTEThatCanReturnTheUpwardRecommendationChainForAnyMember
    {
        [FullyQualifiedTestMethod]
        public void Test_Method1()
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

        [FullyQualifiedTestMethod]
        public void Test_Method2()
        {
            // https://pgexercises.com/questions/recursive/getupwardall.html
            // Produce a CTE that can return the upward recommendation chain for any member.
            // You should be able to select recommender from recommenders where member=x.
            // Demonstrate it by getting the chains for members 12 and 22. Results table should have
            // member and recommender, ordered by member ascending, recommender descending.

            // Using
            // https://github.com/linq2db/linq2db.EntityFrameworkCore
            // https://linq2db.github.io/articles/sql/CTE.html

            EFServiceProvider.RunInContext(context =>
            {
                var memberHierarchyCte =
                    context.CreateLinqToDbContext().GetCte<MemberHierarchyCTE>(memberHierarchy =>
                    {
                        return
                            (
                                from member in context.Members
                                select new MemberHierarchyCTE
                                {
                                    ChildId = member.MemId,
                                    ParentId = member.RecommendedBy
                                }
                            )
                            .Concat
                            (
                                from member in context.Members
                                from hierarchy in memberHierarchy
                                            .InnerJoin(hierarchy => member.MemId == hierarchy.ParentId)
                                select new MemberHierarchyCTE
                                {
                                    ChildId = hierarchy.ChildId,
                                    ParentId = member.RecommendedBy
                                }
                            );
                    });

                var parentIdsQuery = memberHierarchyCte.Where(mh => (mh.ChildId == 12 || mh.ChildId == 22) && mh.ParentId != null)
                                                        .Select(mh => mh.ParentId);

                var parents = context.Members.Where(member => parentIdsQuery.Contains(member.MemId))
                                                .Select(member => new
                                                {
                                                    Recommender = member.MemId,
                                                    member.FirstName,
                                                    member.Surname
                                                })
                                                .OrderByDescending(result => result.Recommender)
                                                .ToLinqToDB()
                                                .ToList();

                /*
                    WITH [memberHierarchy] ([ChildId], [ParentId])
                    AS
                    (
                        SELECT
                            [member_1].[MemId],
                            [member_1].[RecommendedBy]
                        FROM
                            [Members] [member_1]
                        UNION ALL
                        SELECT
                            [hierarchy_1].[ChildId],
                            [member_2].[RecommendedBy]
                        FROM
                            [Members] [member_2]
                                INNER JOIN [memberHierarchy] [hierarchy_1] ON [member_2].[MemId] = [hierarchy_1].[ParentId]
                    )
                    SELECT
                        [member_3].[MemId],
                        [member_3].[FirstName],
                        [member_3].[Surname]
                    FROM
                        [Members] [member_3]
                    WHERE
                        EXISTS(
                            SELECT
                                *
                            FROM
                                [memberHierarchy] [mh]
                            WHERE
                                ([mh].[ChildId] = 12 OR [mh].[ChildId] = 22) AND [mh].[ParentId] IS NOT NULL AND
                                [mh].[ParentId] = [member_3].[MemId]
                        )
                    ORDER BY
                        [member_3].[MemId] DESC
                */

                var expectedResult = new[]
                {
                    new { Recommender = 16, FirstName = "Timothy", Surname ="Baker" },
                    new { Recommender = 13, FirstName = "Jemima", Surname ="Farrell" },
                    new { Recommender = 9, FirstName = "Ponder", Surname ="Stibbons" },
                    new { Recommender = 6, FirstName = "Burton", Surname ="Tracy" }
                };

                parents.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}