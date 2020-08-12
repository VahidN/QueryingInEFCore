using System.Linq;
using EFCorePgExercises.DataLayer;
using System.Collections.Generic;
using FluentAssertions;
using EFCorePgExercises.Utils;
using Microsoft.EntityFrameworkCore;
using LinqToDB;
using LinqToDB.EntityFrameworkCore;

namespace EFCorePgExercises.Exercises.RecursiveQueries
{
    [FullyQualifiedTestClass]
    public class FindTheUpwardRecommendationChainForMemberID27
    {
        [FullyQualifiedTestMethod]
        public void Test_Method1()
        {
            // https://pgexercises.com/questions/recursive/getupward.html
            // Find the upward recommendation chain for member ID 27: that is, the member who recommended them,
            // and the member who recommended that member, and so on. Return member ID, first name,
            // and surname. Order by descending member id.
            //
            //with recursive recommenders(recommender) as (
            //	select recommendedby from cd.members where memid = 27
            //	union all
            //	select mems.recommendedby
            //		from recommenders recs
            //		inner join cd.members mems
            //			on mems.memid = recs.recommender
            //)
            //select recs.recommender, mems.firstname, mems.surname
            //	from recommenders recs
            //	inner join cd.members mems
            //		on recs.recommender = mems.memid
            //order by memid desc
            //
            //with recursive increment(num) as (
            //	select 1
            //	union all
            //	select increment.num + 1 from increment where increment.num < 5
            //)
            //select * from increment;

            EFServiceProvider.RunInContext(context =>
            {
                var id = 27;
                var entity27WithAllOfItsParents =
                        context.Members
                            .Where(member => member.MemId == id
                                            || member.Children.Any(m => member.MemId == m.RecommendedBy))
                            .ToList() //It's a MUST - get all children from the database
                            .FirstOrDefault(x => x.MemId == id);// then get the root of the tree

                /*
                SELECT [m].[MemId], [m].[Address], [m].[FirstName], [m].[JoinDate], [m].[RecommendedBy], [m].[Surname], [m].[Telephone], [m].[ZipCode]
                        FROM [Members] AS [m]
                        WHERE ([m].[MemId] = @__id_0) OR EXISTS (
                            SELECT 1
                            FROM [Members] AS [m0]
                            WHERE ([m].[MemId] = [m0].[RecommendedBy]) AND ([m].[MemId] = [m0].[RecommendedBy]))
                */

                var expectedResult = new[]
                {
                    new { Recommender = 20, FirstName = "Matthew", Surname ="Genting" },
                    new { Recommender = 5, FirstName = "Gerald", Surname ="Butters" },
                    new { Recommender = 1, FirstName = "Darren", Surname ="Smith" }
                };

                var actualResult = new List<dynamic>();
                RecursiveUtils.FindParents(entity27WithAllOfItsParents, actualResult);
                actualResult.Should().BeEquivalentTo(expectedResult);
            });
        }

        [FullyQualifiedTestMethod]
        public void Test_Method2()
        {
            // https://pgexercises.com/questions/recursive/getupward.html
            // Find the upward recommendation chain for member ID 27: that is, the member who recommended them,
            // and the member who recommended that member, and so on. Return member ID, first name,
            // and surname. Order by descending member id.

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

                var parentIdsQuery = memberHierarchyCte.Where(mh => mh.ChildId == 27 && mh.ParentId != null)
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
                                    [mh].[ChildId] = 27 AND [mh].[ParentId] IS NOT NULL AND
                                    [mh].[ParentId] = [member_3].[MemId]
                            )
                        ORDER BY
                            [member_3].[MemId] DESC
                */

                var expectedResult = new[]
                {
                    new { Recommender = 20, FirstName = "Matthew", Surname ="Genting" },
                    new { Recommender = 5, FirstName = "Gerald", Surname ="Butters" },
                    new { Recommender = 1, FirstName = "Darren", Surname ="Smith" }
                };

                parents.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}