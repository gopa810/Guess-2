using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuessApp
{
    public class CalendarDay
    {
        public DateTime date;
        public List<int> numbers = new List<int>();

        public bool Contains(int nNumber)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] == nNumber)
                    return true;
            }
            return false;
        }

        public int CountPresent(List<int> list)
        {
            int count = 0;
            foreach (int a in list)
            {
                if (Contains(a))
                    count++;
            }
            return count;
        }

        public int VectorDistanceSum(CalendarDay day)
        {
            int c = 0;
            if (day.numbers.Count != numbers.Count)
                return -1;
            for (int i = 0; i < day.numbers.Count; i++)
            {
                c += Math.Abs(numbers[i] - day.numbers[i]);
            }
            return c;
        }
        public int VectorDistanceMax(CalendarDay day)
        {
            int c = 0;
            int tmp;
            if (day.numbers.Count != numbers.Count)
                return -1;
            for (int i = 0; i < day.numbers.Count; i++)
            {
                tmp = Math.Abs(numbers[i] - day.numbers[i]);
                if (c < tmp)
                    c = tmp;
            }
            return c;
        }
        public int VectorDistanceMin(CalendarDay day)
        {
            int c = 1000;
            int tmp;
            if (day.numbers.Count != numbers.Count)
                return -1;
            for (int i = 0; i < day.numbers.Count; i++)
            {
                tmp = Math.Abs(numbers[i] - day.numbers[i]);
                if (c > tmp)
                    c = tmp;
            }
            return c;
        }
    }

    public class ArrayIntegers
    {
        public const int MAX = 100;
        public int[] nums = null;
        public int[] colors = null;
        public ArrayIntegers()
        {
            nums = new int[MAX];
            colors = new int[MAX];
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < MAX; i++)
            {
                nums[i] = 0;
                colors[i] = -1;
            }
        }

        public int CountDiff(int a)
        {
            int count = 0;
            for (int i = 0; i < MAX; i++)
            {
                if (nums[i] != a)
                    count++;
            }
            return count;
        }


        public void IncrementDayNumbers(CalendarDay cd)
        {
            for (int i = 0; i < cd.numbers.Count; i++)
            {
                if (cd.numbers[i] > 0 && cd.numbers[i] < MAX)
                {
                    nums[cd.numbers[i]]++;
                }
            }
        }
    }
}
