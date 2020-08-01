using System.Linq;
using EFCorePgExercises.DataLayer;
using FluentAssertions;
using EFCorePgExercises.Utils;
using System;
using System.Globalization;
using System.Linq.Expressions;
using EFCorePgExercises.Entities;

namespace EFCorePgExercises.Exercises.WorkingWithTimestamps
{
    public class UIModel
    {
        public int PersianYear { set; get; }

        public int[] SelectedPersianMonths { set; get; }
    }

    [FullyQualifiedTestClass]
    public class FindRegisteredMembersInGivenMonths
    {
        [FullyQualifiedTestMethod]
        public void Test()
        {
            EFServiceProvider.RunInContext(context =>
            {
                var model = new UIModel
                {
                    PersianYear = 1391,
                    SelectedPersianMonths = new[] { 4, 5 }
                };

                var itemsQuery = context.Members.AsQueryable();

                // Linq chaining where clauses as an `Or` instead of `And`
                var predicate = PredicateBuilder.False<Member>();

                foreach (var month in model.SelectedPersianMonths)
                {
                    var start = new DateTime(model.PersianYear, month, 1, new PersianCalendar());
                    var end = new DateTime(model.PersianYear, month, month <= 6 ? 31 : 30, new PersianCalendar());

                    // We can chain `IQueryable`s.
                    // itemsQuery = itemsQuery.Where(x => x.JoinDate.Date >= start && x.JoinDate.Date <= end);
                    // But it will be translated as an `AND`, not `OR`

                    predicate = predicate.Or(x => x.JoinDate.Date >= start && x.JoinDate.Date <= end);
                }

                itemsQuery = itemsQuery.Where(predicate);

                var items = itemsQuery.Select(x => new { x.FirstName, x.Surname }).ToList();

                /*
                    SELECT [m].[FirstName],
                        [m].[Surname]
                    FROM   [Members] AS [m]
                    WHERE  ((CONVERT (DATE, [m].[JoinDate]) >= '2012-06-21T00:00:00')
                            AND (CONVERT (DATE, [m].[JoinDate]) <= '2012-07-21T00:00:00'))
                        OR ((CONVERT (DATE, [m].[JoinDate]) >= '2012-07-22T00:00:00')
                            AND (CONVERT (DATE, [m].[JoinDate]) <= '2012-08-21T00:00:00'));
                */

                var expectedResult = new[]
                {
                    new { FirstName = "GUEST", Surname = "GUEST" },
                    new { FirstName = "Darren", Surname = "Smith" },
                    new { FirstName = "Tracy", Surname = "Smith" },
                    new { FirstName = "Tim", Surname = "Rownam" },
                    new { FirstName = "Janice", Surname = "Joplette" },
                    new { FirstName = "Gerald", Surname = "Butters" },
                    new { FirstName = "Burton", Surname = "Tracy" },
                    new { FirstName = "Nancy", Surname = "Dare" },
                    new { FirstName = "Tim", Surname = "Boothe" },
                    new { FirstName = "Ponder", Surname = "Stibbons" },
                    new { FirstName = "Charles", Surname = "Owen" },
                    new { FirstName = "David", Surname = "Jones" },
                    new { FirstName = "Anne", Surname = "Baker" },
                    new { FirstName = "Jemima", Surname = "Farrell" },
                    new { FirstName = "Jack", Surname = "Smith" },
                    new { FirstName = "Florence", Surname = "Bader" },
                    new { FirstName = "Timothy", Surname = "Baker" },
                    new { FirstName = "David", Surname = "Pinker" },
                    new { FirstName = "Matthew", Surname = "Genting" }
                };
                items.Should().BeEquivalentTo(expectedResult);
            });
        }
    }

    // From: http://www.albahari.com/nutshell/predicatebuilder.aspx
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}