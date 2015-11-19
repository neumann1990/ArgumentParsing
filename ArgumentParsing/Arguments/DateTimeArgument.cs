using System;
using System.Collections.Generic;

namespace ArgumentParsing.Arguments
{
    public interface IDateTimeArgument : IArgumentWithValue
    {
        DateTime ParsedArgumentValue { get; }
    }

    public class DateTimeArgument : IDateTimeArgument
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

        public DateTime ParsedArgumentValue { get; private set; }
        public bool ParsedSuccessfully { get; set; }

        public DateTimeArgument(IList<string> possibleArgumentNames) : this(new Argument(possibleArgumentNames),  new StringParser()) { }

        public DateTimeArgument(IArgument argumentWithoutValue, IStringParser stringParser)
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

            DateTime parsedArgumentValue;
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