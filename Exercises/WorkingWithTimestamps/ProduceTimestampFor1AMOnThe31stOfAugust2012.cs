using System.Linq;
using System;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class ProduceTimestampFor1AMOnThe31stOfAugust2012
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/timestamp.html
            // Produce a timestamp for 1 a.m. on the 31st of August 2012.
            //
            // select timestamp '2012-08-31 01:00:00';
            // select '2012-08-31 01:00:00'::timestamp;
            // select cast('2012-08-31 01:00:00' as timestamp);

            EFServiceProvider.RunInContext(context =>
            {
                var date1 = new DateTime(2012, 08, 31, 01, 00, 00);
                var item = context.Bookings
                    .Where(x => x.StartTime >= date1)
                    .Select(x => new
                    {
                        Timestamp = date1,
                        x.StartTime
                    }).FirstOrDefault();
                /*
                SELECT TOP(1) @__date1_0 AS [Timestamp], [b].[StartTime]
                    FROM [Bookings] AS [b]
                    WHERE [b].[StartTime] >= @__date1_0
                */
                Console.WriteLine($"Timestamp: {item.Timestamp}, StartTime: {item.StartTime}");
            });
        }
    }
}