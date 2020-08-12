using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCorePgExercises.DataLayer;
using System.Collections.Generic;
using FluentAssertions;
using EFCorePgExercises.Utils;
using LinqToDB.EntityFrameworkCore;
using LinqToDB;

namespace EFCorePgExercises.Exercises.RecursiveQueries
{
    [FullyQualifiedTestClass]
    public class FindTheDownwardRecommendationChainForMemberID1
    {
        [FullyQualifiedTestMethod]
        public void Test_Method1()
        {
            // https://pgexercises.com/questions/recursive/getdownward.html
            // Find the downward recommendation chain for member ID 1: that is,
            // the members they recommended, the members those members recommended,
            // and so on. Return member ID and name, and order by ascending member id.
            //
            //with recursive recommendeds(memid) as (
            //	select memid from cd.members where recommendedby = 1
            //	union all
            //	select mems.memid
            //		from recommendeds recs
            //		inner join cd.members mems
            //			on mems.recommendedby = recs.memid
            //)
            //select recs.memid, mems.firstname, mems.surname
            //	from recommendeds recs
            //	inner join cd.members mems
            //		on recs.memid = mems.memid
            //order by memid

            EFServiceProvider.RunInContext(context =>
            {
                var id = 1;
                var entity1WithAllOfItsDescendants =
                        context.Members
                            .Include(member => member.Children)
                            .Where(member => member.MemId == id
                                            || member.Children.Any(m => member.MemId == m.RecommendedBy))
                            .ToList() //It's a MUST - get all children from the database
                            .FirstOrDefault(x => x.MemId == id);// then get the root of the tree
                /*
                SELECT [m].[MemId], [m].[Address], [m].[FirstName], [m].[JoinDate], [m].[RecommendedBy], [m].[Surname], [m].[Telephone], [m].[ZipCode],
                    [m0].[MemId], [m0].[Address], [m0].[FirstName], [m0].[JoinDate], [m0].[RecommendedBy], [m0].[Surname], [m0].[Telephone], [m0].[ZipCode]
                        FROM [Members] AS [m]
                        LEFT JOIN [Members] AS [m0] ON [m].[MemId] = [m0].[RecommendedBy]
                        WHERE ([m].[MemId] = @__id_0) OR EXISTS (
                            SELECT 1
                            FROM [Members] AS [m1]
                            WHERE ([m].[MemId] = [m1].[RecommendedBy]) AND ([m].[MemId] = [m1].[RecommendedBy]))
                        ORDER BY [m].[MemId], [m0].[MemId]
                */

                var expectedResult = new[]
                {
                    new { MemId = 4, FirstName ="Janice", Surname ="Joplette" } ,
                    new { MemId = 5, FirstName ="Gerald", Surname ="Butters" } ,
                    new { MemId = 7, FirstName ="Nancy", Surname ="Dare" } ,
                    new { MemId = 10, FirstName ="Charles", Surname ="Owen" } ,
                    new { MemId = 11, FirstName ="David", Surname ="Jones" } ,
                    new { MemId = 14, FirstName ="Jack", Surname ="Smith" } ,
                    new { MemId = 20, FirstName ="Matthew", Surname ="Genting" } ,
                    new { MemId = 21, FirstName ="Anna", Surname ="Mackenzie" } ,
                    new { MemId = 26, FirstName ="Douglas", Surname ="Jones" } ,
                    new { MemId = 27, FirstName ="Henrietta", Surname ="Rumney" }
                };

                var actualResult = new List<dynamic>();
                RecursiveUtils.FindChildren(entity1WithAllOfItsDescendants, actualResult);
                var orderedActualResult = actualResult.OrderBy(x => x.MemId);
                orderedActualResult.Should().BeEquivalentTo(expectedResult);
            });
        }

        [FullyQualifiedTestMethod]
        public void Test_Method2()
        {
            // https://pgexercises.com/questions/recursive/getdownward.html
            // Find the downward recommendation chain for member ID 1: that is,
            // the members they recommended, the members those members recommended,
            // and so on. Return member ID and name, and order by ascending member id.

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

                var childrenIdsQuery = memberHierarchyCte.Where(mh => mh.ParentId == 1)
                                                        .Select(mh => mh.ChildId);

                var children = context.Members.Where(member => childrenIdsQuery.Contains(member.MemId))
                                                .Select(member => new
                                                {
                                                    member.MemId,
                                                    member.FirstName,
                                                    member.Surname
                                                })
                                                .OrderBy(result => result.MemId)
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
                                [mh].[ParentId] = 1 AND [mh].[ChildId] = [member_3].[MemId]
                        )
                    ORDER BY
                        [member_3].[MemId]
                */

                var expectedResult = new[]
                {
                    new { MemId = 4, FirstName ="Janice", Surname ="Joplette" } ,
                    new { MemId = 5, FirstName ="Gerald", Surname ="Butters" } ,
                    new { MemId = 7, FirstName ="Nancy", Surname ="Dare" } ,
                    new { MemId = 10, FirstName ="Charles", Surname ="Owen" } ,
                    new { MemId = 11, FirstName ="David", Surname ="Jones" } ,
                    new { MemId = 14, FirstName ="Jack", Surname ="Smith" } ,
                    new { MemId = 20, FirstName ="Matthew", Surname ="Genting" } ,
                    new { MemId = 21, FirstName ="Anna", Surname ="Mackenzie" } ,
                    new { MemId = 26, FirstName ="Douglas", Surname ="Jones" } ,
                    new { MemId = 27, FirstName ="Henrietta", Surname ="Rumney" }
                };

                children.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}