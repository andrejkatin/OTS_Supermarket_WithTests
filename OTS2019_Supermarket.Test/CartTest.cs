using NUnit.Framework;
using OTS_Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket.Test
{
    [TestFixture]
    public class CartTest
    {
        Cart cart;

        [SetUp]
        public void SetUp() {
            cart = new Cart();
        }

        #region AddOneToCart
       
        [Test]
        public void AddOneToCart_OKInput_Success()
        {
            Monitor monitor = new Monitor();

            cart.AddOneToCart(monitor);

            Assert.That(1, Is.EqualTo(cart.Size));
        }

        [Test]
        public void AddOneToCart_SizeGreaterThanTen_Exception()
        {
            Monitor monitor = new Monitor();
            cart.Size = 10;

            Exception ex = Assert.Throws<Exception>(() => cart.AddOneToCart(monitor));
            Assert.That(ex.Message, Is.EqualTo("Number of items in cart must be 10 or less!"));
        }
        #endregion

        #region AddMultipleToCart
        
        [Test]
        public void AddMultipleToCart_ItemsCountOK_Success()
        {

            var item = new Keyboard();
            int amount = 5;

            cart.AddMultipleToCart(item, amount);

            Assert.That(5, Is.EqualTo(cart.Items.Count));
        }

        [Test]
        public void AddMultipleToCart_SizeGreaterThanTen_Exception()
        {
            Keyboard keyboard = new Keyboard();
            cart.Size = 6;

            Exception ex = Assert.Throws<Exception>(() => cart.AddMultipleToCart(keyboard, 5));
            Assert.That(ex.Message, Is.EqualTo("Number of items in cart must be 10 or less!"));
        }
        #endregion

        #region DeleteAll
        [Test]
        public void DeleteAll_SizeOK_Success() {
            cart.Size = 5;
            
            cart.DeleteAll();

            Assert.That(0, Is.EqualTo(cart.Size));
        }

        [Test]
        public void DeleteAll_ItemsCountOK_Success()
        {
            cart.Items = new List<Item>
            {
                new Computer(),
                new Laptop()
            };
            cart.Size = 2;

            cart.DeleteAll();

            Assert.That(0, Is.EqualTo(cart.Items.Count));
        }

        [Test]
        public void DeleteAll_EmptyCart_Exception() {

            Exception ex = Assert.Throws<Exception>(() => cart.DeleteAll());
            Assert.That("Cannot restore empty cart!", Is.EqualTo(ex.Message));

        }
        #endregion

        #region Print
       
        [Test]
        public void Print_OutputStringOK_Success() {

            cart.Items = new List<Item>
            {
                new Computer(),
                new Laptop()
            };
            cart.Size = 2;
            string expected = "\nItem: Computer; Price: 1200\nItem: Laptop; Price: 800";

            string printString = cart.Print();

            Assert.That(expected, Is.EqualTo(printString));

        }

        [Test]
        public void Print_EmptyCart_Exception()
        {
            Exception ex = Assert.Throws<Exception>(() => cart.Print());
            Assert.That("Cannot print empty cart!", Is.EqualTo(ex.Message));
        }
        #endregion

        #region Calculate
        
        [Test]
        public void Calculate_BudgetOK_Success() {
            
            cart.Budget = 5000;
            cart.Items = new List<Item>
            {
                new Computer(),
                new Laptop()
            };
            cart.Amount = 2000;
            cart.Size = 2;
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            string input = GetStringInDateFormat(date);

            cart.Calculate(input);

            Assert.That(3000, Is.EqualTo(cart.Budget));
        }

        [Test]
        public void Calculate_WrongDateFormat_Exception() {
            cart.Budget = 5000;
            cart.Items = new List<Item>
            {
                new Computer(),
                new Laptop()
            };
            cart.Size = 2;
            cart.Amount = 2000;
            DateTime date = DateTime.Now;
            string input = date.Year.ToString() + "." + date.Month.ToString() + "." + date.Day.ToString();

            Exception ex = Assert.Throws<Exception>(() => cart.Calculate(input));
            Assert.That(ex.Message, Is.EqualTo("Wrong date format! Date must be in format yyyy-MM-dd"));
        }


        [Test]
        public void Calculate_TodaysDeliveryDate_Exception() {
            cart.Budget = 5000;
            cart.Items = new List<Item>
            {
                new Computer(),
                new Laptop()
            };
            cart.Size = 2;
            cart.Amount = 2000;
            DateTime date = DateTime.Now;
            string input = GetStringInDateFormat(date);

            Exception ex = Assert.Throws<Exception>(() => cart.Calculate(input));
            Assert.That(ex.Message, Is.EqualTo("Date of delivery can't be today's date!"));
        }

        [Test]
        public void Calculate_DeliveryDateAfterMoreThanSevenDays_Exception() {
            cart.Items = new List<Item>
            {
                new Computer(),
                new Laptop()
            };
            cart.Size = 2;
            cart.Amount = 2000;
            DateTime date = DateTime.Now;
            date = date.AddDays(8);
            string input = GetStringInDateFormat(date);

            Exception ex = Assert.Throws<Exception>(() => cart.Calculate(input));
            Assert.That(ex.Message, Is.EqualTo("Days for delivery must be less than 7!"));
        }

        [Test]
        public void Calculate_BudgetSmallerThanAmount_Exception() {
            cart.Items = new List<Item>
            {
                new Computer(),
                new Laptop()
            };
            cart.Size = 2;
            cart.Amount = 2000;
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            string input = GetStringInDateFormat(date);

            Exception ex = Assert.Throws<Exception>(() => cart.Calculate(input));
            Assert.That(ex.Message, Is.EqualTo("Not enough budget!"));
        }

        #region discount test
        [TestCase(3, 0, 0, 3, 3, 6360, ExpectedResult = 636)]
        [TestCase(2, 3, 1, 0, 3, 4190, ExpectedResult = 419)]
        public double Calculate_TenPercentDiscount_Success(int chair_counter, int computer_counter, int keyboard_counter, int laptop_counter, int monitor_counter, int amount)
        {
            cart.Size = chair_counter + computer_counter + keyboard_counter + laptop_counter + monitor_counter;
            cart.Chair_counter = chair_counter;
            cart.Computer_counter = computer_counter;
            cart.Keyboard_counter = keyboard_counter;
            cart.Laptop_counter = laptop_counter;
            cart.Monitor_counter = monitor_counter;
            cart.Amount = amount;
            cart.Budget = amount;
            DateTime date = DateTime.Now;
            int dayOfTheWeek = (int)date.DayOfWeek; //Sunday is 0, Saturday 6
            date = (dayOfTheWeek == 0 || dayOfTheWeek == 6) ? date.AddDays(2) : date.AddDays(1);
            if ((int)date.DayOfWeek == 6) //if DateTime.Now is friday
            {
                date = date.AddDays(2);
            }
            string input = GetStringInDateFormat(date);

            cart.Calculate(input);

            return cart.Budget;
        }

        [TestCase(7, 1, 0, 1, 0, 2840, ExpectedResult = 227.2)]
        [TestCase(3, 0, 5, 1, 0, 1410, ExpectedResult = 112.8)]
        public double Calculate_EightPercentDiscount_Success(int chair_counter, int computer_counter, int keyboard_counter, int laptop_counter, int monitor_counter, int amount)
        {
            cart.Size = chair_counter + computer_counter + keyboard_counter + laptop_counter + monitor_counter;
            cart.Chair_counter = chair_counter;
            cart.Computer_counter = computer_counter;
            cart.Keyboard_counter = keyboard_counter;
            cart.Laptop_counter = laptop_counter;
            cart.Monitor_counter = monitor_counter;
            cart.Amount = amount;
            cart.Budget = amount;
            DateTime date = DateTime.Now;
            int dayOfTheWeek = (int)date.DayOfWeek; //Sunday is 0, Saturday 6
            date = (dayOfTheWeek == 0 || dayOfTheWeek == 6) ? date.AddDays(2) : date.AddDays(1);
            if ((int)date.DayOfWeek == 6) //if DateTime.Now is friday
            {
                date = date.AddDays(2);
            }
            string input = GetStringInDateFormat(date);

            cart.Calculate(input);

            return Math.Round(cart.Budget,1);
        }

        [TestCaseSource(typeof(CartTestData), "Calculate_FivePercentDiscount_Success_TestData")]
        public void Calculate_FivePercentDiscount_Success(int chair_counter, int computer_counter, int keyboard_counter, int laptop_counter, int monitor_counter, int amount)
        {
            cart.Size = chair_counter + computer_counter + keyboard_counter + laptop_counter + monitor_counter;
            cart.Chair_counter = chair_counter;
            cart.Computer_counter = computer_counter;
            cart.Keyboard_counter = keyboard_counter;
            cart.Laptop_counter = laptop_counter;
            cart.Monitor_counter = monitor_counter;
            cart.Amount = amount;
            cart.Budget = amount;
            DateTime date = DateTime.Now;
            int dayOfTheWeek = (int)date.DayOfWeek; //Sunday is 0, Saturday 6
            date = (dayOfTheWeek == 0 || dayOfTheWeek == 6) ? date.AddDays(2) : date.AddDays(1);
            string input = GetStringInDateFormat(date);
            double discountPrice = amount * 0.95;
            double newBudget = amount - discountPrice;

            cart.Calculate(input);

            Assert.That(cart.Budget, Is.EqualTo(newBudget));
        }

        [TestCaseSource(typeof(CartCsvData), "GetTestCasesData", new object[] { "twenty_percent_disconut_data.csv" })]
        public void Calculate_TwentyPercentDiscount_Success(int chair_counter, int computer_counter, int keyboard_counter, int laptop_counter, int monitor_counter, int amount, int expected)
        {
            cart.Size = chair_counter + computer_counter + keyboard_counter + laptop_counter + monitor_counter;
            cart.Chair_counter = chair_counter;
            cart.Computer_counter = computer_counter;
            cart.Keyboard_counter = keyboard_counter;
            cart.Laptop_counter = laptop_counter;
            cart.Monitor_counter = monitor_counter;
            cart.Amount = amount;
            cart.Budget = amount;
            DateTime date = DateTime.Now;
            int dayOfTheWeek = (int)date.DayOfWeek; //Sunday is 0, Saturday 6
            date = (dayOfTheWeek == 2 || dayOfTheWeek == 3) ? date.AddDays(6) : date.AddDays(4);
            string input = GetStringInDateFormat(date);

            cart.Calculate(input);

            Assert.That(cart.Budget, Is.EqualTo(expected));
        }

        [TestCaseSource(typeof(CartTestData), "Calculate_EighteenPercentDiscount_Success_TestDataWithReturns")]
        public double Calculate_EighteenPercentDiscount_Success(int chair_counter, int computer_counter, int keyboard_counter, int laptop_counter, int monitor_counter, int amount)
        {
            cart.Size = chair_counter + computer_counter + keyboard_counter + laptop_counter + monitor_counter;
            cart.Chair_counter = chair_counter;
            cart.Computer_counter = computer_counter;
            cart.Keyboard_counter = keyboard_counter;
            cart.Laptop_counter = laptop_counter;
            cart.Monitor_counter = monitor_counter;
            cart.Amount = amount;
            cart.Budget = amount;
            DateTime date = DateTime.Now;
            int dayOfTheWeek = (int)date.DayOfWeek; //Sunday is 0, Saturday 6
            date = (dayOfTheWeek == 2 || dayOfTheWeek == 3) ? date.AddDays(6) : date.AddDays(4);
            string input = GetStringInDateFormat(date);
            double discountPrice = amount * 0.82;
            double newBudget = amount - discountPrice;

            cart.Calculate(input);

            return cart.Budget; 
        }

        [TestCaseSource(typeof(CartCsvData), "GetTestCasesData", new object[] { "fifteen_percent_disconut_data.csv" })]
        public void Calculate_FifteenPercentDiscount_Success(int chair_counter, int computer_counter, int keyboard_counter, int laptop_counter, int monitor_counter, int amount, int expected)
        {
            cart.Size = chair_counter + computer_counter + keyboard_counter + laptop_counter + monitor_counter;
            cart.Chair_counter = chair_counter;
            cart.Computer_counter = computer_counter;
            cart.Keyboard_counter = keyboard_counter;
            cart.Laptop_counter = laptop_counter;
            cart.Monitor_counter = monitor_counter;
            cart.Amount = amount;
            cart.Budget = amount;
            DateTime date = DateTime.Now;
            int dayOfTheWeek = (int)date.DayOfWeek; //Sunday is 0, Saturday 6
            date = (dayOfTheWeek == 2 || dayOfTheWeek == 3) ? date.AddDays(6) : date.AddDays(4);
            string input = GetStringInDateFormat(date);

            cart.Calculate(input);

            Assert.That(cart.Budget, Is.EqualTo(expected));
        }

        [Test]
        public void Calculate_NoMoreThanFivePrecentDiscountOnWeekend_Success() {
            cart.Size = 6;
            cart.Laptop_counter = 4;
            cart.Computer_counter = 1;
            cart.Monitor_counter = 1;
            cart.Amount = 3750;
            cart.Budget = 3750;
            DateTime date = DateTime.Now;
            int dayOfTheWeek = (int)date.DayOfWeek; //Sunday is 0, Saturday 6
            int addDays = (dayOfTheWeek == 0 || dayOfTheWeek == 6) ? 7 : (6 - dayOfTheWeek);
            date = date.AddDays(addDays);
            double discountPrice = 3750;
            if (addDays <= 3) {
                discountPrice = 3750 * 0.95;
            }
            string input = GetStringInDateFormat(date);
            
            double newBudget = 3750 - discountPrice;

            cart.Calculate(input);

            Assert.That(cart.Budget, Is.EqualTo(newBudget));
        }

        [TestCaseSource(typeof(CartTxtParser), "GetTestCasesData", new object[] { "PICTResults.txt" })]
        public void Calculate_PICTResultsTesting(int days, int size, int amount, int laptop, int monitor, int chair, int computer, int keyboard, bool isNotWeekend, double discount)
        {
            Cart cart = new Cart();
            cart.Size = size;
            cart.Monitor_counter = monitor;
            cart.Laptop_counter = laptop;
            cart.Computer_counter = computer;
            cart.Keyboard_counter = keyboard;
            cart.Chair_counter = chair;
            cart.Amount = amount;
            cart.Budget = amount;

            DateTime date = DateTime.Now;
            int dayOfTheWeek = days - 1; //Sunday is 0, Saturday 6
            date = (dayOfTheWeek == 2 || dayOfTheWeek == 3) ? date.AddDays(6) : date.AddDays(4);
            string input = GetStringInDateFormat(date);

            cart.Calculate(input);

            Assert.That(cart.Budget, Is.EqualTo(discount));
        }
        #endregion
        #endregion

        private string GetStringInDateFormat(DateTime date) 
        {
            return date.Year.ToString() + "-" + (date.Month >= 10 ? date.Month.ToString() : "0" + date.Month.ToString()) + "-" + (date.Day >= 10 ? date.Day.ToString() : "0" + date.Day.ToString());
        }

    }
}
