using System.Collections.Generic;
using ArgumentParsing.Arguments;
using NUnit.Framework;
using Rhino.Mocks;

namespace ArgumentParsing.Tests
{
    [TestFixture]
    public class ArgumentParser_UT
    {
        private IArgumentValueMapper _argumentValueMapper;
        private IArgumentParser _testObject;

        [SetUp]
        public void SetUp()
        {
            _argumentValueMapper = MockRepository.GenerateMock<IArgumentValueMapper>();
            _testObject = new ArgumentParser(_argumentValueMapper);
        }

        [TearDown]
        public void TearDown()
        {
            _argumentValueMapper.VerifyAllExpectations();
        }

        [Test]
        public void ArgumentParser_Sets_Argument_Name_And_Value_When_Appropriate()
        {
            const string argumentName = "argumentName";
            const string argumentWithValueName = "SomeArgumentWithValueName";
            const string argumentValue = "some argument value";

            var argument = new Argument(new List<string> { argumentName });
            var stringArgument = new StringArgument(new List<string> { argumentWithValueName });
            var allowedArguments = new List<IArgument> {argument, stringArgument };

            var stringArgsArray = new string[]{};
            var argumentToValueMap = new Dictionary<string, string>
            {
                { argumentName, null },
                { argumentWithValueName, argumentValue}
            };
            var argumentMapResult = new ArgumentValueMapperResult { ArgumentToValueMap = argumentToValueMap};

            _argumentValueMapper.Expect(a => a.GetArgumentToValueMap(stringArgsArray, _testObject.ArgumentDelimeters, _testObject.ValueDelimeters)).Return(argumentMapResult);

            var actualResult = _testObject.Parse(stringArgsArray, allowedArguments);

            Assert.That(actualResult.ParsingSuccessful, Is.True);
            Assert.That(argument.ParsedArgumentName, Is.EqualTo(argumentName));
            Assert.That(argument.ParsedSuccessfully, Is.True);

            Assert.That(stringArgument.ParsedArgumentName, Is.EqualTo(argumentWithValueName));
            Assert.That(stringArgument.ParsedArgumentValue, Is.EqualTo(argumentValue));
            Assert.That(stringArgument.ParsedSuccessfully, Is.True);
        }

        [Test]
        public void ArgumentParser_Sets_Argument_Names_And_Values_For_Different_Argument_Types()
        {
            const string charArgumentName = "char argument name";
            const char charArgumentValue = 'A';
            const string intArgumentName = "int argument name";
            const int intArgumentValue = 49;

            var charArgument = new CharArgument(new List<string> { charArgumentName });
            var intArgument = new IntArgument(new List<string> { intArgumentName });
            var allowedArguments = new List<IArgument> { charArgument, intArgument };

            var stringArgsArray = new string[] { };
            var argumentToValueMap = new Dictionary<string, string>
            {
                { charArgumentName, charArgumentValue.ToString() },
                { intArgumentName, intArgumentValue.ToString() }
            };
            var argumentMapResult = new ArgumentValueMapperResult { ArgumentToValueMap = argumentToValueMap };
            
            _argumentValueMapper.Expect(a => a.GetArgumentToValueMap(stringArgsArray, _testObject.ArgumentDelimeters, _testObject.ValueDelimeters)).Return(argumentMapResult);

            var actualResult = _testObject.Parse(stringArgsArray, allowedArguments);

            Assert.That(actualResult.ParsingSuccessful, Is.True);

            Assert.That(charArgument.ParsedArgumentName, Is.EqualTo(charArgumentName));
            Assert.That(charArgument.ParsedArgumentValue, Is.EqualTo(charArgumentValue));
            Assert.That(charArgument.ParsedSuccessfully, Is.True);

            Assert.That(intArgument.ParsedArgumentName, Is.EqualTo(intArgumentName));
            Assert.That(intArgument.ParsedArgumentValue, Is.EqualTo(intArgumentValue));
            Assert.That(intArgument.ParsedSuccessfully, Is.True);
        }

