using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket.Test
{
    public class CartCsvData
    {
        public static IEnumerable GetTestCasesData(string filename)
        {
            string path = $@"{ AppDomain.CurrentDomain.BaseDirectory }\{filename}";
            string[] lines = File.ReadAllLines(path);

            List<TestCaseData> testCasesData = new List<TestCaseData>();
            foreach(string line in lines)
            {
                string[] values = line.Split(',');
                int chair_counter = Int32.Parse(values[0]);
                int computer_counter = Int32.Parse(values[1]); 
                int keyboard_counter = Int32.Parse(values[2]);
                int laptop_counter = Int32.Parse(values[3]); 
                int monitor_counter = Int32.Parse(values[4]);
                int amount = Int32.Parse(values[5]);
                int expected = Int32.Parse(values[6]);
                //int size = monitor_counter + keyboard_counter + laptop_counter + computer_counter + chair_counter;
                testCasesData.Add(new TestCaseData(chair_counter, computer_counter, keyboard_counter, laptop_counter, monitor_counter, amount, expected));

            }
            return testCasesData;

        }
    }
}
