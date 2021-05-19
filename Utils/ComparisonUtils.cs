using System.Collections.Generic;

namespace Final_Task.Utils
{
    class ComparisonUtils
    {
        public static bool IsListContainsAllItemsFromAnother<T>(List<T> list, List<T> anotherList)
        {
            bool isContains = false;

            for (int i = 0; i < list.Count; i++)
            {
                if (anotherList.Contains(list[i]))
                {
                    isContains = true;
                }
                else
                {
                    isContains = false;
                    break;
                }
            }

            return isContains;
        }
    }
}
