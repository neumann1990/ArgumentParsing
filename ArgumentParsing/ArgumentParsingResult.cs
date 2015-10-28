using System.Collections.Generic;

namespace ArgumentParsing
{
    public interface IArgumentParsingResult
    {
        bool ParsingSuccessful { get; }
        IList<Argument> SuccessfullyParsedArguments { get; }
        IList<Argument> InvalidArguments { get; }
    }

    public class ArgumentParsingResult : IArgumentParsingResult
    {
        public bool ParsingSuccessful { get; }
        public IList<Argument> SuccessfullyParsedArguments { get; }
        public IList<Argument> InvalidArguments { get; }

        public ArgumentParsingResult(bool parsingSuccessful, IList<Argument> successfullyParsedArguments, IList<Argument> invalidArguments)
        {
            ParsingSuccessful = parsingSuccessful;
            SuccessfullyParsedArguments = successfullyParsedArguments;
            InvalidArguments = invalidArguments;
        }
    }
}