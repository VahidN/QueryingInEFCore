using EFCorePgExercises.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class InsertSomeDataIntoATable
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/insert.html
            // The club is adding a new facility - a spa. We need to add it into the facilities table.
            // Use the following values:
            // facid: 9, Name: 'Spa', membercost: 20, guestcost: 30, initialoutlay: 100000, monthlymaintenance: 800.
            // insert into cd.facilities
            //    (facid, name, membercost, guestcost, initialoutlay, monthlymaintenance)
            //    values (9, 'Spa', 20, 30, 100000, 800);
            //
            // insert into cd.facilities values (9, 'Spa', 20, 30, 100000, 800);

            EFServiceProvider.RunInContext(context =>
            {
                context.Facilities.Add(new Facility
                {
                    Name = "Spa",
                    MemberCost = 20,
                    GuestCost = 30,
                    InitialOutlay = 100000,
                    MonthlyMaintenance = 800
                });
                context.SaveChanges();
            });
        }
    }
}