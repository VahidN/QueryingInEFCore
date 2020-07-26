using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.StringOperations
{
    [FullyQualifiedTestClass]
    public class CountNumberOfMembersWhoseSurnameStartsWithEachLetterOfTheAlphabet
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/string/substr.html
            // You'd like to produce a count of how many members you have whose surname starts with
            // each letter of the alphabet. Sort by the letter, and don't worry about printing out a
            // letter if the count is 0.
            //
            //select substr (mems.surname,1,1) as letter, count(*) as count
            //    from cd.members mems
            //    group by letter
            //    order by letter

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members
                                    .Select(member => new { Letter = member.Surname.Substring(0, 1) })
                                    .GroupBy(m => m.Letter)
                                    .Select(g => new
                                    {
                                        Letter = g.Key,
                                        Count = g.Count()
                                    })
                                    .OrderBy(r => r.Letter)
                                    .ToList();
                /*
                    SELECT SUBSTRING([m].[Surname], 0 + 1, 1) AS [Letter], COUNT(*) AS [Count]
                        FROM [Members] AS [m]
                        GROUP BY SUBSTRING([m].[Surname], 0 + 1, 1)
                        ORDER BY SUBSTRING([m].[Surname], 0 + 1, 1)
                */
                var expectedResult = new[]
                {
                    new { Letter = "B", Count = 5 },
                    new { Letter = "C", Count =     2 },
                    new { Letter = "D", Count =     1 },
                    new { Letter = "F", Count =     2 },
                    new { Letter = "G", Count =     2 },
                    new { Letter = "H", Count =     1 },
                    new { Letter = "J", Count =     3 },
                    new { Letter = "M", Count =     1 },
                    new { Letter = "O", Count =     1 },
                    new { Letter = "P", Count =     2 },
                    new { Letter = "R", Count =     2 },
                    new { Letter = "S", Count =     6 },
                    new { Letter = "T", Count =     2 },
                    new { Letter = "W", Count =     1 }
                };
                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}