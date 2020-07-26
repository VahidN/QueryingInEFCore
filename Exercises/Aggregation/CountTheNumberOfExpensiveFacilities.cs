using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class CountTheNumberOfExpensiveFacilities
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/count2.html
            // Produce a count of the number of facilities that have a cost to guests of 10 or more.
            // select count(*) from cd.facilities where guestcost >= 10;

            EFServiceProvider.RunInContext(context =>
            {
                var count = context.Facilities.Count(x => x.GuestCost >= 10);
                /*
                    SELECT COUNT(*)
                    FROM [Facilities] AS [f]
                    WHERE [f].[GuestCost] >= 10.0
                */
                Assert.AreEqual(expected: 6, actual: count);
            });
        }
    }
}