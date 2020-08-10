using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;
using System;

namespace EFCorePgExercises.Exercises.JoinsAndSubqueries
{
    [FullyQualifiedTestClass]
    public class ListOfFacilitiesWhichAreNotBookedIn2013
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            EFServiceProvider.RunInContext(context =>
            {
                // The list of facilities which are not booked in 2013.
                //
                //SELECT f.Name
                //FROM   Facilities AS f
                //       LEFT JOIN
                //       Bookings AS b
                //       ON f.FacId = b.FacId
                //          AND YEAR(b.StartTime) = 2013
                //WHERE  b.StartTime IS NULL;

                var facilitiesBookedIn2013Query = context.Bookings
                            .Where(booking => booking.StartTime.Year == 2013)
                            .Select(booking => booking.Facility.FacId);

                var facilitiesNotBookedIn2013 =
                        context.Facilities
                                .Where(facility => !facilitiesBookedIn2013Query.Contains(facility.FacId))
                                .Select(facility => facility.Name)
                                .ToList();

                /*
                    SELECT [f].[Name]
                        FROM [Facilities] AS [f]
                        WHERE [f].[FacId] NOT IN (
                            SELECT [f0].[FacId]
                            FROM [Bookings] AS [b]
                            INNER JOIN [Facilities] AS [f0] ON [b].[FacId] = [f0].[FacId]
                            WHERE DATEPART(year, [b].[StartTime]) = 2013
                        )
                */

                string[] expectedResult =
                {
                    "Tennis Court 1",
                    "Tennis Court 2",
                    "Badminton Court",
                    "Table Tennis",
                    "Massage Room 1",
                    "Massage Room 2",
                    "Squash Court",
                    "Snooker Table"
                };

                facilitiesNotBookedIn2013.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}