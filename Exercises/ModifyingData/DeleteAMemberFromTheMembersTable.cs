using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.ModifyingData
{
    [FullyQualifiedTestClass]
    public class DeleteAMemberFromTheMembersTable
    {
        [FullyQualifiedTestMethod, Ignore]
        public void Test()
        {
            // https://pgexercises.com/questions/updates/deletewh.html
            // We want to remove member 37, who has never made a booking, from our database. How can we achieve that?
            // delete from cd.members where memid = 37;

            EFServiceProvider.RunInContext(context =>
            {
                var mem37 = context.Members.Where(x => x.MemId == 37).First();
                context.Members.Remove(mem37);

                context.SaveChanges();
            });
        }
    }
}