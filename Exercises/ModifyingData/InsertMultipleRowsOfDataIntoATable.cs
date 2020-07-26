using EFCorePgExercises.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class InsertMultipleRowsOfDataIntoATable
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/insert2.html
            // In the previous exercise, you learned how to add a facility.
            // Now you're going to add multiple facilities in one command. Use the following values:
            //facid: 9, Name: 'Spa', membercost: 20, guestcost: 30, initialoutlay: 100000, monthlymaintenance: 800.
            //facid: 10, Name: 'Squash Court 2', membercost: 3.5, guestcost: 17.5, initialoutlay: 5000, monthlymaintenance: 80.
            //insert into cd.facilities
            //    (facid, name, membercost, guestcost, initialoutlay, monthlymaintenance)
            //    values
            //        (9, 'Spa', 20, 30, 100000, 800),
            //        (10, 'Squash Court 2', 3.5, 17.5, 5000, 80);
            //
            //insert into cd.facilities
            //    (facid, name, membercost, guestcost, initialoutlay, monthlymaintenance)
            //    SELECT 9, 'Spa', 20, 30, 100000, 800
            //    UNION ALL
            //        SELECT 10, 'Squash Court 2', 3.5, 17.5, 5000, 80;

            EFServiceProvider.RunInContext(context =>
            {
                context.Facilities.Add(new Facility
                {
                    Name = "Squash Court 2",
                    MemberCost = 3.5M,
                    GuestCost = 17.5M,
                    InitialOutlay = 5000,
                    MonthlyMaintenance = 80
                });
                context.SaveChanges();
            });
        }
    }
}