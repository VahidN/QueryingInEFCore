using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class InsertCalculatedDataIntoATable
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/insert3.html
            // Let's try adding the spa to the facilities table again. This time, though,
            // we want to automatically generate the value for the next facid, rather than
            // specifying it as a constant. Use the following values for everything else:
            // Name: 'Spa', membercost: 20, guestcost: 30, initialoutlay: 100000, monthlymaintenance: 800.
            //insert into cd.facilities
            //    (facid, name, membercost, guestcost, initialoutlay, monthlymaintenance)
            //    select (select max(facid) from cd.facilities)+1, 'Spa', 20, 30, 100000, 800;

            EFServiceProvider.RunInContext(context =>
            {
                // This is not necessary, because it's defined as `UseIdentityColumn(seed: 0, increment: 1)` here.
            });
        }
    }
}