using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class UpdateMultipleRowsAndColumnsAtTheSameTime
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/updatemultiple.html
            // We want to increase the price of the tennis courts for both members and guests.
            // Update the costs to be 6 for members, and 30 for guests.
            //update cd.facilities
            //    set
            //        membercost = 6,
            //        guestcost = 30
            //    where facid in (0,1);

            EFServiceProvider.RunInContext(context =>
            {
                int[] facIds = { 0, 1 };
                var tennisCourts = context.Facilities.Where(x => facIds.Contains(x.FacId)).ToList();
                foreach (var tennisCourt in tennisCourts)
                {
                    tennisCourt.MemberCost = 6;
                    tennisCourt.GuestCost = 30;
                }

                context.SaveChanges();
            });
        }
    }
}