        [Test]
        public void ArgumentParser_Sets_ParsingSuccessful_To_False_If_Required_Argument_Could_Not_Be_Parsed()
        {
            const string charArgumentName = "char argument name";
            const char charArgumentValue = 'A';
            const string intArgumentName = "int argument name";

            var charArgument = new CharArgument(new List<string> { charArgumentName });
            var intArgument = new IntArgument(new List<string> { intArgumentName });
            var allowedArguments = new List<IArgument> { charArgument, intArgument };

            var stringArgsArray = new string[] { };
            var argumentToValueMap = new Dictionary<string, string>
            {
                { charArgumentName, charArgumentValue.ToString() },
                { intArgumentName, "nonparsable int" }
            };
            var argumentMapResult = new ArgumentValueMapperResult { ArgumentToValueMap = argumentToValueMap };
            
            _argumentValueMapper.Expect(a => a.GetArgumentToValueMap(stringArgsArray, _testObject.ArgumentDelimeters, _testObject.ValueDelimeters)).Return(argumentMapResult);

            var actualResult = _testObject.Parse(stringArgsArray, allowedArguments);

            Assert.That(actualResult.ParsingSuccessful, Is.False);

            Assert.That(charArgument.ParsedArgumentName, Is.EqualTo(charArgumentName));
            Assert.That(charArgument.ParsedArgumentValue, Is.EqualTo(charArgumentValue));
            Assert.That(charArgument.ParsedSuccessfully, Is.True);

            Assert.That(intArgument.ParsedArgumentName, Is.EqualTo(intArgumentName));
            Assert.That(intArgument.ParsedArgumentValue, Is.EqualTo(0));
            Assert.That(intArgument.ParsedSuccessfully, Is.False);
        }

        [Test]
        public void ArgumentParser_Sets_ParsingSuccessful_To_True_If_Argument_Could_Not_Be_Parsed_But_Was_Not_Required_And_FailOnUnknownArgument_Set_To_False()
        {
            const string charArgumentName = "char argument name";
            const char charArgumentValue = 'A';
            const string intArgumentName = "int argument name";

            var charArgument = new CharArgument(new List<string> { charArgumentName });
            var intArgument = new IntArgument(new List<string> { intArgumentName }) {IsRequired = false};
            var allowedArguments = new List<IArgument> { charArgument, intArgument };

            var stringArgsArray = new string[] { };
            var argumentToValueMap = new Dictionary<string, string>
            {
                { charArgumentName, charArgumentValue.ToString() },
                { intArgumentName, "nonparsable int" }
            };
            var argumentMapResult = new ArgumentValueMapperResult { ArgumentToValueMap = argumentToValueMap };
            
            _argumentValueMapper.Expect(a => a.GetArgumentToValueMap(stringArgsArray, _testObject.ArgumentDelimeters, _testObject.ValueDelimeters)).Return(argumentMapResult);

            _testObject.FailOnUnknownArgument = false;
            var actualResult = _testObject.Parse(stringArgsArray, allowedArguments);

            Assert.That(actualResult.ParsingSuccessful, Is.True);

            Assert.That(charArgument.ParsedArgumentName, Is.EqualTo(charArgumentName));
            Assert.That(charArgument.ParsedArgumentValue, Is.EqualTo(charArgumentValue));
            Assert.That(charArgument.ParsedSuccessfully, Is.True);

            Assert.That(intArgument.ParsedArgumentName, Is.EqualTo(intArgumentName));
            Assert.That(intArgument.ParsedArgumentValue, Is.EqualTo(0));
            Assert.That(intArgument.ParsedSuccessfully, Is.False);
        }

        [Test]
        public void ArgumentParser_Sets_Argument_Parsing_Result_Unparsed_Values_With_Mapped_Values_That_Fail_Map_To_A_Valid_Argument()
        {
            const string charArgumentName = "char argument name";
            const char charArgumentValue = 'A';
            const string intArgumentName = "int argument name";

            var charArgument = new CharArgument(new List<string> { charArgumentName });
            var intArgument = new IntArgument(new List<string> { intArgumentName });
            var allowedArguments = new List<IArgument> { charArgument, intArgument };

            var stringArgsArray = new string[] { };
            var argumentToValueMap = new Dictionary<string, string>
            {
                { charArgumentName, charArgumentValue.ToString() },
                { intArgumentName, "nonparsable int" }
            };
            var argumentMapResult = new ArgumentValueMapperResult { ArgumentToValueMap = argumentToValueMap };

            _argumentValueMapper.Expect(a => a.GetArgumentToValueMap(stringArgsArray, _testObject.ArgumentDelimeters, _testObject.ValueDelimeters)).Return(argumentMapResult);

            var actualResult = _testObject.Parse(stringArgsArray, allowedArguments);

            Assert.That(actualResult.ParsingSuccessful, Is.False);

            Assert.That(charArgument.ParsedArgumentName, Is.EqualTo(charArgumentName));
            Assert.That(charArgument.ParsedArgumentValue, Is.EqualTo(charArgumentValue));
            Assert.That(charArgument.ParsedSuccessfully, Is.True);

            Assert.That(intArgument.ParsedArgumentName, Is.EqualTo(intArgumentName));
            Assert.That(intArgument.ParsedArgumentValue, Is.EqualTo(0));
            Assert.That(intArgument.ParsedSuccessfully, Is.False);

            Assert.That(actualResult.UnparsableArguments[intArgumentName], Is.EqualTo("nonparsable int"));
        }

