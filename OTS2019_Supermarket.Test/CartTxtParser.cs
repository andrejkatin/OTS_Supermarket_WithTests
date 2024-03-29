using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket.Test
{
    public class CartTxtParser
    {
        public static IEnumerable<TestCaseData> GetTestCasesData(string fileName)
        {
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}{fileName}";
            string[] lines = File.ReadAllLines(path);

            List<TestCaseData> testCases = new List<TestCaseData>();
            foreach (string line in lines)
            {
                string[] chars = line.Split('\t');
                int days = Convert.ToInt32(chars[0]);
                int size = Convert.ToInt32(chars[1]);
                int amount = Convert.ToInt32(chars[2]);
                int laptop = Convert.ToInt32(chars[3]);
                int monitor = Convert.ToInt32(chars[4]);
                int chair = Convert.ToInt32(chars[5]);
                int computer = Convert.ToInt32(chars[6]);
                int keyboard = Convert.ToInt32(chars[7]);
                bool isNotWeekend = Convert.ToBoolean(chars[8]);
                double discount = Convert.ToDouble(chars[9]);

                testCases.Add(new TestCaseData(days, size, amount, laptop, monitor, chair, computer, keyboard, isNotWeekend, discount));
            }
            return testCases;
        }
    }
}
