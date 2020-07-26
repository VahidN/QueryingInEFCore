using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class DeleteAllBookings
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/delete.html
            // As part of a clearout of our database, we want to delete all bookings from
            // the cd.bookings table. How can we accomplish this?
            // delete from cd.bookings;
            // truncate cd.bookings;

            EFServiceProvider.RunInContext(context =>
            {
                context.Bookings.RemoveRange(context.Bookings.ToList());
                context.SaveChanges();
            });
        }
    }
}