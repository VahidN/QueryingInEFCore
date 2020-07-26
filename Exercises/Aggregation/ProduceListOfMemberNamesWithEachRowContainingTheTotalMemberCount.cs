using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class ProduceListOfMemberNamesWithEachRowContainingTheTotalMemberCount
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/countmembers.html
            // Produce a list of member names, with each row containing the total member count.
            // Order by join date.
            //
            //select count(*) over(), firstname, surname
            //	from cd.members
            //order by joindate
            //
            // OR
            //
            //select (select count(*) from cd.members) as count, firstname, surname
            //	from cd.members
            //order by joindate

            EFServiceProvider.RunInContext(context =>
            {
                var members = context.Members
                        .OrderBy(member => member.JoinDate)
                        .Select(member => new
                        {
                            Count = context.Members.Count(),
                            member.FirstName,
                            member.Surname
                        })
                        .ToList();
                /*
                    SELECT COUNT(*)
                        FROM [Members] AS [m]

                    SELECT [m].[FirstName], [m].[Surname], @__Count_0 AS [Count]
                        FROM [Members] AS [m]
                        ORDER BY [m].[JoinDate]
                */

                var expectedResult = new[]
                {
                    new { Count = 31, FirstName = "GUEST" , Surname="GUEST" } ,
                    new { Count = 31, FirstName = "Darren" , Surname="Smith" } ,
                    new { Count = 31, FirstName = "Tracy" , Surname="Smith" } ,
                    new { Count = 31, FirstName = "Tim" , Surname="Rownam" } ,
                    new { Count = 31, FirstName = "Janice" , Surname="Joplette" } ,
                    new { Count = 31, FirstName = "Gerald" , Surname="Butters" } ,
                    new { Count = 31, FirstName = "Burton" , Surname="Tracy" } ,
                    new { Count = 31, FirstName = "Nancy" , Surname="Dare" } ,
                    new { Count = 31, FirstName = "Tim" , Surname="Boothe" } ,
                    new { Count = 31, FirstName = "Ponder" , Surname="Stibbons" } ,
                    new { Count = 31, FirstName = "Charles" , Surname="Owen" } ,
                    new { Count = 31, FirstName = "David" , Surname="Jones" } ,
                    new { Count = 31, FirstName = "Anne" , Surname="Baker" } ,
                    new { Count = 31, FirstName = "Jemima" , Surname="Farrell" } ,
                    new { Count = 31, FirstName = "Jack" , Surname="Smith" } ,
                    new { Count = 31, FirstName = "Florence" , Surname="Bader" } ,
                    new { Count = 31, FirstName = "Timothy" , Surname="Baker" } ,
                    new { Count = 31, FirstName = "David" , Surname="Pinker" } ,
                    new { Count = 31, FirstName = "Matthew" , Surname="Genting" } ,
                    new { Count = 31, FirstName = "Anna" , Surname="Mackenzie" } ,
                    new { Count = 31, FirstName = "Joan" , Surname="Coplin" } ,
                    new { Count = 31, FirstName = "Ramnaresh" , Surname="Sarwin" } ,
                    new { Count = 31, FirstName = "Douglas" , Surname="Jones" } ,
                    new { Count = 31, FirstName = "Henrietta" , Surname="Rumney" } ,
                    new { Count = 31, FirstName = "David" , Surname="Farrell" } ,
                    new { Count = 31, FirstName = "Henry" , Surname="Worthington-Smyth" } ,
                    new { Count = 31, FirstName = "Millicent" , Surname="Purview" } ,
                    new { Count = 31, FirstName = "Hyacinth" , Surname="Tupperware" } ,
                    new { Count = 31, FirstName = "John" , Surname="Hunt" } ,
                    new { Count = 31, FirstName = "Erica" , Surname="Crumpet" } ,
                    new { Count = 31, FirstName = "Darren" , Surname="Smith" }
                };

                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}