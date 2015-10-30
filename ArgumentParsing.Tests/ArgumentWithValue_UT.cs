using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;

namespace ArgumentParsing.Tests
{
    [TestFixture]
    public class ArgumentWithValue_UT
    {
        private IStringParser _stringParser;

        [SetUp]
        public void SetUp()
        {
            _stringParser = MockRepository.GenerateStrictMock<IStringParser>();
        }

        [TearDown]
        public void TearDown()
        {
            _stringParser.VerifyAllExpectations();
        }

        [Test]
        public void Constructor_Throws_Exception_When_Created_With_Invalid_Type()
        {
            Assert.Throws<TypeLoadException>(() => new ArgumentWithValue<object>(null, _stringParser));
            Assert.Throws<TypeLoadException>(() => new ArgumentWithValue<byte>(null, _stringParser));
            Assert.Throws<TypeLoadException>(() => new ArgumentWithValue<dynamic>(null, _stringParser));
        }

        [Test]
        public void Constructor_Allows_Creation_With_Supported_Types()
        {
            new ArgumentWithValue<char>(null, _stringParser);
            new ArgumentWithValue<string>(null, _stringParser);
            new ArgumentWithValue<short>(null, _stringParser);
            new ArgumentWithValue<ushort>(null, _stringParser);
            new ArgumentWithValue<int>(null, _stringParser);
            new ArgumentWithValue<uint>(null, _stringParser);
            new ArgumentWithValue<long>(null, _stringParser);
            new ArgumentWithValue<ulong>(null, _stringParser);
            new ArgumentWithValue<double>(null, _stringParser);
            new ArgumentWithValue<decimal>(null, _stringParser);
            new ArgumentWithValue<bool>(null, _stringParser);
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentName_When_Argument_Name_Is_Incorrect()
        {
            var testObject = new ArgumentWithValue<string>(null, _stringParser);
            var actual = testObject.TrySetArgument("some arg");
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
            Assert.That(testObject.ParsedArgumentValue, Is.Null);
        }

        [Test]
        public void TrySetArgument_Returns_Success_When_Argument_Name_Is_Correct_And_No_Value_Required()
        {
            const string argumentName = "someArg";
            var testObject = new ArgumentWithValue<string>(new List<string> { argumentName }, _stringParser);

            var actual = testObject.TrySetArgument(argumentName);
            Assert.That(actual, Is.EqualTo(SetArgumentResult.Success));
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(argumentName));
            Assert.That(testObject.ParsedArgumentValue, Is.Null);
        }

        [Test]
        public void TrySetArgument_Returns_Success_When_Argument_Name_Matches_Any_Of_The_Possible_Values()
        {
            const string possibleArgumentName1 = "someArg";
            const string possibleArgumentName2 = "someArg2";
            var testObject = new ArgumentWithValue<string>(new List<string> { possibleArgumentName1, possibleArgumentName2 }, _stringParser);

            var actual = testObject.TrySetArgument(possibleArgumentName1);
            Assert.That(actual, Is.EqualTo(SetArgumentResult.Success));
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(possibleArgumentName1));
            Assert.That(testObject.ParsedArgumentValue, Is.Null);

            actual = testObject.TrySetArgument(possibleArgumentName2);
            Assert.That(actual, Is.EqualTo(SetArgumentResult.Success));
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(possibleArgumentName2));
            Assert.That(testObject.ParsedArgumentValue, Is.Null);
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentName_When_Argument_Name_Does_Not_Match_Any_Of_The_Possible_Values()
        {
            const string possibleArgumentName1 = "someArg";
            const string possibleArgumentName2 = "someArg2";
            var testObject = new ArgumentWithValue<string>(new List<string> { possibleArgumentName1, possibleArgumentName2 }, _stringParser);

            var actual = testObject.TrySetArgument("someInvalidArg");
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
            Assert.That(testObject.ParsedArgumentValue, Is.Null);

            actual = testObject.TrySetArgument("someArg3");
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
            Assert.That(testObject.ParsedArgumentValue, Is.Null);
        }

        [Test]
        public void TrySetArgument_Ignores_Case_Based_On_StringComparison_Property()
        {
            const string argumentName = "someArg";
            var testObject = new ArgumentWithValue<string>(new List<string> { argumentName }, _stringParser)
            {
                ArgumentNameStringComparison = StringComparison.CurrentCultureIgnoreCase
            };


            var actual = testObject.TrySetArgument(argumentName.ToLower());
            Assert.That(actual, Is.EqualTo(SetArgumentResult.Success));

            testObject = new ArgumentWithValue<string>(new List<string> { argumentName }, _stringParser)
            {
                ArgumentNameStringComparison = StringComparison.CurrentCulture
            };

            actual = testObject.TrySetArgument(argumentName.ToLower());
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
            Assert.That(testObject.ParsedArgumentValue, Is.Null);
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentValue_When_Value_Expected_But_Not_Set()
        {
            const string argumentName = "someArg";
            var testObject = new ArgumentWithValue<string>(new List<string> { argumentName }, _stringParser);

            string parsedValue;
            _stringParser.Expect(s => s.TryParse(null, out parsedValue)).Return(false);

            var actual = testObject.TrySetArgument(argumentName);
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentValue));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
            Assert.That(testObject.ParsedArgumentValue, Is.Null);
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentValue_When_Value_Expected_But_Not_Set_To_Parsable_Value()
        {
            const string argumentName = "someArg";
            var testObject = new ArgumentWithValue<int>(new List<string> { argumentName }, _stringParser);

            int parsedValue;
            _stringParser.Expect(s => s.TryParse(null, out parsedValue)).Return(false);

            var actual = testObject.TrySetArgument(argumentName);
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentValue));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
            Assert.That(testObject.ParsedArgumentValue, Is.EqualTo(0));
        }
    }
}
