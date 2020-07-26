using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFCorePgExercises.Utils
{
    public class FullyQualifiedTestClassAttribute : TestClassAttribute
    {
        public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute wrappedTestMethodAttribute)
        {
            var attribute = base.GetTestMethodAttribute(wrappedTestMethodAttribute);

            return attribute as FullyQualifiedTestMethodAttribute ?? new FullyQualifiedTestMethodAttribute(attribute);
        }
    }

    public class FullyQualifiedTestMethodAttribute : TestMethodAttribute
    {
        private readonly TestMethodAttribute wrappedTestMethodAttribute;

        public FullyQualifiedTestMethodAttribute() { }

        public FullyQualifiedTestMethodAttribute(TestMethodAttribute wrappedTestMethodAttribute) =>
            this.wrappedTestMethodAttribute = wrappedTestMethodAttribute;

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            TestResult[] results = wrappedTestMethodAttribute is null
                ? base.Execute(testMethod)
                : wrappedTestMethodAttribute.Execute(testMethod);

            if (results.Any())
            {
                results[0].DisplayName = $"{testMethod.TestClassName}.{testMethod.TestMethodName}"
                    .Replace("_eq_", " == ")
                    .Replace("_neq_", " != ")
                    .Replace("_gt_", " > ")
                    .Replace("_gte_", " >= ")
                    .Replace("_lt_", " < ")
                    .Replace("_lte_", " <= ")
                    .Replace("Bug_", "🐞 ")
                    .Replace("_", " ");
            }

            return results;
        }
    }
}