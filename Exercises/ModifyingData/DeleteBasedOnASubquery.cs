using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class DeleteBasedOnASubquery
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/deletewh2.html
            // In our previous exercises, we deleted a specific member who had never made a booking.
            // How can we make that more general, to delete all members who have never made a booking?
            // delete from cd.members where memid not in (select memid from cd.bookings);
            // delete from cd.members mems where not exists (select 1 from cd.bookings where memid = mems.memid);

            EFServiceProvider.RunInContext(context =>
            {
                var mems = context.Members.Where(x =>
                                !context.Bookings.Select(x => x.MemId).Contains(x.MemId)).ToList();
                context.Members.RemoveRange(mems);

                context.SaveChanges();
            });
        }
    }
}