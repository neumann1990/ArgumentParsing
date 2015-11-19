using System.Collections.Generic;
using System.Linq;
using ArgumentParsing.Arguments;

namespace ArgumentParsing
{
    public interface IArgumentParser
    {
        IList<string> ArgumentDelimeters { get; set; }
        IList<string> ValueDelimeters { get; set; }
        bool FailOnUnknownArgument { get; set; }
        IArgumentParsingResult Parse(string[] rawArgs, IList<IArgument> allowedArguments);
    }

    public class ArgumentParser : IArgumentParser
    {
        private readonly IArgumentValueMapper _argumentValueMapper;

        public IList<string> ArgumentDelimeters { get; set; } = new List<string> {"-", "--", "/"};
        public IList<string> ValueDelimeters { get; set; } = new List<string> {"=", ":"};
        public bool FailOnUnknownArgument { get; set; } = true;

        public ArgumentParser() : this(new ArgumentValueMapper())
        {}

        public ArgumentParser(IArgumentValueMapper argumentValueMapper)
        {
            _argumentValueMapper = argumentValueMapper;
        }

        public IArgumentParsingResult Parse(string[] rawArgs, IList<IArgument> allowedArguments)
        {
            var mapperResult = _argumentValueMapper.GetArgumentToValueMap(rawArgs, ArgumentDelimeters, ValueDelimeters);
            var argumentToValueMap = mapperResult.ArgumentToValueMap;
            var unparsableArguments = new Dictionary<string, string>();
            mapperResult.UnknownArgumentStrings.ForEach(u => unparsableArguments.Add(u, null));

            foreach (var argumentValuePair in argumentToValueMap)
            {
                var argumentParsedSuccessfully = false;

                var argumentNameString = argumentValuePair.Key;
                var argumentValueString = argumentValuePair.Value;
                foreach (var unparsedArgument in allowedArguments)
                {
                    if (unparsedArgument.ParsedSuccessfully) { continue; }

                    var argumentWithValue = unparsedArgument as IArgumentWithValue;
                    var setArgumentResult = argumentWithValue?.TrySetArgumentNameAndValue(argumentNameString, argumentValueString) 
                                            ?? unparsedArgument.TrySetArgumentName(argumentNameString);

                    if (setArgumentResult != SetArgumentDataResult.Success) { continue; }

                    argumentParsedSuccessfully = true;
                    break;
                }

                if (!argumentParsedSuccessfully)
                {
                    unparsableArguments.Add(argumentValuePair.Key, argumentValuePair.Value);
                }
            }

            var areAllRequiredArgumentsSet = allowedArguments.Where(a => a.IsRequired).All(a => a.ParsedSuccessfully);
            var unknownArgumentFailureOccured = FailOnUnknownArgument && unparsableArguments.Any();
            var wasParsingSuccessful = areAllRequiredArgumentsSet && !unknownArgumentFailureOccured;

            return new ArgumentParsingResult(wasParsingSuccessful, allowedArguments, unparsableArguments);
        }
    }
}
