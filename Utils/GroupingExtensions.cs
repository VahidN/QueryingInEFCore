using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EFCorePgExercises.Utils
{
    /// <summary>
    /// Represents an instance of an IGrouping<>.  Used by GroupByMany(), GroupByWithRollup(), and GrandTotal().
    /// </summary>
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public TKey Key { get; set; }
        public IEnumerable<TElement> Items { get; set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }

    public static class GroupingExtensions
    {
        /// <summary>
        /// Groups by two columns.
        /// </summary>
        /// <typeparam name="TElement">Type of elements to group.</typeparam>
        /// <typeparam name="TKey1">Type of the first expression to group by.</typeparam>
        /// <typeparam name="TKey2">Type of the second expression to group by.</typeparam>
        /// <param name="orderedElements">Elements to group.</param>
        /// <param name="groupByKey1Expression">The first expression to group by.</param>
        /// <param name="groupByKey2Expression">The second expression to group by.</param>
        /// <param name="newElementExpression">An expression that returns a new TElement.</param>
        public static IEnumerable<Grouping<TKey1, TElement>> GroupByMany<TElement, TKey1, TKey2>(
            this IEnumerable<TElement> orderedElements,
            Func<TElement, TKey1> groupByKey1Expression,
            Func<TElement, TKey2> groupByKey2Expression,
            Func<IGrouping<TKey1, TElement>, IGrouping<TKey2, TElement>, TElement> newElementExpression
            )
        {
            // Group the items by Key1 and Key2
            return from element in orderedElements
                   group element by groupByKey1Expression(element) into groupByKey1
                   select new Grouping<TKey1, TElement>
                   {
                       Key = groupByKey1.Key,
                       Items = from key1Item in groupByKey1
                               group key1Item by groupByKey2Expression(key1Item) into groupByKey2
                               select newElementExpression(groupByKey1, groupByKey2)
                   };
        }

        /// <summary>
        /// Returns a List of TElement containing all elements of orderedElements as well as subTotals and a grand total.
        /// </summary>
        /// <typeparam name="TElement">Type of elements to group.</typeparam>
        /// <typeparam name="TKey1">Type of the first expression to group by.</typeparam>
        /// <typeparam name="TKey2">Type of the second expression to group by.</typeparam>
        /// <param name="orderedElements">Elements to group.</param>
        /// <param name="groupByKey1Expression">The first expression to group by.</param>
        /// <param name="groupByKey2Expression">The second expression to group by.</param>
        /// <param name="newElementExpression">An expression that returns a new TElement.</param>
        /// <param name="subTotalExpression">An expression that returns a new TElement that represents a subTotal.</param>
        /// <param name="totalExpression">An expression that returns a new TElement that represents a grand total.</param>
        public static List<TElement> GroupByWithRollup<TElement, TKey1, TKey2>(
            this IEnumerable<TElement> orderedElements,
            Func<TElement, TKey1> groupByKey1Expression,
            Func<TElement, TKey2> groupByKey2Expression,
            Func<IGrouping<TKey1, TElement>, IGrouping<TKey2, TElement>, TElement> newElementExpression,
            Func<IGrouping<TKey1, TElement>, TElement> subTotalExpression,
            Func<IEnumerable<Grouping<TKey1, TElement>>, TElement> totalExpression
            )
        {
            // Group the items by Key1 and Key2
            IEnumerable<Grouping<TKey1, TElement>> groupedItems = orderedElements.GroupByMany(groupByKey1Expression, groupByKey2Expression, newElementExpression);

            // Create a new list the items, subtotals, and the grand total.
            List<TElement> results = new List<TElement>();
            foreach (Grouping<TKey1, TElement> item in groupedItems)
            {
                // Add items under current group
                results.AddRange(item);
                // Add subTotal for current group
                results.Add(subTotalExpression(item));
            }
            // Add grand total
            results.Add(totalExpression(groupedItems));

            return results;
        }

        /// <summary>
        /// Returns the subTotal sum of sumExpression.
        /// </summary>
        /// <param name="sumExpression">An expression that returns the value to sum.</param>
        public static int SubTotal<TKey, TElement>(this IGrouping<TKey, TElement> query, Func<TElement, int> sumExpression)
        {
            return query.Sum(group => sumExpression(group));
        }

        /// <summary>
        /// Returns the subTotal sum of sumExpression.
        /// </summary>
        /// <param name="sumExpression">An expression that returns the value to sum.</param>
        public static decimal SubTotal<TKey, TElement>(this IGrouping<TKey, TElement> query, Func<TElement, decimal> sumExpression)
        {
            return query.Sum(group => sumExpression(group));
        }

        /// <summary>
        /// Returns the grand total sum of sumExpression.
        /// </summary>
        /// <param name="sumExpression">An expression that returns the value to sum.</param>
        public static int GrandTotal<TKey, TElement>(this IEnumerable<Grouping<TKey, TElement>> query, Func<TElement, int> sumExpression)
        {
            return query.Sum(group => group.Sum(innerGroup => sumExpression(innerGroup)));
        }

        /// <summary>
        /// Returns the grand total sum of sumExpression.
        /// </summary>
        /// <param name="sumExpression">An expression that returns the value to sum.</param>
        public static decimal GrandTotal<TKey, TElement>(this IEnumerable<Grouping<TKey, TElement>> query, Func<TElement, decimal> sumExpression)
        {
            return query.Sum(group => group.Sum(innerGroup => sumExpression(innerGroup)));
        }
    }
}