using System.Collections.Generic;
using ArgumentParsing.Arguments;

namespace ArgumentParsing
{
    public interface IArgumentParsingResult
    {
        bool ParsingSuccessful { get; }
        IList<IArgument> Arguments { get; }
    }

    public class ArgumentParsingResult : IArgumentParsingResult
    {
        public bool ParsingSuccessful { get; }
        public IList<IArgument> Arguments { get; }

        public ArgumentParsingResult(bool parsingSuccessful, IList<IArgument> arguments)
        {
            ParsingSuccessful = parsingSuccessful;
            Arguments = arguments;
        }
    }
}