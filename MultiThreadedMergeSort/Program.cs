using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadedMergeSort
{
    public class Program
    {
        public void Sort(int[] input, int start, int end)
        {
            if (end - start <= 1) return; // Base case: if there's only one element or no elements, return

            int mid = (start + end) / 2;
            var thread1 = new Thread(() => Sort(input, start, mid)); // Sort the left half
            var thread2 = new Thread(() => Sort(input, mid, end)); // Sort the right half
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            // Merge the sorted halves in-place
            int[] temp = new int[end - start];
            int i = start, j = mid, k = 0;

            while (i < mid && j < end)
            {
                if (input[i] < input[j])
                {
                    temp[k++] = input[i++];
                }
                else
                {
                    temp[k++] = input[j++];
                }
            }

            while (i < mid)
            {
                temp[k++] = input[i++];
            }

            while (j < end)
            {
                temp[k++] = input[j++];
            }

            // Copy the merged elements back to the original array
            for (int idx = 0; idx < temp.Length; idx++)
            {
                input[start + idx] = temp[idx];
            }
        }

        public static void Main(string[] args)
        {
            int[] input = { 5, 3, 2, 1, 6 };
            var obj = new Program();
            obj.Sort(input, 0, input.Length);
        }
    }
}
