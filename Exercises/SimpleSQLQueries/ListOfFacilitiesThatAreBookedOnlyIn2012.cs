using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;
using System;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class ListOfFacilitiesThatAreBookedOnlyIn2012
    {
        [FullyQualifiedTestMethod]
        public void Test_Method1()
        {
            EFServiceProvider.RunInContext(context =>
            {
                // The list of facilities that are booked only in 2012.
                //
                //SELECT Name
                //FROM   Facilities
                //WHERE  FacId IN (SELECT FacId
                //                 FROM   Bookings
                //                 EXCEPT
                //                 SELECT FacId
                //                 FROM   Bookings
                //                 WHERE  YEAR(StartTime) <> 2012);

                var facilitiesBookedNotIn2012Query = context.Bookings
                            .Where(booking => booking.StartTime.Year != 2012)
                            .Select(Booking => Booking.FacId);

                var facilitiesBookedOnlyIn2012Query = context.Bookings
                            .Where(booking => !facilitiesBookedNotIn2012Query.Contains(booking.FacId))
                            .Select(Booking => Booking.FacId);

                var facilitiesBookedOnlyIn2012 = context.Facilities
                            .Where(facility => facilitiesBookedOnlyIn2012Query.Contains(facility.FacId))
                            .Select(facility => facility.Name)
                            .ToList();

                /*
                    SELECT [f].[Name]
                        FROM [Facilities] AS [f]
                        WHERE [f].[FacId] IN (
                            SELECT [b].[FacId]
                            FROM [Bookings] AS [b]
                            WHERE [b].[FacId] NOT IN (
                                SELECT [b0].[FacId]
                                FROM [Bookings] AS [b0]
                                WHERE (DATEPART(year, [b0].[StartTime]) <> 2012) OR DATEPART(year, [b0].[StartTime]) IS NULL
                            )
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

                facilitiesBookedOnlyIn2012.Should().BeEquivalentTo(expectedResult);
            });
        }

        [FullyQualifiedTestMethod]
        public void Test_Method2()
        {
            EFServiceProvider.RunInContext(context =>
            {
                // The list of facilities that are booked only in 2012.
                //
                //SELECT Name
                //FROM   Facilities
                //WHERE  FacId IN (SELECT FacId
                //                 FROM   Bookings
                //                 EXCEPT
                //                 SELECT FacId
                //                 FROM   Bookings
                //                 WHERE  YEAR(StartTime) <> 2012);

                var facilitiesBookedNotIn2012Query = context.Bookings
                            .Where(booking => booking.StartTime.Year != 2012)
                            .Select(Booking => Booking.FacId);

                var facilitiesBookedOnlyIn2012Query =
                    context.Bookings.Select(Booking => Booking.FacId).Except(facilitiesBookedNotIn2012Query);

                var facilitiesBookedOnlyIn2012 = context.Facilities
                            .Where(facility => facilitiesBookedOnlyIn2012Query.Contains(facility.FacId))
                            .Select(facility => facility.Name)
                            .ToList();

                /*
                    SELECT [f].[Name]
                        FROM [Facilities] AS [f]
                        WHERE [f].[FacId] IN (
                            SELECT [b].[FacId]
                            FROM [Bookings] AS [b]
                            EXCEPT
                            SELECT [b0].[FacId]
                            FROM [Bookings] AS [b0]
                            WHERE (DATEPART(year, [b0].[StartTime]) <> 2012) OR DATEPART(year, [b0].[StartTime]) IS NULL
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

                facilitiesBookedOnlyIn2012.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}