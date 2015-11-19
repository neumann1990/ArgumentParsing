using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ArgumentParsing.Tests
{
    [TestFixture]
    public class ArgumentValueMapper_UT
    {
        private readonly IList<string> _argumentDelimeters = new List<string> {"-", "/", "--"};
        private readonly IList<string> _valueDelimeters = new List<string> {"=", ":"};
        private IArgumentValueMapper _testObject;

        [SetUp]
        public void SetUp()
        {
            _testObject = new ArgumentValueMapper();
        }

        [Test]
        public void GetArgumentToValueMap_Maps_Single_Argument_Without_Value()
        {
            const string argumentName = "someArgumentName";

            var stringArguments = new[] { $"-{argumentName}" };
            var result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            var actualArgumentToValueMap = result.ArgumentToValueMap;

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName], Is.Null);
            Assert.That(actualArgumentToValueMap.Count, Is.EqualTo(1));

            stringArguments = new[] { $"/{argumentName}" };
            result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            actualArgumentToValueMap = result.ArgumentToValueMap;

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName], Is.Null);
            Assert.That(actualArgumentToValueMap.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetArgumentToValueMap_Uses_Longest_Delimeter_When_Multiple_Delimeters_Match()
        {
            const string argumentName = "someArgumentName";

            var stringArguments = new[] { $"--{argumentName}" };
            var result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            var actualArgumentToValueMap = result.ArgumentToValueMap;

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName], Is.Null);

            Assert.That(actualArgumentToValueMap.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetArgumentToValueMap_Maps_Arguments_Without_Values()
        {
            const string argumentName1 = "someArgumentName1";
            const string argumentName2 = "someArgumentName2";
            const string argumentName3 = "someArgumentName3";

            var stringArguments = new[] {$"-{argumentName1}", $"/{argumentName2}", $"--{argumentName3}" };
            var result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            var actualArgumentToValueMap = result.ArgumentToValueMap;

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName1), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName1], Is.Null);

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName2), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName2], Is.Null);

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName3), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName2], Is.Null);

            Assert.That(actualArgumentToValueMap.Count, Is.EqualTo(3));
        }

        [Test]
        public void GetArgumentToValueMap_Maps_Argument_Name_To_Value()
        {
            const string argumentName = "someArgumentName";
            const string argumentValue = "someArgumentValue";

            var stringArguments = new[] { $"--{argumentName}={argumentValue}" };
            var result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            var actualArgumentToValueMap = result.ArgumentToValueMap;

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName], Is.EqualTo(argumentValue));

            Assert.That(actualArgumentToValueMap.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetArgumentToValueMap_Maps_Argument_Name_To_Value_When_Value_Contains_An_Argument_Delimeter()
        {
            const string argumentName = "filename";
            const string argumentValue = "c:/temp";

            var stringArguments = new[] { $"--{argumentName}={argumentValue}" };
            var result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            var actualArgumentToValueMap = result.ArgumentToValueMap;

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName], Is.EqualTo(argumentValue));

            Assert.That(actualArgumentToValueMap.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetArgumentToValueMap_Maps_Argument_Name_To_Value_When_Value_Is_The_Following_Array_Entry()
        {
            const string argumentName = "filename";
            const string argumentValue = "c:/temp";

            var stringArguments = new[] { $"--{argumentName}", $"{argumentValue}" };
            var result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            var actualArgumentToValueMap = result.ArgumentToValueMap;

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName], Is.EqualTo(argumentValue));

            Assert.That(actualArgumentToValueMap.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetArgumentToValueMap_Adds_String_To_UnknownArgumentStrings_Collection_If_No_Argument_Delimeter_Found_In_String_And_Is_Not_Value()
        {
            var stringArguments = new[] { "someArgumentName" };
            var result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            
            Assert.That(result.ArgumentToValueMap, Is.Empty);
            CollectionAssert.AreEqual(result.UnknownArgumentStrings, stringArguments.ToList());

            stringArguments = new[] { "_someArgumentName", "-validArgument", "--validStringArgument=value" };
            result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);

            Assert.That(result.ArgumentToValueMap["validArgument"], Is.Null);
            Assert.That(result.ArgumentToValueMap["validStringArgument"], Is.EqualTo("value"));
            Assert.That(result.ArgumentToValueMap.Count, Is.EqualTo(2));
            Assert.That(result.UnknownArgumentStrings.Contains("_someArgumentName"), Is.True);
            Assert.That(result.UnknownArgumentStrings.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetArgumentToValueMap_Handles_Mixture_Of_Argument_Value_Combinations()
        {
            const string argumentName1 = "filename";
            const string argumentValue1 = "c:/temp";

            const string argumentName2 = "date";
            const string argumentValue2 = "10/11/12";

            const string argumentName3 = "developermode";
            const string argumentValue3 = "true";

            const string argumentName4 = "help";

            var stringArguments = new[] 
            {
                $"--{argumentName1}",
                $"{argumentValue1}",
                $"-{argumentName2}={argumentValue2}",
                $"/{argumentName3}:{argumentValue3}",
                $"--{argumentName4}"
            };
            var result = _testObject.GetArgumentToValueMap(stringArguments, _argumentDelimeters, _valueDelimeters);
            var actualArgumentToValueMap = result.ArgumentToValueMap;

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName1), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName1], Is.EqualTo(argumentValue1));

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName2), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName2], Is.EqualTo(argumentValue2));

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName3), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName3], Is.EqualTo(argumentValue3));

            Assert.That(actualArgumentToValueMap.ContainsKey(argumentName4), Is.True);
            Assert.That(actualArgumentToValueMap[argumentName4], Is.Null);

            Assert.That(actualArgumentToValueMap.Count, Is.EqualTo(4));
        }
    }
}