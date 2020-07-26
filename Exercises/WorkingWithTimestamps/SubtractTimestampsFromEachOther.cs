using System.Linq;
using System;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class SubtractTimestampsFromEachOther
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/interval.html
            // Find the result of subtracting the timestamp '2012-07-30 01:00:00' from
            // the timestamp '2012-08-31 01:00:00'
            //
            // select timestamp '2012-08-31 01:00:00' - timestamp '2012-07-30 01:00:00' as interval;

            EFServiceProvider.RunInContext(context =>
            {
                var date1 = new DateTime(2012, 08, 31, 01, 00, 00);
                var date2 = new DateTime(2012, 07, 30, 01, 00, 00);
                var item = context.Bookings
                    .Where(x => x.StartTime >= date2 && x.StartTime <= date1)
                    .Select(x => new
                    {
                        Interval = (date1 - date2).Days,
                        x.StartTime
                    }).FirstOrDefault();
                /*
                Executed DbCommand (114ms) [Parameters=[@__Days_2='32', @__date2_0='2012-07-30T01:00:00', @__date1_1='2012-08-31T01:00:00'], CommandType='Text', CommandTimeout='30']
                    SELECT TOP(1) @__Days_2 AS [Interval], [b].[StartTime]
                    FROM [Bookings] AS [b]
                    WHERE ([b].[StartTime] >= @__date2_0) AND ([b].[StartTime] <= @__date1_1)
                */
                Console.WriteLine($"Interval: {item.Interval}");
            });
        }
    }
}