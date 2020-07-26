using System.Linq;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using FluentAssertions;

namespace EFCorePgExercises.Exercises.Aggregation
{
    [FullyQualifiedTestClass]
    public class CalculateThePaybackTimeForEachFacility
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/aggregates/payback.html
            // Based on the 3 complete months of data so far, calculate the amount of time each
            // facility will take to repay its cost of ownership. Remember to take into account
            // ongoing monthly maintenance. Output facility name and payback time in months,
            //  order by facility name. Don't worry about differences in month lengths,
            // we're only looking for a rough value here!
            //select 	facs.name as name,
            //	facs.initialoutlay/((sum(case
            //			when memid = 0 then slots * facs.guestcost
            //			else slots * membercost
            //		end)/3) - facs.monthlymaintenance) as months
            //	from cd.bookings bks
            //	inner join cd.facilities facs
            //		on bks.facid = facs.facid
            //	group by facs.facid
            //order by name;
            //
            //
            //select 	name,
            //	initialoutlay / (monthlyrevenue - monthlymaintenance) as repaytime
            //	from
            //		(select facs.name as name,
            //			facs.initialoutlay as initialoutlay,
            //			facs.monthlymaintenance as monthlymaintenance,
            //			sum(case
            //				when memid = 0 then slots * facs.guestcost
            //				else slots * membercost
            //			end)/3 as monthlyrevenue
            //		from cd.bookings bks
            //		inner join cd.facilities facs
            //			on bks.facid = facs.facid
            //		group by facs.name, facs.initialoutlay, facs.monthlymaintenance
            //	) as subq
            //order by name;
            //
            //
            //with monthdata as (
            //	select 	mincompletemonth,
            //		maxcompletemonth,
            //		(extract(year from maxcompletemonth)*12) +
            //			extract(month from maxcompletemonth) -
            //			(extract(year from mincompletemonth)*12) -
            //			extract(month from mincompletemonth) as nummonths
            //	from (
            //		select 	date_trunc('month',
            //				(select max(starttime) from cd.bookings)) as maxcompletemonth,
            //			date_trunc('month',
            //				(select min(starttime) from cd.bookings)) as mincompletemonth
            //	) as subq
            //)
            //select 	name,
            //	initialoutlay / (monthlyrevenue - monthlymaintenance) as repaytime
            //
            //	from
            //		(select facs.name as name,
            //			facs.initialoutlay as initialoutlay,
            //			facs.monthlymaintenance as monthlymaintenance,
            //			sum(case
            //				when memid = 0 then slots * facs.guestcost
            //				else slots * membercost
            //			end)/(select nummonths from monthdata) as monthlyrevenue
            //
            //			from cd.bookings bks
            //			inner join cd.facilities facs
            //				on bks.facid = facs.facid
            //			where bks.starttime < (select maxcompletemonth from monthdata)
            //			group by facs.facid
            //		) as subq
            //order by name;

            EFServiceProvider.RunInContext(context =>
            {
                var facilities =
                            context.Bookings.Select(booking =>
                                new
                                {
                                    booking.Facility.Name,
                                    booking.Facility.InitialOutlay,
                                    booking.Facility.MonthlyMaintenance,
                                    Revenue = booking.MemId == 0 ?
                                            booking.Slots * booking.Facility.GuestCost
                                            : booking.Slots * booking.Facility.MemberCost
                                })
                                .GroupBy(b => new
                                {
                                    b.Name,
                                    b.InitialOutlay,
                                    b.MonthlyMaintenance
                                })
                                .Select(group => new
                                {
                                    group.Key.Name,
                                    RepayTime =
                                        group.Key.InitialOutlay /
                                                ((group.Sum(b => b.Revenue) / 3) - group.Key.MonthlyMaintenance)
                                })
                                .OrderBy(result => result.Name)
                                .ToList();

                /*
                SELECT [f].[Name], [f].[InitialOutlay] / ((SUM(CASE
                          WHEN [b].[MemId] = 0 THEN CAST([b].[Slots] AS decimal(18,6)) * [f].[GuestCost]
                          ELSE CAST([b].[Slots] AS decimal(18,6)) * [f].[MemberCost]
                        END) / 3.0) - [f].[MonthlyMaintenance]) AS [RepayTime]
                        FROM [Bookings] AS [b]
                        INNER JOIN [Facilities] AS [f] ON [b].[FacId] = [f].[FacId]
                        GROUP BY [f].[Name], [f].[InitialOutlay], [f].[MonthlyMaintenance]
                        ORDER BY [f].[Name]
                */

                var expectedResult = new[]
                {
                    new { Name = "Badminton Court", RepayTime = 6.831767719897523M }, // decimal(18, 6)
                    new { Name = "Massage Room 1", RepayTime =  0.188857412653446M },
                    new { Name = "Massage Room 2", RepayTime =  1.762114537444933M },
                    new { Name = "Pool Table",     RepayTime =  5.333333333333333M },
                    new { Name = "Snooker Table", RepayTime =   6.923076923076923M },
                    new { Name = "Squash Court", RepayTime =    1.133958270335652M },
                    new { Name = "Table Tennis", RepayTime =    6.400000000000000M },
                    new { Name = "Tennis Court 1", RepayTime =  2.262443438914027M },
                    new { Name = "Tennis Court 2", RepayTime =  1.750547045951859M }
                };

                facilities.Should().BeEquivalentTo(expectedResult);
            });
        }
    }
}