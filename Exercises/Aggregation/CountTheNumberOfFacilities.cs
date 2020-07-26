using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class CountTheNumberOfFacilities
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/count.html
            // For our first foray into aggregates, we're going to stick to something simple.
            // We want to know how many facilities exist - simply produce a total count.
            // select count(*) from cd.facilities;

            EFServiceProvider.RunInContext(context =>
            {
                var count = context.Facilities.Count();
                /*
                    SELECT COUNT(*)
                    FROM [Facilities] AS [f]
                */
                Assert.AreEqual(expected: 9, actual: count);
            });
        }
    }
}