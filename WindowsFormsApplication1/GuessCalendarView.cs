using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuessApp
{
    public partial class GuessCalendarView : UserControl
    {
        public string command = string.Empty;
        public NumberStack numStack = new NumberStack();
        public StringFormat format = new StringFormat();
        public Size cellSize = new Size(20, 20);

        public int[] k10min = new int[] {1,2,3,4,5,6,11,13,17,23};
        public int[] k10max = new int[] {8,22,24,28,34,39,40,41,43,45};
        public int k10accepted = 0;
        public int k10rejected = 0;

        public GuessCalendarView()
        {
            InitializeComponent();
 
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
        }

        public static string[] Commands
        {
            get
            {
                return new string[] {
                    "plain",
                    "frequency",
                    "presence",
                    "distance.last.used",
                    "sum.combine.previous",
                    "insect.combine.previous",
                    "vector.distance.sum",
                    "vector.distance.max",
                    "vector.distance.min",
                    "digits.statistic",
                    "frequency.hits",
                    "frequency.hits.p3-5",
                    "frequency.hits.p4-6",
                    "frequency.hits.p5-7",
                    "frequency.hits.p6-8",
                    "proposal.a5-7",
                    "half.analyze",
                    "min.nums", "max.nums",
                    "number.exist.a"
                };
            }
        }

        protected void DrawMatrixLines(Graphics g, int cols, int rows)
        {
            for (int i = 1; i <= rows; i++)
            {
                g.DrawLine(Pens.Black, 0, getPositionY(i), getPositionX(cols), getPositionY(i));
            }
            for (int i = 0; i <= cols; i++)
            {
                g.DrawLine(Pens.Black, getPositionX(i), 0, getPositionX(i), getPositionY(rows));
            }
        }

        protected void DrawNumStack(Graphics g)
        {
            for (int i = 0; i < numStack.Count; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (numStack.array[i].nums[j] != 0)
                    {
                        if (numStack.array[i].colors[j] >= 0)
                            g.FillRectangle(getBrush(numStack.array[i].colors[j]), getItemCell(i, j));
                        g.DrawString(numStack.array[i].nums[j].ToString(), SystemFonts.CaptionFont,
                            Brushes.Black, getItemCell(i, j), format);
                    }
                }
            }
        }

        protected void DrawPresentNumbers(Graphics g)
        {
            List<CalendarDay> days = CalendarData.days;
            
            // drawing numbers in normal way
            for (int i = 0; i < days.Count; i++)
            {
                CalendarDay cd = days[i] as CalendarDay;
                g.DrawString(cd.date.ToShortDateString(), SystemFonts.MenuFont, Brushes.Blue, getDateCell(i), format);

                for (int j = 0; j < cd.numbers.Count; j++)
                {
                    if (cd.numbers[j] > 0 && cd.numbers[j] < 81)
                    {
                        g.DrawString(cd.numbers[j].ToString(), SystemFonts.CaptionFont, Brushes.Black, getItemCell(i, cd.numbers[j] - 1), format);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            //this.Invalidate();
            Rectangle myBounds = Bounds;
            g.FillRectangle(SystemBrushes.Control, Bounds);

            List<CalendarDay> days = CalendarData.days;



            // drawing distances
            if (command == "distance.last.used")
            {
                for (int i = 0; i < days.Count; i++)
                {
                    CalendarDay cd = days[i] as CalendarDay;
                    for (int j = 0; j < 80; j++)
                    {
                        int mis = CalendarData.FindRow(j+1, i);
                        g.FillRectangle(getBrush(mis), getItemCell(i, j));
                    }
                }
                DrawPresentNumbers(g);
                DrawMatrixLines(g, 80, days.Count);
            }
            // drawing distances
            else if (command == "presence")
            {
                for (int i = 0; i < days.Count; i++)
                {
                    CalendarDay cd = days[i] as CalendarDay;
                    for (int j = 0; j < 80; j++)
                    {
                        int mis = cd.Contains(j+1) ? 6 : 0;
                        g.FillRectangle(getBrush(mis), getItemCell(i, j));
                    }
                }
                DrawPresentNumbers(g);
                DrawMatrixLines(g, 80, days.Count);
            }
            else if (command == "frequency")
            {
                for (int i = 0; i < days.Count; i++)
                {
                    CalendarDay cd = days[i] as CalendarDay;
                    for (int j = 0; j < 80; j++)
                    {
                        int mis = 0;
                        for (int k = i - 8; k < i + 8; k++)
                        {
                            if (k >= 0 && k < days.Count)
                            {
                                if ((days[k] as CalendarDay).Contains(j + 1))
                                    mis++;
                            }
                        }
                        g.FillRectangle(getBrush(mis), getItemCell(i, j));
                    }
                }
                DrawPresentNumbers(g);
                DrawMatrixLines(g, 80, days.Count);
            }
            else if (command == "sum.combine.previous" || command == "insect.combine.previous"
                || command == "vector.distance.sum" || command == "vector.distance.max"
                || command == "vector.distance.min" || command == "digits.statistic"
                || command == "frequency.hits")
            {
                cellSize = new Size(32, 20);
                DrawNumStack(g);
                DrawMatrixLines(g, 80, days.Count);
                cellSize = new Size(20, 20);
            }
            else if (command != null && command.StartsWith("frequency.hits.p"))
            {
                DrawNumStack(g);
                DrawMatrixLines(g, 80, days.Count);
            }
            else if (command == "proposal.a5-7" || command == "half.analyze" || command == "min.nums"
                || command == "max.nums" || command == "number.exist.a")
            {
                DrawNumStack(g);
                DrawMatrixLines(g, 80, days.Count);
            }
            else
            {
                DrawPresentNumbers(g);
                DrawMatrixLines(g, 80, days.Count);
            }

        }

        public string[] colors = new string[]
        {
            "#ffffff",
            "#eeffff",
            "#ddffff",
            "#ccffff",
            "#bbffff",
            "#aaffff",
            "#99ffff",
            "#88ffff",
            "#77ffff",
            "#66ffff",
            "#55ffff",
            "#44ffff",
            "#33ffff",
            "#22ffff",
            "#11ffff",
            "#00ffff"
        };

        public Brush getBrush(int distance)
        {
            if (distance > 15)
                distance = 15;
            SolidBrush sb = new SolidBrush(ColorTranslator.FromHtml(colors[distance]));
            return sb;
        }

        public float getPositionX(int nCol)
        {
            return 100 + nCol * cellSize.Width;
        }

        public float getPositionY(int nRow)
        {
            return nRow * cellSize.Height;
        }

        public RectangleF getDateCell(int nRow)
        {
            return new RectangleF(0, getPositionY(nRow), 100, cellSize.Height);
        }

        public RectangleF getItemCell(int nRow, int nCol)
        {
            return new RectangleF(getPositionX(nCol), getPositionY(nRow), cellSize.Width, cellSize.Height);
        }

        private void GuessCalendarView_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        public void Calculate(string cmd)
        {
            command = cmd;
            List<CalendarDay> days = CalendarData.days;

            if (cmd == "sum.combine.previous")
            {
                ArrayIntegers array = new ArrayIntegers();
                numStack.Clear();
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    ArrayIntegers line = numStack.AddLine();
                    int ki = 0;
                    array.Clear();
                    for (int k = i; k >= 0; k--)
                    {
                        array.IncrementDayNumbers(days[k]);
                        if (ki < 100)
                            line.nums[ki] = array.CountDiff(0);
                        ki++;
                    }
                }
            }
            else if (cmd == "digits.statistic")
            {
                ArrayIntegers array = new ArrayIntegers();
                numStack.Clear();
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    ArrayIntegers line = numStack.AddLine();
                    int ki = 0;
                    array.Clear();
                    for (int k = 0; k < days[i].numbers.Count; k++)
                    {
                        line.nums[days[i].numbers[k] % 10]++;
                    }

                    for (int k = 0; k < 10; k++)
                    {
                        line.nums[12 + line.nums[k]]++;
                    }
                }
            }
            else if (cmd == "frequency.hits")
            {
                ArrayIntegers array = new ArrayIntegers();
                numStack.Clear();
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    ArrayIntegers line = numStack.AddLine();
                    int ki = 0;
                    array.Clear();
                    for (int k = i - 1; k >= i - 20 && k >= 0; k--)
                    {
                        for (int j = 0; j < days[k].numbers.Count; j++)
                        {
                            array.nums[days[k].numbers[j]]++;
                        }
                    }

                    for (int k = 0; k < days[i].numbers.Count; k++)
                    {
                        line.nums[array.nums[days[i].numbers[k]]]++;
                    }
                    /*for (int k = 0; k < 100; k++)
                    {
                        line.nums[array.nums[k]]++;
                    }*/
                }
            }
            else if (cmd == "frequency.hits.p6-8")
            {
                ArrayIntegers array = new ArrayIntegers();
                numStack.Clear();
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    ArrayIntegers line = numStack.AddLine();
                    int ki = 0;
                    array.Clear();
                    for (int m = 0; m < 100; m++)
                    {
                        ki = 0;
                        for (int k = i - 1; k >= i - 20 && k >= 0; k--)
                        {
                            if (days[k].Contains(m))
                                ki++;
                        }
                        if (ki >= 6 && ki <= 8)
                        {
                            line.nums[m] = m;
                            if (days[i].Contains(m))
                                line.colors[m] = 5;
                        }
                    }
                }
            }
            else if (cmd == "frequency.hits.p5-7")
            {
                FrequencyHits(5, 7);
            }
            else if (cmd == "frequency.hits.p4-6")
            {
                FrequencyHits(4, 6);
            }
            else if (cmd == "frequency.hits.p3-5")
            {
                FrequencyHits(3, 5);
            }
            else if (cmd == "proposal.a5-7")
            {
                ProposalA(5, 7);
            }
            else if (cmd == "vector.distance.sum")
            {
                numStack.Clear();
                CalendarDay last;
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    last = days[i];
                    ArrayIntegers line = numStack.AddLine();
                    int ki = 0;
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (ki < 100)
                            line.nums[ki] = last.VectorDistanceSum(days[k]);
                        ki++;
                    }
                }
            }
            else if (cmd == "vector.distance.min")
            {
                numStack.Clear();
                CalendarDay last;
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    last = days[i];
                    ArrayIntegers line = numStack.AddLine();
                    int ki = 0;
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (ki < 100)
                            line.nums[ki] = last.VectorDistanceMin(days[k]);
                        ki++;
                    }
                }
            }
            else if (cmd == "vector.distance.max")
            {
                numStack.Clear();
                CalendarDay last;
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    last = days[i];
                    ArrayIntegers line = numStack.AddLine();
                    int ki = 0;
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (ki < 100)
                            line.nums[ki] = last.VectorDistanceMax(days[k]);
                        ki++;
                    }
                }
            }
            else if (cmd == "min.nums")
            {
                numStack.Clear();
                CalendarDay last;
                int j;
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    ArrayIntegers line = numStack.AddLine();
                    last = days[i];
                    for (int k = i; k >= 0; k--)
                    {
                        for (int m = 0; m < days[k].numbers.Count; m++)
                        {
                            if (line.nums[m] < 1 || line.nums[m] > days[k].numbers[m])
                                line.nums[m] = days[k].numbers[m];
                        }
                    }
                }
            }
            else if (cmd == "max.nums")
            {
                numStack.Clear();
                CalendarDay last;
                int j;
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    ArrayIntegers line = numStack.AddLine();
                    last = days[i];
                    for (int k = i; k >= 0; k--)
                    {
                        for (int m = 0; m < days[k].numbers.Count; m++)
                        {
                            if (line.nums[m] < 1 || line.nums[m] < days[k].numbers[m])
                                line.nums[m] = days[k].numbers[m];
                        }
                    }
                }
            }
            else if (cmd == "number.exist.a")
            {
                ArrayIntegers line = null;
                numStack.Clear();
                foreach (CalendarDay day in days)
                {
                    for (int i = 0; i < day.numbers.Count; i++)
                    {
                        while (numStack.Count <= i)
                        {
                            numStack.AddLine();
                        }

                        line = numStack.array[i];

                        line.nums[day.numbers[i]]++;
                    }
                }

                for (int i = 0; i < numStack.Count; i++)
                {
                    line = numStack.array[i];
                    int max = 0;
                    for (int j = 0; j < line.nums.Length; j++)
                    {
                        if (max < line.nums[j])
                            max = line.nums[j];
                    }

                    for (int j = 0; j < line.nums.Length; j++)
                    {
                        line.colors[j] = line.nums[j] * 15 / max;
                    }
                }
            }
            else if (cmd == "half.analyze")
            {
                numStack.Clear();
                CalendarDay last;
                int j;
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    ArrayIntegers line = numStack.AddLine();
                    last = days[i];
                    for (j = 1; j < last.numbers.Count / 2; j++)
                    {
                        line.nums[last.numbers[j] - last.numbers[j - 1]]++;
                    }

                    line.nums[25] = last.numbers[j - 1];
                }

                //GenerateCombinations(50, 10, 8);
            }
            else if (cmd == "insect.combine.previous")
            {
                ArrayIntegers array = new ArrayIntegers();
                CalendarDay last = null;
                numStack.Clear();
                for (int i = days.Count - 1; i >= 0; i--)
                {
                    ArrayIntegers line = numStack.AddLine();
                    int ki = 0;
                    int k = 0;
                    array.Clear();
                    // last day results
                    last = days[i];
                    // process all previous days
                    for (k = i - 1, ki = 0; k >= 0 && ki < 100; k--, ki++)
                    {
                        array.IncrementDayNumbers(days[k]);
                        line.nums[ki] = 0;
                        // compare all numbers of last day
                        // with combined numbers of previous days
                        for (int l = 0; l < last.numbers.Count; l++)
                        {
                            if (last.numbers[l] > 0 && last.numbers[l] < 100)
                            {
                                // in line.nums is count of numbers present in last day
                                // and in combined array
                                if (array.nums[last.numbers[l]] > 0)
                                    line.nums[ki]++;
                            }
                        }
                    }
                }
            }
        }

        private void GenerateCombinations(int rangeTop, int numsMax, int minMatches)
        {
            List<List<int>> combs = new List<List<int>>();
            List<int> currComb = new List<int>();
            k10accepted = 0;
            k10rejected = 0;
            GenerateCombs(rangeTop, numsMax, minMatches, 0, combs, currComb);
        }

        private void GenerateCombs(int rangeTop, int numsMax, int minMatches, int start,
            List<List<int>> combs, List<int>workComb)
        {
            for (int i = start + 1; i < rangeTop; i++)
            {
                List<int> newComb = new List<int>();
                newComb.AddRange(workComb);
                newComb.Add(i);
                if (newComb.Count == numsMax)
                {
                    TestComb(combs, newComb, minMatches);
                }
                else
                {
                    GenerateCombs(rangeTop, numsMax, minMatches, i, combs, newComb);
                }
            }
        }

        private int GetCommonNums(List<int> A, List<int> B)
        {
            int count = 0;
            if (A.Count != B.Count)
                return 0;
            for (int i = 0; i < A.Count; i++)
            {
                for (int j = 0; j < B.Count; j++)
                {
                    if (A[i] == B[j])
                        count++;
                }
            }
            return count;
        }

        private void TestComb(List<List<int>> combs, List<int> workComb, int minMatches)
        {
            for (int i = 0; i < 10; i++)
            {
                if (workComb[i] < k10min[i] || workComb[i] > k10max[i])
                {
                    k10rejected++;
                    return;
                }
            }

            foreach (List<int> item in combs)
            {
                if (GetCommonNums(item, workComb) >= minMatches)
                {
                    k10rejected++;
                    return;
                }                
            }

            k10accepted++;
            combs.Add(workComb);
        }

        private void FrequencyHits(int aFrom, int aTo)
        {
            List<CalendarDay> days = CalendarData.days;
            ArrayIntegers array = new ArrayIntegers();
            numStack.Clear();
            for (int i = days.Count - 1; i >= 0; i--)
            {
                ArrayIntegers line = numStack.AddLine();
                int ki = 0;
                int mk = 0;
                array.Clear();
                for (int m = 0; m < 100; m++)
                {
                    ki = 0;
                    for (int k = i - 1; k >= i - 20 && k >= 0; k--)
                    {
                        if (days[k].Contains(m))
                            ki++;
                    }
                    if (ki >= aFrom && ki <= aTo)
                    {
                        line.nums[m] = m;
                        if (days[i].Contains(m))
                            line.colors[m] = 5;
                    }
                }

                ki = 0;
                for (int m = 1; m < 100; m++)
                {
                    if (line.nums[m] > 0)
                    {
                        ki++;
                    }
                    if (line.colors[m] > 0)
                    {
                        line.nums[mk] = ki;
                        line.colors[mk] = 0;
                        mk++;
                    }
                }

                for (int m = mk; m < 100; m++)
                {
                    line.nums[m] = 0;
                }
            }
        }

        private void ProposalA(int aFrom, int aTo)
        {
            List<CalendarDay> days = CalendarData.days; 
            ArrayIntegers array = new ArrayIntegers();
            numStack.Clear();
            
            for (int i = days.Count - 1; i >= 0; i--)
            {
                List<int> probe_a = new List<int>();
                ArrayIntegers line = numStack.AddLine();
                int ki = 0;
                array.Clear();
                for (int m = 0; m < 100; m++)
                {
                    ki = 0;
                    for (int k = i - 1; k >= i - 20 && k >= 0; k--)
                    {
                        if (days[k].Contains(m))
                            ki++;
                    }
                    if (ki >= aFrom && ki <= aTo)
                    {
                        probe_a.Add(m);
                    }
                }

                List<List<int>> tries = GenerateTries(probe_a, 10, 8);

                foreach (List<int> list in tries)
                {
                    line.nums[days[i].CountPresent(list)]++;
                }
            }
        }

        public List<List<int>> GenerateTries(List<int> probe, int nums_in_try, int count_of_tries)
        {
            int index = 0;
            Random rnd = new Random();
            List<int> space = new List<int>();
            List<List<int>> result = new List<List<int>>();
            List<int> line = null;
            for (int trya = 0; trya < count_of_tries; trya++)
            {
                if (space.Count >= nums_in_try)
                {
                    line = new List<int>();
                    for (int k = 0; k < nums_in_try; k++)
                    {
                        index = rnd.Next(space.Count - 1);
                        line.Add(space[index]);
                        //space.RemoveAt(index);
                    }
                    result.Add(line);
                }
                else
                {
                    space.Clear();
                    space.AddRange(probe);
                }
            }

            return result;
        }

    }
}
