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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            CalendarData.Load();

            comboBox1.Items.AddRange(GuessCalendarView.Commands);

            comboBox2.Items.AddRange(CalendarData.Databases.Keys.ToArray<string>());
            comboBox2.SelectedIndex = 0;
            CalendarData.SetDatabase(comboBox2.SelectedItem as string);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DayDialog dd = new DayDialog();
            if (dd.ShowDialog() == DialogResult.OK)
            {
                CalendarData.days.Insert(0, dd.dayData);
                guessCalendarView1.Invalidate();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CalendarData.Save();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            guessCalendarView1.Calculate(comboBox1.SelectedItem as string);
            guessCalendarView1.Invalidate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalendarData.SetDatabase(comboBox2.SelectedItem as string);
            guessCalendarView1.Calculate(comboBox1.SelectedItem as string);
            guessCalendarView1.Invalidate();
        }
    }
}
