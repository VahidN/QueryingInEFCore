using System.Collections.Generic;
using EFCorePgExercises.Entities;

namespace EFCorePgExercises.Exercises.RecursiveQueries
{
    public static class RecursiveUtils
    {
        public static void FindParents(Member member, List<dynamic> actualResult)
        {
            if (member == null || member.Recommender == null)
            {
                return;
            }

            var item = member.Recommender;
            actualResult.Add(new { Recommender = item.MemId, item.FirstName, item.Surname });

            if (item.Recommender != null)
            {
                FindParents(item, actualResult);
            }
        }

        public static void FindChildren(Member member, List<dynamic> actualResult)
        {
            if (member == null)
            {
                return;
            }

            foreach (var item in member.Children)
            {
                actualResult.Add(new { item.MemId, item.FirstName, item.Surname });
                if (item.Children != null)
                {
                    FindChildren(item, actualResult);
                }
            }
        }
    }
}