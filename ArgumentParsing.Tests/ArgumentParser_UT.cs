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
    public class ArgumentParser_UT
    {
        private const string _argumentName = "argumentName";
        private IArgumentParser _testObject;

        [SetUp]
        public void SetUp()
        {
            _testObject = new ArgumentParser();
        }

        [Test]
        public void ArgumentParser_Calls_TrySetArgument_On_Each_Argument_Given()
        {
            const string argumentValue = "some argument value";

            var argument = MockRepository.GenerateMock<IArgument>();
            var argumentWithValue = MockRepository.GenerateStub<IArgumentWithValue<string>>();
            var allowedArguments = new List<IArgument> {argument, (Argument)argumentWithValue};

            argument.Expect(a => a.TrySetArgument(_argumentName)).Return(SetArgumentResult.Success);
            argumentWithValue.Expect(a => a.TrySetArgument(_argumentName, argumentValue)).Return(SetArgumentResult.Success);

            _testObject.Parse(new[] {$"{_argumentName}={argumentValue}"}, allowedArguments);
        }
    }
}
