using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class FindTheCountOfMembersWhoHaveMadeAtLeastOneBooking
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/members1.html
            // Find the total number of members who have made at least one booking.
            // select count(distinct memid) from cd.bookings

            EFServiceProvider.RunInContext(context =>
            {
                var count = context.Bookings.Select(booking => booking.MemId).Distinct().Count();
                /*
                    SELECT COUNT(*)
                        FROM (
                            SELECT DISTINCT [b].[MemId]
                            FROM [Bookings] AS [b]
                        ) AS [t]
                */
                count.Should().Be(30);
            });
        }
    }
}