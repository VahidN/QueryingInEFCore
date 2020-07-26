using EFCorePgExercises.DataLayer;
using EFCorePgExercises.Utils;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    [FullyQualifiedTestClass]
    public class WorkoutTheNumberOfDaysRemainingInTheMonth
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            // https://pgexercises.com/questions/date/daysremaining.html
            // For any given timestamp, work out the number of days remaining in the month.
            // The current day should count as a whole day, regardless of the time.
            // Use '2012-02-11 01:00:00' as an example timestamp for the purposes
            // of making the answer. Format the output as a single interval value.
            //
            //select (date_trunc('month',ts.testts) + interval '1 month')
            //		- date_trunc('day', ts.testts) as remaining
            //	from (select timestamp '2012-02-11 01:00:00' as testts) ts

            EFServiceProvider.RunInContext(context =>
            {

            });
        }
    }
}