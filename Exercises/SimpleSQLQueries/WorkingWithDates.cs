using System;
using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using System.Globalization;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class WorkingWithDates
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/date.html
            // How can you produce a list of members who joined after the start of September 2012?
            // Return the memid, surname, firstname, and joindate of the members in question.
            // select memid, surname, firstname, joindate from cd.members where joindate >= '2012-09-01';

            EFServiceProvider.RunInContext(context =>
            {
                var date = new DateTime(2012, 09, 01);
                var members = context.Members.Where(x => x.JoinDate >= date)
                                            .Select(x =>
                                                        new
                                                        {
                                                            x.MemId,
                                                            x.Surname,
                                                            x.FirstName,
                                                            x.JoinDate
                                                        }).ToList();
                /*
                    SELECT [m].[MemId], [m].[Surname], [m].[FirstName], [m].[JoinDate]
                    FROM [Members] AS [m]
                    WHERE [m].[JoinDate] >= @__date_0
                */
                var expectedResult = new[]
                {
                    new { MemId = 24, Surname ="Sarwin", FirstName = "Ramnaresh", JoinDate = DateTime.Parse("2012-09-01 08:44:42", CultureInfo.InvariantCulture) } ,
                    new { MemId = 26, Surname ="Jones", FirstName = "Douglas", JoinDate = DateTime.Parse("2012-09-02 18:43:05", CultureInfo.InvariantCulture) } ,
                    new { MemId = 27, Surname ="Rumney", FirstName = "Henrietta", JoinDate = DateTime.Parse("2012-09-05 08:42:35", CultureInfo.InvariantCulture) } ,
                    new { MemId = 28, Surname ="Farrell", FirstName = "David", JoinDate = DateTime.Parse("2012-09-15 08:22:05", CultureInfo.InvariantCulture) } ,
                    new { MemId = 29, Surname ="Worthington-Smyth", FirstName = "Henry", JoinDate = DateTime.Parse("2012-09-17 12:27:15", CultureInfo.InvariantCulture) } ,
                    new { MemId = 30, Surname ="Purview", FirstName = "Millicent", JoinDate = DateTime.Parse("2012-09-18 19:04:01", CultureInfo.InvariantCulture) } ,
                    new { MemId = 33, Surname ="Tupperware", FirstName = "Hyacinth", JoinDate = DateTime.Parse("2012-09-18 19:32:05", CultureInfo.InvariantCulture) } ,
                    new { MemId = 35, Surname ="Hunt", FirstName = "John", JoinDate = DateTime.Parse("2012-09-19 11:32:45", CultureInfo.InvariantCulture) } ,
                    new { MemId = 36, Surname ="Crumpet", FirstName = "Erica", JoinDate = DateTime.Parse("2012-09-22 08:36:38", CultureInfo.InvariantCulture) } ,
                    new { MemId = 37, Surname ="Smith", FirstName = "Darren", JoinDate = DateTime.Parse("2012-09-26 18:08:45", CultureInfo.InvariantCulture) }
                };
                members.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}