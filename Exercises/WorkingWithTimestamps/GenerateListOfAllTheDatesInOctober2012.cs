using System.Linq;
using System;
using System.Collections.Generic;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class GenerateListOfAllTheDatesInOctober2012
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/series.html
            // Produce a list of all the dates in October 2012. They can be output as
            // a timestamp (with time set to midnight) or a date.
            //
            // select generate_series(timestamp '2012-10-01', timestamp '2012-10-31', interval '1 day') as ts;

            EFServiceProvider.RunInContext(context =>
            {
                var date1 = new DateTime(2012, 10, 01);
                var date2 = new DateTime(2012, 10, 31);
                var items = context.Bookings
                    .Where(x => x.StartTime >= date1 && x.StartTime <= date2)
                    .Select(x => new { x.StartTime.Date })
                    .Distinct()
                    .ToList();
                /*
                    SELECT DISTINCT CONVERT(date, [b].[StartTime]) AS [Date]
                        FROM [Bookings] AS [b]
                        WHERE ([b].[StartTime] >= @__date1_0) AND ([b].[StartTime] <= @__date2_1)
                */
                foreach (var item in items)
                {
                    Console.WriteLine($"Date: {item.Date}");
                }
            });
        }
    }
}