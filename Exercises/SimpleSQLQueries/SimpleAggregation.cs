using System;
using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using System.Globalization;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.SimpleSQLQueries
{
    [FullyQualifiedTestClass]
    public class SimpleAggregation
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/basic/agg.html
            // You'd like to get the signup date of your last member. How can you retrieve this information?
            // select max(joindate) as latest from cd.members;

            EFServiceProvider.RunInContext(context =>
            {
                var latest = context.Members.Max(x => x.JoinDate);
                /*
                    Returns null ― for nullable overloads
                    Throws Sequence contains no element exception ― for non-nullable overloads
                */
                /*
                    SELECT MAX([m].[JoinDate])
                    FROM [Members] AS [m]
                */
                var expectedResult = DateTime.Parse("2012-09-26 18:08:45", CultureInfo.InvariantCulture);
                latest.Should().Be(expectedResult);

                var latest2 = context.Members.Select(m => m.JoinDate).DefaultIfEmpty().Max();
                /*
                    SELECT MAX([m].[JoinDate])
                    FROM (
                        SELECT NULL AS [empty]
                    ) AS [empty]
                    LEFT JOIN [Members] AS [m] ON 1 = 1
                */
                latest2.Should().Be(expectedResult);


                var latest3 = context.Members.Max(m => (DateTime?)m.JoinDate) ?? DateTime.Now;
                /*
                    SELECT MAX([m].[JoinDate])
                    FROM [Members] AS [m]
                */
                latest3.Should().Be(expectedResult);

                var latest4 = context.Members.OrderByDescending(m => m.JoinDate).Select(m => m.JoinDate).FirstOrDefault();
                /*
                    SELECT TOP(1) [m].[JoinDate]
                    FROM [Members] AS [m]
                    ORDER BY [m].[JoinDate] DESC
                */
                latest4.Should().Be(expectedResult);
            });
        }
    }
}