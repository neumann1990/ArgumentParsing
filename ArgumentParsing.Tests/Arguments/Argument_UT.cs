using System;
using System.Collections.Generic;
using ArgumentParsing.Arguments;
using NUnit.Framework;

namespace ArgumentParsing.Tests.Arguments
{
    [TestFixture]
    public class Argument_UT
    {
        [Test]
        public void TrySetArgument_Returns_InvalidArgumentName_When_Argument_Name_Is_Incorrect()
        {
            var testObject = new Argument(null);
            var actual = testObject.TrySetArgumentName("some arg");
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
        }

        [Test]
        public void TrySetArgument_Returns_Success_When_Argument_Name_Is_Correct_And_No_Value_Required()
        {
            const string argumentName = "someArg";
            var testObject = new Argument(new List<string> {argumentName});

            var actual = testObject.TrySetArgumentName(argumentName);
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.Success));
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(argumentName));
        }

        [Test]
        public void TrySetArgument_Returns_Success_When_Argument_Name_Matches_Any_Of_The_Possible_Values()
        {
            const string possibleArgumentName1 = "someArg";
            const string possibleArgumentName2 = "someArg2";
            var testObject = new Argument(new List<string> { possibleArgumentName1, possibleArgumentName2 });

            var actual = testObject.TrySetArgumentName(possibleArgumentName1);
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.Success));
            Assert.That(testObject.ParsedSuccessfully, Is.True);
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(possibleArgumentName1));

            actual = testObject.TrySetArgumentName(possibleArgumentName2);
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.Success));
            Assert.That(testObject.ParsedSuccessfully, Is.True);
            Assert.That(testObject.ParsedArgumentName, Is.EqualTo(possibleArgumentName2));
        }

        [Test]
        public void TrySetArgument_Returns_InvalidArgumentName_When_Argument_Name_Does_Not_Match_Any_Of_The_Possible_Values()
        {
            const string possibleArgumentName1 = "someArg";
            const string possibleArgumentName2 = "someArg2";
            var testObject = new Argument(new List<string> { possibleArgumentName1, possibleArgumentName2 });

            var actual = testObject.TrySetArgumentName("someInvalidArg");
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);

            actual = testObject.TrySetArgumentName("someArg3");
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.InvalidArgumentName));
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


            var actual = testObject.TrySetArgumentName(argumentName.ToLower());
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.Success));

            testObject = new Argument(new List<string> { argumentName })
            {
                ArgumentNameStringComparison = StringComparison.CurrentCulture
            };

            actual = testObject.TrySetArgumentName(argumentName.ToLower());
            Assert.That(actual, Is.EqualTo(SetArgumentDataResult.InvalidArgumentName));
            Assert.That(testObject.ParsedArgumentName, Is.Null);
        }
    }
}
