using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class CombiningResultsFromMultipleQueries
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/union.html
            // You, for some reason, want a combined list of all surnames and all facility names.
            // Yes, this is a contrived example :-). Produce that list!
            // select surname from cd.members
            // union
            // select name	from cd.facilities;

            EFServiceProvider.RunInContext(context =>
            {
                var names = context.Members.Select(m => m.Surname).ToList()
                            .Union(context.Facilities.Select(f => f.Name).ToList()) // For now we have to use `.ToList()` here
                            .ToList();

                string[] expectedResult =
                {
                    "Tennis Court 2",
                    "Worthington-Smyth",
                    "Badminton Court",
                    "Pinker",
                    "Dare",
                    "Bader",
                    "Mackenzie",
                    "Crumpet",
                    "Massage Room 1",
                    "Squash Court",
                    "Tracy",
                    "Hunt",
                    "Tupperware",
                    "Smith",
                    "Butters",
                    "Rownam",
                    "Baker",
                    "Genting",
                    "Purview",
                    "Coplin",
                    "Massage Room 2",
                    "Joplette",
                    "Stibbons",
                    "Rumney",
                    "Pool Table",
                    "Sarwin",
                    "Boothe",
                    "Farrell",
                    "Tennis Court 1",
                    "Snooker Table",
                    "Owen",
                    "Table Tennis",
                    "GUEST",
                    "Jones"
                };

                names.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}