        [Test]
        public void ArgumentParser_Sets_Argument_Parsing_Result_Unparsed_Values_With_Values_That_Failed_Argument_Value_Mapping()
        {
            const string charArgumentName = "char argument name";
            const char charArgumentValue = 'A';
            const string intArgumentName = "int argument name";

            var charArgument = new CharArgument(new List<string> { charArgumentName });
            var intArgument = new IntArgument(new List<string> { intArgumentName });
            var allowedArguments = new List<IArgument> { charArgument, intArgument };

            var stringArgsArray = new string[] { };
            var argumentToValueMap = new Dictionary<string, string>
            {
                { charArgumentName, charArgumentValue.ToString() },
                { intArgumentName, "nonparsable int" }
            };
            var unknownArgumentStrings = new List<string>
            {
                "someArgumentThatCouldn'tBeMapped"
            };
            var argumentMapResult = new ArgumentValueMapperResult { ArgumentToValueMap = argumentToValueMap, UnknownArgumentStrings = unknownArgumentStrings};

            _argumentValueMapper.Expect(a => a.GetArgumentToValueMap(stringArgsArray, _testObject.ArgumentDelimeters, _testObject.ValueDelimeters)).Return(argumentMapResult);

            var actualResult = _testObject.Parse(stringArgsArray, allowedArguments);

            Assert.That(actualResult.ParsingSuccessful, Is.False);

            Assert.That(charArgument.ParsedArgumentName, Is.EqualTo(charArgumentName));
            Assert.That(charArgument.ParsedArgumentValue, Is.EqualTo(charArgumentValue));
            Assert.That(charArgument.ParsedSuccessfully, Is.True);

            Assert.That(intArgument.ParsedArgumentName, Is.EqualTo(intArgumentName));
            Assert.That(intArgument.ParsedArgumentValue, Is.EqualTo(0));
            Assert.That(intArgument.ParsedSuccessfully, Is.False);

            Assert.That(actualResult.UnparsableArguments["someArgumentThatCouldn'tBeMapped"], Is.Null);
        }

        [Test]
        public void ArgumentParser_Returns_False_If_FailOnUnknownArgument_Set_To_True_And_UnknownArgument_Encountered()
        {
            const string charArgumentName = "char argument name";
            const char charArgumentValue = 'A';
            const string intArgumentName = "int argument name";

            var charArgument = new CharArgument(new List<string> { charArgumentName });
            var intArgument = new IntArgument(new List<string> { intArgumentName }) {IsRequired = false};
            var allowedArguments = new List<IArgument> { charArgument, intArgument };

            var stringArgsArray = new string[] { };
            var argumentToValueMap = new Dictionary<string, string>
            {
                { charArgumentName, charArgumentValue.ToString() },
                { intArgumentName, "nonparsable int" }
            };
            var unknownArgumentStrings = new List<string>
            {
                "someArgumentThatCouldn'tBeMapped"
            };
            var argumentMapResult = new ArgumentValueMapperResult { ArgumentToValueMap = argumentToValueMap, UnknownArgumentStrings = unknownArgumentStrings };

            _argumentValueMapper.Expect(a => a.GetArgumentToValueMap(stringArgsArray, _testObject.ArgumentDelimeters, _testObject.ValueDelimeters)).Return(argumentMapResult);

            var actualResult = _testObject.Parse(stringArgsArray, allowedArguments);

            Assert.That(actualResult.ParsingSuccessful, Is.False);

            Assert.That(charArgument.ParsedArgumentName, Is.EqualTo(charArgumentName));
            Assert.That(charArgument.ParsedArgumentValue, Is.EqualTo(charArgumentValue));
            Assert.That(charArgument.ParsedSuccessfully, Is.True);

            Assert.That(intArgument.ParsedArgumentName, Is.EqualTo(intArgumentName));
            Assert.That(intArgument.ParsedArgumentValue, Is.EqualTo(0));
            Assert.That(intArgument.ParsedSuccessfully, Is.False);

            Assert.That(actualResult.UnparsableArguments["someArgumentThatCouldn'tBeMapped"], Is.Null);
        }
    }
}
