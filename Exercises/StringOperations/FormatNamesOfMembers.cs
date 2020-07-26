using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.StringOperations
{
    [FullyQualifiedTestClass]
    public class FormatNamesOfMembers
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/string/concat.html
            // Output the names of all members, formatted as 'Surname, Firstname'
            //
            // select surname || ', ' || firstname as name from cd.members

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members
                                    .Select(member => new { Name = member.Surname + ", " + member.FirstName })
                                    .ToList();
                /*
                SELECT ([m].[Surname] + N', ') + [m].[FirstName] AS [Name]
                    FROM [Members] AS [m]
                */
                var expectedResult = new[]
                {
                    new { Name = "GUEST, GUEST" },
                    new { Name = "Smith, Darren" },
                    new { Name = "Smith, Tracy" },
                    new { Name = "Rownam, Tim" },
                    new { Name = "Joplette, Janice" },
                    new { Name = "Butters, Gerald" },
                    new { Name = "Tracy, Burton" },
                    new { Name = "Dare, Nancy" },
                    new { Name = "Boothe, Tim" },
                    new { Name = "Stibbons, Ponder" },
                    new { Name = "Owen, Charles" },
                    new { Name = "Jones, David" },
                    new { Name = "Baker, Anne" },
                    new { Name = "Farrell, Jemima" },
                    new { Name = "Smith, Jack" },
                    new { Name = "Bader, Florence" },
                    new { Name = "Baker, Timothy" },
                    new { Name = "Pinker, David" },
                    new { Name = "Genting, Matthew" },
                    new { Name = "Mackenzie, Anna" },
                    new { Name = "Coplin, Joan" },
                    new { Name = "Sarwin, Ramnaresh" },
                    new { Name = "Jones, Douglas" },
                    new { Name = "Rumney, Henrietta" },
                    new { Name = "Farrell, David" },
                    new { Name = "Worthington-Smyth, Henry" },
                    new { Name = "Purview, Millicent" },
                    new { Name = "Tupperware, Hyacinth" },
                    new { Name = "Hunt, John" },
                    new { Name = "Crumpet, Erica" },
                    new { Name = "Smith, Darren" }
                };
                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}