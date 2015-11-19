using System;
using System.Collections.Generic;

namespace ArgumentParsing.Arguments
{
    public interface ICharArgument : IArgumentWithValue
    {
        char ParsedArgumentValue { get; }
    }

    public class CharArgument : ICharArgument
    {
        private readonly IArgument _argument;
        private readonly IStringParser _stringParser;

        public bool IsRequired { get; set; } = true;

        public StringComparison ArgumentNameStringComparison
        {
            get { return _argument.ArgumentNameStringComparison; }
            set { _argument.ArgumentNameStringComparison = value; }
        }

        public IList<string> PossibleArgumentNames => _argument.PossibleArgumentNames;
        public string ParsedArgumentName => _argument.ParsedArgumentName;

        public char ParsedArgumentValue { get; private set; }
        public bool ParsedSuccessfully { get; set; }

        public CharArgument(string possibleArgumentName) : this(new List<string> { possibleArgumentName }) { }

        public CharArgument(IList<string> possibleArgumentNames) : this(new Argument(possibleArgumentNames),  new StringParser()) { }

        public CharArgument(IArgument argumentWithoutValue, IStringParser stringParser)
        {
            _argument = argumentWithoutValue;
            _stringParser = stringParser;
        }

        public SetArgumentDataResult TrySetArgumentName(string argumentName)
        {
            return TrySetArgumentNameAndValue(argumentName, null);
        }

        public SetArgumentDataResult TrySetArgumentNameAndValue(string argumentName, string argumentValue)
        {
            var argumentNameParseResult = _argument.TrySetArgumentName(argumentName);

            if (argumentNameParseResult != SetArgumentDataResult.Success)
            {
                return argumentNameParseResult;
            }

            char parsedArgumentValue;
            var isArgumentValueValid = _stringParser.TryParse(argumentValue, out parsedArgumentValue);

            if (!isArgumentValueValid)
            {
                return SetArgumentDataResult.InvalidArgumentValue;
            }

            ParsedArgumentValue = parsedArgumentValue;
            ParsedSuccessfully = true;
            return SetArgumentDataResult.Success;
        }
    }
}
