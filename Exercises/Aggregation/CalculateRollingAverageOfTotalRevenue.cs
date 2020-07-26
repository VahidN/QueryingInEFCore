using System.Linq;
using System;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class CalculateRollingAverageOfTotalRevenue
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/rollingavg.html
            // For each day in August 2012, calculate a rolling average of total revenue over the previous 15 days.
            // Output should contain date and revenue columns, sorted by the date. Remember to account for
            // the possibility of a day having zero revenue. This one's a bit tough, so don't be afraid
            // to check out the hint!
            //select 	dategen.date,
            //	(
            //		-- correlated subquery that, for each day fed into it,
            //		-- finds the average revenue for the last 15 days
            //		select sum(case
            //			when memid = 0 then slots * facs.guestcost
            //			else slots * membercost
            //		end) as rev
            //
            //		from cd.bookings bks
            //		inner join cd.facilities facs
            //			on bks.facid = facs.facid
            //		where bks.starttime > dategen.date - interval '14 days'
            //			and bks.starttime < dategen.date + interval '1 day'
            //	)/15 as revenue
            //	from
            //	(
            //		-- generates a list of days in august
            //		select 	cast(generate_series(timestamp '2012-08-01',
            //			'2012-08-31','1 day') as date) as date
            //	)  as dategen
            //order by dategen.date;
            //
            //
            //select date, avgrev from (
            //	-- AVG over this row and the 14 rows before it.
            //	select 	dategen.date as date,
            //		avg(revdata.rev) over(order by dategen.date rows 14 preceding) as avgrev
            //	from
            //		-- generate a list of days.  This ensures that a row gets generated
            //		-- even if the day has 0 revenue.  Note that we generate days before
            //		-- the start of october - this is because our window function needs
            //		-- to know the revenue for those days for its calculations.
            //		(select
            //			cast(generate_series(timestamp '2012-07-10', '2012-08-31','1 day') as date) as date
            //		)  as dategen
            //		left outer join
            //			-- left join to a table of per-day revenue
            //			(select cast(bks.starttime as date) as date,
            //				sum(case
            //					when memid = 0 then slots * facs.guestcost
            //					else slots * membercost
            //				end) as rev
            //
            //				from cd.bookings bks
            //				inner join cd.facilities facs
            //					on bks.facid = facs.facid
            //				group by cast(bks.starttime as date)
            //			) as revdata
            //			on dategen.date = revdata.date
            //	) as subq
            //	where date >= '2012-08-01'
            //order by date;
            //
            //
            //create or replace view cd.dailyrevenue as
            //	select 	cast(bks.starttime as date) as date,
            //		sum(case
            //			when memid = 0 then slots * facs.guestcost
            //			else slots * membercost
            //		end) as rev
            //
            //		from cd.bookings bks
            //		inner join cd.facilities facs
            //			on bks.facid = facs.facid
            //		group by cast(bks.starttime as date);
            //
            //select date, avgrev from (
            //	select  dategen.date as date,
            //		avg(revdata.rev) over(order by dategen.date rows 14 preceding) as avgrev
            //	from
            //		(select
            //			cast(generate_series(timestamp '2012-07-10', '2012-08-31','1 day') as date) as date
            //		)  as dategen
            //		left outer join
            //			cd.dailyrevenue as revdata on dategen.date = revdata.date
            //		) as subq
            //	where date >= '2012-08-01'
            //order by date;
            //
            // OR
            //
            //create view DailyRevenue
            //as
            //SELECT CONVERT(date, [b].[StartTime]) AS [Date], SUM(CASE
            //                  WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
            //                  ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
            //              END) AS [TotalRevenue]
            //              FROM [Bookings] AS [b]
            //              INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
            //              GROUP BY CONVERT(date, [b].[StartTime])

            //select *
            //        , (select avg(c2.[TotalRevenue]) from DailyRevenue c2
            //            where c2.[date] between dateadd(dd, -14, c1.[date]) and c1.[date]) mov_avg
            //from DailyRevenue c1
            //order by c1.[date]


            EFServiceProvider.RunInContext(context =>
            {
                var startDate = new DateTime(2012, 08, 1);
                var endDate = new DateTime(2012, 08, 31);
                var period = 14;

                var dailyRevenueQuery =
                        context.Bookings
                                .Select(booking =>
                                new
                                {
                                    StartDate = booking.StartTime.Date, // How to group by date (or TruncateTime) in EF-Core
                                    Revenue = booking.MemId == 0 ?
                                                           booking.Slots * booking.Facility.GuestCost
                                                           : booking.Slots * booking.Facility.MemberCost
                                })
                                .GroupBy(b => b.StartDate)
                                .Select(group =>
                                new
                                {
                                    Date = group.Key,
                                    TotalRevenue = group.Sum(b => b.Revenue)
                                });

                var movingAvgs =
                        dailyRevenueQuery
                                .Select(dr1 =>
                                new
                                {
                                    dr1.Date,
                                    MovingAvg = dailyRevenueQuery
                                        .Where(dr2 => dr2.Date <= dr1.Date && dr2.Date >= dr1.Date.AddDays(-period))
                                        .Average(dr2 => dr2.TotalRevenue)
                                })
                                .Where(result => result.Date >= startDate && result.Date <= endDate)
                                .OrderBy(result => result.Date)
                                .ToList();
                /*
                    SELECT CONVERT(date, [b0].[StartTime]) AS [Date], (
                            SELECT AVG([t].[c0])
                            FROM (
                                SELECT CONVERT(date, [b].[StartTime]) AS [c], SUM(CASE
                                    WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                                    ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                                END) AS [c0]
                                FROM [Bookings] AS [b]
                                INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                                GROUP BY CONVERT(date, [b].[StartTime])
                                HAVING (CONVERT(date, [b].[StartTime]) <= CONVERT(date, [b0].[StartTime])) AND (CONVERT(date, [b].[StartTime]) >= DATEADD(day, CAST(@__p_0 AS int), CONVERT(date, [b0].[StartTime])))
                            ) AS [t]) AS [MovingAvg]
                        FROM [Bookings] AS [b0]
                        INNER JOIN [Facilities] AS [f0] ON [b0].[FacId] = [f0].[FacId]
                        GROUP BY CONVERT(date, [b0].[StartTime])
                        HAVING (CONVERT(date, [b0].[StartTime]) >= @__startDate_1) AND (CONVERT(date, [b0].[StartTime]) <= @__endDate_2)
                        ORDER BY CONVERT(date, [b0].[StartTime])
                */
                var expectedResult = new[]
                {
                    new { Date = new DateTime(2012,08,01), MovingAvg =  1126.833333333333M } ,
                    new { Date = new DateTime(2012,08,02), MovingAvg =  1153.000000000000M } ,
                    new { Date = new DateTime(2012,08,03), MovingAvg =  1162.900000000000M } ,
                    new { Date = new DateTime(2012,08,04), MovingAvg =  1177.366666666666M } ,
                    new { Date = new DateTime(2012,08,05), MovingAvg =  1160.933333333333M } ,
                    new { Date = new DateTime(2012,08,06), MovingAvg =  1185.400000000000M } ,
                    new { Date = new DateTime(2012,08,07), MovingAvg =  1182.866666666666M } ,
                    new { Date = new DateTime(2012,08,08), MovingAvg =  1172.600000000000M } ,
                    new { Date = new DateTime(2012,08,09), MovingAvg =  1152.466666666666M } ,
                    new { Date = new DateTime(2012,08,10), MovingAvg =  1175.033333333333M } ,
                    new { Date = new DateTime(2012,08,11), MovingAvg =  1176.633333333333M } ,
                    new { Date = new DateTime(2012,08,12), MovingAvg =  1195.666666666666M } ,
                    new { Date = new DateTime(2012,08,13), MovingAvg =  1218.000000000000M } ,
                    new { Date = new DateTime(2012,08,14), MovingAvg =  1247.466666666666M } ,
                    new { Date = new DateTime(2012,08,15), MovingAvg =  1274.100000000000M } ,
                    new { Date = new DateTime(2012,08,16), MovingAvg =  1281.233333333333M } ,
                    new { Date = new DateTime(2012,08,17), MovingAvg =  1324.466666666666M } ,
                    new { Date = new DateTime(2012,08,18), MovingAvg =  1373.733333333333M } ,
                    new { Date = new DateTime(2012,08,19), MovingAvg =  1406.066666666666M } ,
                    new { Date = new DateTime(2012,08,20), MovingAvg =  1427.066666666666M } ,
                    new { Date = new DateTime(2012,08,21), MovingAvg =  1450.333333333333M } ,
                    new { Date = new DateTime(2012,08,22), MovingAvg =  1539.700000000000M } ,
                    new { Date = new DateTime(2012,08,23), MovingAvg =  1567.300000000000M } ,
                    new { Date = new DateTime(2012,08,24), MovingAvg =  1592.333333333333M } ,
                    new { Date = new DateTime(2012,08,25), MovingAvg =  1615.033333333333M } ,
                    new { Date = new DateTime(2012,08,26), MovingAvg =  1631.200000000000M } ,
                    new { Date = new DateTime(2012,08,27), MovingAvg =  1659.433333333333M } ,
                    new { Date = new DateTime(2012,08,28), MovingAvg =  1687.000000000000M } ,
                    new { Date = new DateTime(2012,08,29), MovingAvg =  1684.633333333333M } ,
                    new { Date = new DateTime(2012,08,30), MovingAvg =  1657.933333333333M } ,
                    new { Date = new DateTime(2012,08,31), MovingAvg =  1703.400000000000M }
                };

                movingAvgs.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}