using System.Collections.Generic;
using System.IO;
using Xunit;

namespace LL1Generator.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var expectedCase = new StreamReader("../../../Expected/1.test");
            const string way = "../../../TestCases/1.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test2()
        {
            var expectedCase = new StreamReader("../../../Expected/2.test");
            const string way = "../../../TestCases/2.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test3()
        {
            var expectedCase = new StreamReader("../../../Expected/3.test");
            const string way = "../../../TestCases/3.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test4()
        {
            var expectedCase = new StreamReader("../../../Expected/4.test");
            const string way = "../../../TestCases/4.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test5()
        {
            var expectedCase = new StreamReader("../../../Expected/5.test");
            const string way = "../../../TestCases/5.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test6()
        {
            var expectedCase = new StreamReader("../../../Expected/6.test");
            const string way = "../../../TestCases/6.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test7()
        {
            var expectedCase = new StreamReader("../../../Expected/7.test");
            const string way = "../../../TestCases/7.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test8()
        {
            var expectedCase = new StreamReader("../../../Expected/8.test");
            const string way = "../../../TestCases/8.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test9()
        {
            const string way = "../../../TestCases/9.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);

            Assert.Null(actual);
        }

        [Fact]
        public void Test10()
        {
            const string way = "../../../TestCases/10.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();

            Assert.Null(actual);
        }

        [Fact]
        public void Test11()
        {
            var expectedCase = new StreamReader("../../../Expected/11.test");
            const string way = "../../../TestCases/11.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test12()
        {
            var expectedCase = new StreamReader("../../../Expected/12.test");
            const string way = "../../../TestCases/12.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test13()
        {
            const string way = "../../../TestCases/13.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);

            Assert.Null(actual);
        }

        [Fact]
        public void Test14()
        {
            var expectedCase = new StreamReader("../../../Expected/14.test");
            const string way = "../../../TestCases/14.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test15()
        {
            const string way = "../../../TestCases/15.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            Assert.Null(actual);
        }

        [Fact]
        public void Test16()
        {
            var expectedCase = new StreamReader("../../../Expected/16.test");
            const string way = "../../../TestCases/16.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test17()
        {
            var expectedCase = new StreamReader("../../../Expected/17.test");
            const string way = "../../../TestCases/17.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test18()
        {
            var expectedCase = new StreamReader("../../../Expected/18.test");
            const string way = "../../../TestCases/18.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test19()
        {
            var expectedCase = new StreamReader("../../../Expected/19.test");
            const string way = "../../../TestCases/19.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test20()
        {
            var expectedCase = new StreamReader("../../../Expected/20.test");
            const string way = "../../../TestCases/20.test";
            var rules = new List<string>();

            var p = new Program();
            var actual = Program.CheckTests(way, rules);
            var expected = new List<string>();
            string line;
            while ((line = expectedCase.ReadLine()) != null)
            {
                expected.Add(line);
            }

            Assert.Equal(expected, actual);
        }
    }
}