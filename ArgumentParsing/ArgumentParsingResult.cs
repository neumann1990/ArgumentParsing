using System.Collections.Generic;
using ArgumentParsing.Arguments;

namespace ArgumentParsing
{
    public interface IArgumentParsingResult
    {
        bool ParsingSuccessful { get; }
        IList<IArgument> Arguments { get; }
        IDictionary<string, string> UnparsableArguments { get; }
    }

    public class ArgumentParsingResult : IArgumentParsingResult
    {
        public bool ParsingSuccessful { get; }
        public IList<IArgument> Arguments { get; }
        public IDictionary<string, string> UnparsableArguments { get; }

        public ArgumentParsingResult(bool parsingSuccessful, IList<IArgument> arguments, IDictionary<string, string> unparsableArguments)
        {
            ParsingSuccessful = parsingSuccessful;
            Arguments = arguments;
            UnparsableArguments = unparsableArguments;
        }
    }
}