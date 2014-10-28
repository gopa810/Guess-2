using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuessApp
{
    public partial class DayDialog : Form
    {
        public CalendarDay dayData = null;

        public DayDialog()
        {
            InitializeComponent();
        }

        public DayDialog(CalendarDay cd)
        {
            InitializeComponent();
            if (cd != null)
            {
                dayData = cd;
                monthCalendar1.SelectionStart = dayData.date;

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dayData.numbers.Count; i++)
                {
                    if (sb.Length > 0)
                        sb.Append(',');
                    sb.AppendFormat("{0}", dayData.numbers[i]);
                }
                richTextBox1.Text = sb.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dayData = new CalendarDay();
            dayData.date = monthCalendar1.SelectionStart;
            string text = richTextBox1.Text.Replace('\n', ' ').Replace('\r', ' ');
            string[] parts = text.Split(',');
            int top = parts.Length > 20 ? 20 : parts.Length;
            for (int i = 0; i < top; i++)
            {
                int val = 0;
                string p = parts[i].Trim();
                if (int.TryParse(p, out val))
                {
                    dayData.numbers.Add(val);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dayData = null;
        }
    }
}
