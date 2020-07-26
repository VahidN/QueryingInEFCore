using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class UpdateARowBasedOnTheContentsOfAnotherRow
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/updatecalculated.html
            // We want to alter the price of the second tennis court so that it costs 10% more than the first one.
            // Try to do this without using constant values for the prices, so that we can reuse the statement if we want to.
            //update cd.facilities facs
            //    set
            //        membercost = (select membercost * 1.1 from cd.facilities where facid = 0),
            //        guestcost = (select guestcost * 1.1 from cd.facilities where facid = 0)
            //    where facs.facid = 1;
            //
            //update cd.facilities facs
            //    set
            //        membercost = facs2.membercost * 1.1,
            //        guestcost = facs2.guestcost * 1.1
            //    from (select * from cd.facilities where facid = 0) facs2
            //    where facs.facid = 1;

            EFServiceProvider.RunInContext(context =>
            {
                var fac0 = context.Facilities.Where(x => x.FacId == 0).First();
                var fac1 = context.Facilities.Where(x => x.FacId == 1).First();
                fac1.MemberCost = fac0.MemberCost * 1.1M;
                fac1.GuestCost = fac0.GuestCost * 1.1M;

                context.SaveChanges();
            });
        }
    }
}