using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)] // Workers: The number of threads to run the tests. Set it to 0 to use the number of core of your computer.
namespace EFCorePgExercises
{
    [FullyQualifiedTestClass]
    public class Bootstrapper
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            SeedData.Start();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
        }
    }
}