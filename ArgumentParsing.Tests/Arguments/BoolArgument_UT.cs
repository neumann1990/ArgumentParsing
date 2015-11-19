using System;
using System.Collections.Generic;
using ArgumentParsing.Arguments;
using NUnit.Framework;
using Rhino.Mocks;

namespace ArgumentParsing.Tests.Arguments
{
    [TestFixture]
    public class BoolArgument_UT
    {
        private IArgument _argument;
        private IStringParser _stringParser;

        [SetUp]
        public void SetUp()
        {
            _argument = MockRepository.GenerateMock<IArgument>();
            _stringParser = MockRepository.GenerateStrictMock<IStringParser>();
        }

        [TearDown]
        public void TearDown()
        {
            _argument.VerifyAllExpectations();
            _stringParser.VerifyAllExpectations();
        }

        [Test]
        public void ArgumentNameStringComparison_Get_And_Set_Pass_Through_To_Internal_Argument()
        {
            _argument.Expect(a => a.ArgumentNameStringComparison).SetPropertyWithArgument(StringComparison.CurrentCulture);
            _argument.Expect(a => a.ArgumentNameStringComparison).Return(StringComparison.InvariantCulture);

            var testObject = new BoolArgument(_argument, _stringParser)
            {
                ArgumentNameStringComparison = StringComparison.CurrentCulture
            };
            var returnedStringComparison = testObject.ArgumentNameStringComparison;

            Assert.That(returnedStringComparison, Is.EqualTo(StringComparison.InvariantCulture));
        }

        [Test]
        public void PossibleArgumentNames_Get_Passes_Through_To_Internal_Argument()
        {
            var expectedArgumentNames = MockRepository.GenerateStub<List<string>>();
            _argument.Expect(a => a.PossibleArgumentNames).Return(expectedArgumentNames);

            var testObject = new BoolArgument(_argument, _stringParser);
            var actualArgumentNames = testObject.PossibleArgumentNames;

            Assert.That(actualArgumentNames, Is.EqualTo(expectedArgumentNames));
        }

        [Test]
        public void ParsedArgumentName_Get_Passes_Through_To_Internal_Argument()
        {
            const string expectedArgumentName = "some argument name";
            _argument.Expect(a => a.ParsedArgumentName).Return(expectedArgumentName);

            var testObject = new BoolArgument(_argument, _stringParser);
            var actualArgumentName = testObject.ParsedArgumentName;

            Assert.That(actualArgumentName, Is.EqualTo(expectedArgumentName));
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentName_When_Argument_Name_Is_Incorrect()
        {
            const string argumentName = "some arg";
            _argument.Expect(a => a.TrySetArgumentName(argumentName)).Return(SetArgumentDataResult.InvalidArgumentName);

            var testObject = new BoolArgument(_argument, _stringParser);
            var actual = testObject.TrySetArgumentName(argumentName);

            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.InvalidArgumentName));
            Assert.That(testObject.ParsedSuccessfully, Is.False);
            Assert.That(testObject.ParsedArgumentValue, Is.EqualTo(default(bool)));
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentValue_When_Value_Expected_But_Not_Set()
        {
            const string argumentName = "someArg";
            var testObject = new BoolArgument(_argument, _stringParser);

            bool parsedValue;
            _stringParser.Expect(s => s.TryParse(null, out parsedValue)).Return(false);
            _argument.Expect(a => a.TrySetArgumentName(argumentName)).Return(SetArgumentDataResult.Success);

            var actual = testObject.TrySetArgumentNameAndValue(argumentName, null);
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.InvalidArgumentValue));
            Assert.That(testObject.ParsedSuccessfully, Is.False);
            Assert.That(testObject.ParsedArgumentValue, Is.EqualTo(default(bool)));
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentValue_When_Value_Expected_But_Not_Set_To_Parsable_Value()
        {
            const string argumentName = "someArg";
            const string argumentValue = "some argument";
            var testObject = new BoolArgument(_argument, _stringParser);

            bool parsedValue;
            _stringParser.Expect(s => s.TryParse(argumentValue, out parsedValue)).Return(false);
            _argument.Expect(a => a.TrySetArgumentName(argumentName)).Return(SetArgumentDataResult.Success);

            var actual = testObject.TrySetArgumentNameAndValue(argumentName, argumentValue);
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.InvalidArgumentValue));
            Assert.That(testObject.ParsedSuccessfully, Is.False);
            Assert.That(testObject.ParsedArgumentValue, Is.EqualTo(default(bool)));
        }

        [Test]
        public void TrySetArgument_Returns_Success_And_Sets_Parsing_Related_Properties_When_No_Problems_Encountered()
        {
            const string argumentName = "someArg";
            const string argumentValue = "some argument";
            const bool expectedArgumentValue = true;
            var testObject = new BoolArgument(_argument, _stringParser);

            bool parsedValue;
            _stringParser.Expect(s => s.TryParse(argumentValue, out parsedValue)).OutRef(expectedArgumentValue).Return(true);
            _argument.Expect(a => a.TrySetArgumentName(argumentName)).Return(SetArgumentDataResult.Success);

            var actual = testObject.TrySetArgumentNameAndValue(argumentName, argumentValue);
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.Success));
            Assert.That(testObject.ParsedSuccessfully, Is.True);
            Assert.That(testObject.ParsedArgumentValue, Is.EqualTo(expectedArgumentValue));
        }
    }
}