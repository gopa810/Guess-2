using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace GuessApp
{
    public class CalendarData
    {
        public static Dictionary<string,List<CalendarDay>> dba = new Dictionary<string,List<CalendarDay>>();
        public static List<CalendarDay> days = new List<CalendarDay>();
        public static Dictionary<string, string> databases;

        public static Dictionary<string,string> Databases
        {
            get
            {
                if (databases != null)
                    return databases;

                databases = new Dictionary<string, string>();
                databases["Keno10"] = "data.txt";
                databases["5z35"] = "data35.txt";
                databases["6z49"] = "data49.txt";

                return databases;
            }
        }
        

        public static void SetDatabase(string name)
        {
            if (dba.ContainsKey(name))
                days = dba[name];
            else
            {
                days = new List<CalendarDay>();
                dba[name] = days;
            }
        }

        public static int FindRow(int nNumber, int startingRow)
        {
            for (int j = startingRow; j < days.Count; j++)
            {
                CalendarDay cd = days[j];
                if (cd.Contains(nNumber))
                    return j - startingRow;
            }
            return days.Count - startingRow;
        }

        public static void Load()
        {
            foreach (string s in Databases.Keys)
            {
                Load(s);
            }
        }

        public static void Load(string dbName)
        {
            if (dba.ContainsKey(dbName))
                dba.Remove(dbName);
            string fielPath = GetFilePath(dbName);
            if (!File.Exists(fielPath))
                return;
            string[] lines = File.ReadAllLines(GetFilePath(dbName));
            if (lines != null)
            {
                days = new List<CalendarDay>();
                days.Clear();
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(',');
                    int c = -1;
                    CalendarDay cd = new CalendarDay();
                    for (int j = 0; j < parts.Length; j++)
                    {
                        long pp;
                        if (long.TryParse(parts[j], out pp))
                        {
                            if (c == -1)
                            {
                                cd.date = new DateTime(pp);
                            }
                            else
                            {
                                cd.numbers.Add(Convert.ToInt32(pp));
                            }
                            c++;
                        }
                    }
                    days.Add(cd);
                }
            }

            dba[dbName] = days;
        }

        public static void Save()
        {
            foreach (string s in dba.Keys)
            {
                Save(s, dba[s]);
            }
        }

        public static void Save(string dbName, List<CalendarDay> theDays)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < theDays.Count; i++)
            {
                CalendarDay cd = theDays[i] as CalendarDay;
                StringBuilder sc = new StringBuilder();
                sc.AppendFormat("{0},", cd.date.Ticks);
                for (int j = 0; j < cd.numbers.Count; j++)
                {
                    sc.AppendFormat("{0},", cd.numbers[j]);
                }
                sb.AppendLine(sc.ToString());
            }

            File.WriteAllText(GetFilePath(dbName), sb.ToString());
            Debugger.Log(0, "debug", sb.ToString());
            Debugger.Log(0, "debug", GetFilePath(dbName) + "\n");
        }

        public static string FileNameFromDatabaseName(string dbName)
        {
            if (Databases.ContainsKey(dbName))
                return Databases[dbName];
            return "datamis.txt";
        }

        public static string GetFilePath(string dbName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileNameFromDatabaseName(dbName));
        }
    }

    public class NumberStack
    {
        public List<ArrayIntegers> array = new List<ArrayIntegers>();

        public ArrayIntegers AddLine()
        {
            ArrayIntegers arr = new ArrayIntegers();
            array.Add(arr);
            return arr;
        }

        public void Clear()
        {
            array.Clear();
        }

        public int Count
        {
            get
            {
                return array.Count;
            }
        }
    }
}
