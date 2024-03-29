using NUnit.Framework;
using System;
using System.Collections; //not System.Collection.Generics
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket.Test
{
    public class CartTestData
    {
        public static IEnumerable Calculate_FivePercentDiscount_Success_TestData
        {
            get
            {
                yield return new TestCaseData(4, 1, 0, 1, 0, 2480);
                yield return new TestCaseData(0, 0, 0, 1, 5, 1700);

            }
        }
       

        public static IEnumerable Calculate_EighteenPercentDiscount_Success_TestDataWithReturns
        {
            get
            {
                yield return new TestCaseData(4, 0, 1, 1, 0, 1450).Returns(261);
                yield return new TestCaseData(0, 0, 1, 4, 1, 2550).Returns(459);

            }
        }
    }
}
