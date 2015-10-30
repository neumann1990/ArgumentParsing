using System.Collections.Generic;

namespace ArgumentParsing
{
    public interface IArgumentParsingResult
    {
        bool ParsingSuccessful { get; }
        IList<IArgument> SuccessfullyParsedArguments { get; }
        IList<IArgument> InvalidArguments { get; }
    }

    public class ArgumentParsingResult : IArgumentParsingResult
    {
        public bool ParsingSuccessful { get; }
        public IList<IArgument> SuccessfullyParsedArguments { get; }
        public IList<IArgument> InvalidArguments { get; }

        public ArgumentParsingResult(bool parsingSuccessful, IList<IArgument> successfullyParsedArguments, IList<IArgument> invalidArguments)
        {
            ParsingSuccessful = parsingSuccessful;
            SuccessfullyParsedArguments = successfullyParsedArguments;
            InvalidArguments = invalidArguments;
        }
    }
}