using System;
using System.Collections.Generic;

namespace Final_Task.Utils
{
    public class SortUtils
    {
        public static bool IsTestsSortedByDescendingDate(List<DateTime> dateList)
        {
            bool isSorted = true;

            for (int i = 0; i < dateList.Count - 1; i++)
            {
                if (dateList[i].CompareTo(dateList[i + 1]) < 0)
                {
                    isSorted = false;
                    break;
                }
            }

            return isSorted;
        }
    }
}
