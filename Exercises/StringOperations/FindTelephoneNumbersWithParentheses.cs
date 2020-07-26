using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.StringOperations
{
    [FullyQualifiedTestClass]
    public class FindTelephoneNumbersWithParentheses
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/string/reg.html
            // You've noticed that the club's member table has telephone numbers with very inconsistent
            // formatting. You'd like to find all the telephone numbers that contain parentheses,
            // returning the member ID and telephone number sorted by member ID.
            //
            // select memid, telephone from cd.members where telephone ~ '[()]';
            //
            // select memid, telephone from cd.members where telephone similar to '%[()]%';

            EFServiceProvider.RunInContext(context =>
            {
                // Method 1
                var members = context.Members
                                    .Select(member => new { member.MemId, member.Telephone })
                                    .Where(member => member.Telephone.Contains("(")
                                                    && member.Telephone.Contains(")"))
                                    .ToList();
                /*
                SELECT [m].[MemId], [m].[Telephone]
                    FROM [Members] AS [m]
                    WHERE (CHARINDEX(N'(', [m].[Telephone]) > 0) AND (CHARINDEX(N')', [m].[Telephone]) > 0)
                */
                var expectedResult = new[]
                {
                    new { MemId = 0, Telephone = "(000) 000-0000" },
                    new { MemId = 3, Telephone = "(844) 693-0723" },
                    new { MemId = 4, Telephone = "(833) 942-4710" },
                    new { MemId = 5, Telephone = "(844) 078-4130" },
                    new { MemId = 6, Telephone = "(822) 354-9973" },
                    new { MemId = 7, Telephone = "(833) 776-4001" },
                    new { MemId = 8, Telephone = "(811) 433-2547" },
                    new { MemId = 9, Telephone = "(833) 160-3900" },
                    new { MemId = 10, Telephone = "(855) 542-5251" },
                    new { MemId = 11, Telephone = "(844) 536-8036" },
                    new { MemId = 13, Telephone = "(855) 016-0163" },
                    new { MemId = 14, Telephone = "(822) 163-3254" },
                    new { MemId = 15, Telephone = "(833) 499-3527" },
                    new { MemId = 20, Telephone = "(811) 972-1377" },
                    new { MemId = 21, Telephone = "(822) 661-2898" },
                    new { MemId = 22, Telephone = "(822) 499-2232" },
                    new { MemId = 24, Telephone = "(822) 413-1470" },
                    new { MemId = 27, Telephone = "(822) 989-8876" },
                    new { MemId = 28, Telephone = "(855) 755-9876" },
                    new { MemId = 29, Telephone = "(855) 894-3758" },
                    new { MemId = 30, Telephone = "(855) 941-9786" },
                    new { MemId = 33, Telephone = "(822) 665-5327" },
                    new { MemId = 35, Telephone = "(899) 720-6978" },
                    new { MemId = 36, Telephone = "(811) 732-4816" },
                    new { MemId = 37, Telephone = "(822) 577-3541" }
                };

                members.Should().BeEquivalentTo(expectedResult);

                // Method 2
                // `Like` query supports wildcard characters and hence very useful compared to the string extension methods in some scenarios.
                members = context.Members
                                    .Select(member => new { member.MemId, member.Telephone })
                                    .Where(member => EF.Functions.Like(member.Telephone, "%[()]%"))
                                    .ToList();
                /*
                    SELECT [m].[MemId], [m].[Telephone]
                        FROM [Members] AS [m]
                        WHERE [m].[Telephone] LIKE N'%[()]%'
                */
                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}