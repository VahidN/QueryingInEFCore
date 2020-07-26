using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ProduceNumberedListOfMembers
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/nummembers.html
            // Produce a monotonically increasing numbered list of members, ordered by their date of joining.
            // Remember that member IDs are not guaranteed to be sequential.
            //
            //select row_number() over(order by joindate), firstname, surname
            //	from cd.members
            //order by joindate

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members
                        .OrderBy(member => member.JoinDate)
                        .Select(member => new
                        {
                            member.FirstName,
                            member.Surname
                        })
                        .ToList()
                        /*
                            SELECT [m].[FirstName], [m].[Surname]
                                FROM [Members] AS [m]
                                ORDER BY [m].[JoinDate]
                        */
                        // Now using LINQ to Objects
                        .Select((member, index) => new
                        {
                            RowNumber = index + 1,
                            member.FirstName,
                            member.Surname
                        })
                        .ToList();

                var expectedResult = new[]
                {
                    new { RowNumber = 1, FirstName = "GUEST" , Surname="GUEST" } ,
                    new { RowNumber = 2, FirstName = "Darren" , Surname="Smith" } ,
                    new { RowNumber = 3, FirstName = "Tracy" , Surname="Smith" } ,
                    new { RowNumber = 4, FirstName = "Tim" , Surname="Rownam" } ,
                    new { RowNumber = 5, FirstName = "Janice" , Surname="Joplette" } ,
                    new { RowNumber = 6, FirstName = "Gerald" , Surname="Butters" } ,
                    new { RowNumber = 7, FirstName = "Burton" , Surname="Tracy" } ,
                    new { RowNumber = 8, FirstName = "Nancy" , Surname="Dare" } ,
                    new { RowNumber = 9, FirstName = "Tim" , Surname="Boothe" } ,
                    new { RowNumber = 10, FirstName = "Ponder" , Surname="Stibbons" } ,
                    new { RowNumber = 11, FirstName = "Charles" , Surname="Owen" } ,
                    new { RowNumber = 12, FirstName = "David" , Surname="Jones" } ,
                    new { RowNumber = 13, FirstName = "Anne" , Surname="Baker" } ,
                    new { RowNumber = 14, FirstName = "Jemima" , Surname="Farrell" } ,
                    new { RowNumber = 15, FirstName = "Jack" , Surname="Smith" } ,
                    new { RowNumber = 16, FirstName = "Florence" , Surname="Bader" } ,
                    new { RowNumber = 17, FirstName = "Timothy" , Surname="Baker" } ,
                    new { RowNumber = 18, FirstName = "David" , Surname="Pinker" } ,
                    new { RowNumber = 19, FirstName = "Matthew" , Surname="Genting" } ,
                    new { RowNumber = 20, FirstName = "Anna" , Surname="Mackenzie" } ,
                    new { RowNumber = 21, FirstName = "Joan" , Surname="Coplin" } ,
                    new { RowNumber = 22, FirstName = "Ramnaresh" , Surname="Sarwin" } ,
                    new { RowNumber = 23, FirstName = "Douglas" , Surname="Jones" } ,
                    new { RowNumber = 24, FirstName = "Henrietta" , Surname="Rumney" } ,
                    new { RowNumber = 25, FirstName = "David" , Surname="Farrell" } ,
                    new { RowNumber = 26, FirstName = "Henry" , Surname="Worthington-Smyth" } ,
                    new { RowNumber = 27, FirstName = "Millicent" , Surname="Purview" } ,
                    new { RowNumber = 28, FirstName = "Hyacinth" , Surname="Tupperware" } ,
                    new { RowNumber = 29, FirstName = "John" , Surname="Hunt" } ,
                    new { RowNumber = 30, FirstName = "Erica" , Surname="Crumpet" } ,
                    new { RowNumber = 31, FirstName = "Darren" , Surname="Smith" }
                };

                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}