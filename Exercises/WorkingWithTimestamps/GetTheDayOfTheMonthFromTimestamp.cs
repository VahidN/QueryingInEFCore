using System.Linq;
using System;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class GetTheDayOfTheMonthFromTimestamp
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/extract.html
            // Get the day of the month from the timestamp '2012-08-31' as an integer.
            //
            // select extract(day from timestamp '2012-08-31');

            EFServiceProvider.RunInContext(context =>
            {
                var date1 = new DateTime(2012, 08, 31, 01, 00, 00);
                var item = context.Bookings
                    .Where(x => x.StartTime >= date1)
                    .Select(x => new
                    {
                        date1.Day,
                        x.StartTime
                    }).FirstOrDefault();
                /*
                SELECT TOP(1) @__date1_Day_1 AS [Day], [b].[StartTime]
                    FROM [Bookings] AS [b]
                    WHERE [b].[StartTime] >= @__date1_0
                */
                Console.WriteLine($"Day: {item.Day}, StartTime: {item.StartTime}");
            });
        }
    }
}