using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace ArgumentParsing.Tests
{
    [TestFixture]
    public class Argument_UT
    {
        [Test]
        public void TrySetArgument_Returns_InvalidArgumentName_When_Argument_Name_Is_Incorrect()
        {
            var testObject = new Argument(null);
            var actual = testObject.TrySetArgument("some arg");
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
        }

        [Test]
        public void TrySetArgument_Returns_Success_When_Argument_Name_Is_Correct_And_No_Value_Required()
        {
            const string argumentName = "someArg";
            var testObject = new Argument(new List<string> {argumentName});

            var actual = testObject.TrySetArgument(argumentName);
            Assert.That(actual, Is.EqualTo(SetArgumentResult.Success));
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(argumentName));
        }

        [Test]
        public void TrySetArgument_Returns_Success_When_Argument_Name_Matches_Any_Of_The_Possible_Values()
        {
            const string possibleArgumentName1 = "someArg";
            const string possibleArgumentName2 = "someArg2";
            var testObject = new Argument(new List<string> { possibleArgumentName1, possibleArgumentName2 });

            var actual = testObject.TrySetArgument(possibleArgumentName1);
            Assert.That(actual, Is.EqualTo(SetArgumentResult.Success));
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(possibleArgumentName1));

            actual = testObject.TrySetArgument(possibleArgumentName2);
            Assert.That(actual, Is.EqualTo(SetArgumentResult.Success));
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(possibleArgumentName2));
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentName_When_Argument_Name_Does_Not_Match_Any_Of_The_Possible_Values()
        {
            const string possibleArgumentName1 = "someArg";
            const string possibleArgumentName2 = "someArg2";
            var testObject = new Argument(new List<string> { possibleArgumentName1, possibleArgumentName2 });

            var actual = testObject.TrySetArgument("someInvalidArg");
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);

            actual = testObject.TrySetArgument("someArg3");
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
        }

        [Test]
        public void TrySetArgument_Ignores_Case_Based_On_StringComparison_Property()
        {
            const string argumentName = "someArg";
            var testObject = new Argument(new List<string> { argumentName })
            {
                ArgumentNameStringComparison = StringComparison.CurrentCultureIgnoreCase
            };


            var actual = testObject.TrySetArgument(argumentName.ToLower());
            Assert.That(actual, Is.EqualTo(SetArgumentResult.Success));

            testObject = new Argument(new List<string> { argumentName })
            {
                ArgumentNameStringComparison = StringComparison.CurrentCulture
            };

            actual = testObject.TrySetArgument(argumentName.ToLower());
            Assert.That(actual, Is.EqualTo(SetArgumentResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
        }
    }
}
