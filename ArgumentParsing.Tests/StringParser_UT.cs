using System;
using System.Globalization;
using NUnit.Framework;

namespace ArgumentParsing.Tests
{
    [TestFixture]
    public class StringParser_UT
    {
        private StringParser _testObject;

        [SetUp]
        public void SetUp()
        {
            _testObject = new StringParser();
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Short()
        {
            const string argumentValueString = "12345678910";

            short parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Short()
        {
            const short expectedArgumentValue = -10;
            var argumentValueString = expectedArgumentValue.ToString();

            short parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Ushort()
        {
            const string argumentValueString = "-10";

            ushort parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Ushort()
        {
            const ushort expectedArgumentValue = 10;
            var argumentValueString = expectedArgumentValue.ToString();

            ushort parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Int()
        {
            const string argumentValueString = "123456789101111111";

            int parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Int()
        {
            const int expectedArgumentValue = -1000000;
            var argumentValueString = expectedArgumentValue.ToString();

            int parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Uint()
        {
            const string argumentValueString = "-12345678910";

            uint parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Uint()
        {
            const uint expectedArgumentValue = 1000000;
            var argumentValueString = expectedArgumentValue.ToString();

            uint parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Long()
        {
            const string argumentValueString = "w";

            long parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Long()
        {
            const long expectedArgumentValue = long.MaxValue;
            var argumentValueString = expectedArgumentValue.ToString();

            long parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Ulong()
        {
            const string argumentValueString = "-12345678910";

            ulong parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Ulong()
        {
            const ulong expectedArgumentValue = ulong.MaxValue;
            var argumentValueString = expectedArgumentValue.ToString();

            ulong parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Char()
        {
            const string argumentValueString = "AB";

            char parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Char()
        {
            const char expectedArgumentValue = 'A';
            var argumentValueString = expectedArgumentValue.ToString();

            char parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Double()
        {
            const string argumentValueString = "1.23a";

            double parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Double()
        {
            const double expectedArgumentValue = 1.2345;
            var argumentValueString = expectedArgumentValue.ToString();

            double parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Decimal()
        {
            const string argumentValueString = "1.23a";

            decimal parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Decimal()
        {
            const decimal expectedArgumentValue = decimal.MaxValue;
            var argumentValueString = expectedArgumentValue.ToString();

            decimal parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_Value_Not_Set_To_Parsable_Bool()
        {
            const string argumentValueString = "T";

            bool parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Bool()
        {
            const bool expectedArgumentValue = true;
            var argumentValueString = expectedArgumentValue.ToString();

            bool parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_False_When_ValueString_Not_Set_To_Parsable_Date()
        {
            var argumentValueString = "a";
            DateTime parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);
            Assert.That(expectedResult, Is.False);

            argumentValueString = "10";
            expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);
            Assert.That(expectedResult, Is.False);

            argumentValueString = "10/20/u";
            expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);
            Assert.That(expectedResult, Is.False);

            argumentValueString = "8:";
            expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);
            Assert.That(expectedResult, Is.False);
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Only_Month_And_Day()
        {
            var expectedArgumentValue = new DateTime(DateTime.Today.Year, 10, 20);
            const string argumentValueString = "10/20";

            DateTime parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Only_A_Time()
        {
            var today = DateTime.Today;

            var expectedArgumentValue = new DateTime(today.Year, today.Month, today.Day, 8, 11, 0);
            const string argumentValueString = "8:11";

            DateTime parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_Date()
        {
            var expectedArgumentValue = new DateTime(2015, 10, 3);
            const string argumentValueString = "10/03/2015";

            DateTime parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue, Is.EqualTo(expectedArgumentValue));
        }

        [Test]
        public void TryParse_Returns_True_And_Sets_ParsedValue_When_ValueString_Set_To_Parsable_DateTime()
        {
            var expectedArgumentValue = DateTime.Now;
            var argumentValueString = expectedArgumentValue.ToString("G");

            DateTime parsedValue;
            var expectedResult = _testObject.TryParse(argumentValueString, out parsedValue);

            Assert.That(expectedResult, Is.True);
            Assert.That(parsedValue.ToString("G"), Is.EqualTo(expectedArgumentValue.ToString("G")));
        }
    }
}
