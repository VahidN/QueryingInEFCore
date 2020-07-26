using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.StringOperations
{
    [FullyQualifiedTestClass]
    public class CleanupTelephoneNumbers
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/string/translate.html
            // The telephone numbers in the database are very inconsistently formatted.
            // You'd like to print a list of member ids and numbers that have had '-','(',')',
            // and ' ' characters removed. Order by member id.
            //
            //select memid, translate(telephone, '-() ', '') as telephone
            //    from cd.members
            //    order by memid;
            //
            //select memid, regexp_replace(telephone, '[^0-9]', '', 'g') as telephone
            //    from cd.members
            //    order by memid;

            EFServiceProvider.RunInContext(context =>
            {
                // TODO: Use regexp_replace. SQL Server doesn't have a native regexp_replace function!
                var members = context.Members
                                .Select(member => new
                                {
                                    member.MemId,
                                    Telephone = member.Telephone.Replace("-", "")
                                                        .Replace("(", "")
                                                        .Replace(")", "")
                                                        .Replace(" ", "")
                                })
                                .OrderBy(r => r.MemId)
                                .ToList();
                /*
                SELECT [m].[MemId], REPLACE(REPLACE(REPLACE(REPLACE([m].[Telephone], N'-', N''), N'(', N''), N')', N''), N' ', N'') AS [Telephone]
                    FROM [Members] AS [m]
                    ORDER BY [m].[MemId]
                */

                var expectedResult = new[]
                {
                    new { MemId = 0, Telephone = "0000000000" },
                    new { MemId = 1, Telephone = "5555555555" },
                    new { MemId = 2, Telephone = "5555555555" },
                    new { MemId = 3, Telephone = "8446930723" },
                    new { MemId = 4, Telephone = "8339424710" },
                    new { MemId = 5, Telephone = "8440784130" },
                    new { MemId = 6, Telephone = "8223549973" },
                    new { MemId = 7, Telephone = "8337764001" },
                    new { MemId = 8, Telephone = "8114332547" },
                    new { MemId = 9, Telephone = "8331603900" },
                    new { MemId = 10, Telephone = "8555425251" },
                    new { MemId = 11, Telephone = "8445368036" },
                    new { MemId = 12, Telephone = "8440765141" },
                    new { MemId = 13, Telephone = "8550160163" },
                    new { MemId = 14, Telephone = "8221633254" },
                    new { MemId = 15, Telephone = "8334993527" },
                    new { MemId = 16, Telephone = "8339410824" },
                    new { MemId = 17, Telephone = "8114096734" },
                    new { MemId = 20, Telephone = "8119721377" },
                    new { MemId = 21, Telephone = "8226612898" },
                    new { MemId = 22, Telephone = "8224992232" },
                    new { MemId = 24, Telephone = "8224131470" },
                    new { MemId = 26, Telephone = "8445368036" },
                    new { MemId = 27, Telephone = "8229898876" },
                    new { MemId = 28, Telephone = "8557559876" },
                    new { MemId = 29, Telephone = "8558943758" },
                    new { MemId = 30, Telephone = "8559419786" },
                    new { MemId = 33, Telephone = "8226655327" },
                    new { MemId = 35, Telephone = "8997206978" },
                    new { MemId = 36, Telephone = "8117324816" },
                    new { MemId = 37, Telephone = "8225773541" }
                };
                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}