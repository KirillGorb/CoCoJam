using CodeScripts.Algorithms.Sort.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeScripts.Algorithms.Sort
{
    public class TimSort : ISortAlgorithms
    {
        public List<int> SortList(List<int> ListToSort)
        {
            int n = ListToSort.Count;

            if (n < 2)
                return ListToSort;

            int minMarge = GetOptimalMinMerge(ListToSort.Count);

            for (int i = 0; i < n; i += minMarge)
            {
                int runEnd = Math.Min(i + minMarge, n);
                InsertionSort(ref ListToSort, i, runEnd);
            }

            for (int size = minMarge; size < n; size = 2 * size)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = Math.Min(left + size, n);
                    int right = Math.Min(left + 2 * size, n);
                    if (mid < right)
                    {
                        Merge(ref ListToSort, left, mid, right);
                    }
                }
            }

            return ListToSort;
        }

        private void InsertionSort(ref List<int> list, int left, int right)
        {
            for (int i = left + 1; i < right; i++)
            {
                int key = list[i];
                int j = i - 1;

                while (j >= left && list[j] > key)
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = key;
            }
        }
        private void Merge(ref List<int> list, int left, int mid, int right)
        {
            int[] arr = list.ToArray();

            int len1 = mid - left;
            int len2 = right - mid;

            int[] leftArray = new int[len1];
            int[] rightArray = new int[len2];

            Array.Copy(arr, left, leftArray, 0, len1);
            Array.Copy(arr, mid, rightArray, 0, len2);

            int i = 0;
            int j = 0;
            int k = left;

            while (i < len1 && j < len2)
            {
                if (leftArray[i] <= rightArray[j])
                {
                    arr[k] = leftArray[i];
                    i++;
                }
                else
                {
                    arr[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            while (i < len1)
            {
                arr[k] = leftArray[i];
                i++;
                k++;
            }

            while (j < len2)
            {
                arr[k] = rightArray[j];
                j++;
                k++;
            }

            list = arr.ToList();
        }

        private int GetOptimalMinMerge(int Count)
        {
            if (Count > 100)
                return 50;

            int nearestPowerOfTwo = 1;          

            while (Count > nearestPowerOfTwo)
                nearestPowerOfTwo *= 2;

            return nearestPowerOfTwo / 2;
        }
    }
}
