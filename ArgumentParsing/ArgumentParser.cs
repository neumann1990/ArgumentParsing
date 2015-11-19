using System;
using System.Collections.Generic;
using System.Linq;
using ArgumentParsing.Arguments;

namespace ArgumentParsing
{
    public interface IArgumentParser
    {
        IList<string> ArgumentDelimeters { get; set; }
        IList<string> ValueDelimeters { get; set; }
        IArgumentParsingResult Parse(string[] rawArgs, IList<IArgument> allowedArguments);
    }

    public class ArgumentParser : IArgumentParser
    {
        private readonly IArgumentValueMapper _argumentValueMapper;

        public IList<string> ArgumentDelimeters { get; set; } = new List<string> {"-", "--", "/"};
        public IList<string> ValueDelimeters { get; set; } = new List<string> {"=", ":"};

        public ArgumentParser() : this(new ArgumentValueMapper())
        {}

        public ArgumentParser(IArgumentValueMapper argumentValueMapper)
        {
            _argumentValueMapper = argumentValueMapper;
        }

        public IArgumentParsingResult Parse(string[] rawArgs, IList<IArgument> allowedArguments)
        {
            var argumentToValueMap = _argumentValueMapper.GetArgumentToValueMap(rawArgs, ArgumentDelimeters, ValueDelimeters);

            foreach (var argumentValuePair in argumentToValueMap)
            {
                var argumentNameString = argumentValuePair.Key;
                var argumentValueString = argumentValuePair.Value;
                foreach (var unparsedArgument in allowedArguments)
                {
                    if (unparsedArgument.ParsedSuccessfully) { continue; }

                    var argumentWithValue = unparsedArgument as IArgumentWithValue;
                    var result = argumentWithValue?.TrySetArgumentNameAndValue(argumentNameString, argumentValueString) 
                                    ?? unparsedArgument.TrySetArgumentName(argumentNameString);

                    if (result == SetArgumentDataResult.Success) { break; }
                }
            }

            var wasParsingSuccessful = allowedArguments.Where(a => a.IsRequired).All(a => a.ParsedSuccessfully);

            return new ArgumentParsingResult(wasParsingSuccessful, allowedArguments);
        }
    }
}
