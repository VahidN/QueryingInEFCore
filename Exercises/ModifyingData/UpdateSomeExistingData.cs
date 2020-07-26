using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class UpdateSomeExistingData
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/update.html
            // We made a mistake when entering the data for the second tennis court.
            // The initial outlay was 10000 rather than 8000: you need to alter the data to fix the error.
            //update cd.facilities
            //    set initialoutlay = 10000
            //    where facid = 1;
            //
            // update cd.facilities
            // set initialoutlay = 10000;

            EFServiceProvider.RunInContext(context =>
            {
                var facility1 = context.Facilities.Find(1);
                facility1.InitialOutlay = 10000;
                context.SaveChanges();
            });
        }
    }
}