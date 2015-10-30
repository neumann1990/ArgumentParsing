using System;
using System.Collections.Generic;

namespace ArgumentParsing
{
    public interface IArgumentWithValue<out T>
    {
        T ParsedArgumentValue { get; }
        SetArgumentResult TrySetArgument(string argumentName, string argumentValue);
    }

    public class ArgumentWithValue<T> : Argument, IArgumentWithValue<T>
    {
        private readonly IStringParser _stringParser;
        public T ParsedArgumentValue { get; private set; }

        public ArgumentWithValue(IList<string> possibleArgumentNames) : this(possibleArgumentNames, new StringParser())
        {}

        public ArgumentWithValue(IList<string> possibleArgumentNames, IStringParser stringParser) : base(possibleArgumentNames)
        {
            var argumentValueType = typeof (T);
            if (!SupportedTypes.Contains(argumentValueType))
            {
                throw new TypeLoadException(nameof(ArgumentWithValue<object>) + " created with unsupported type T:" + argumentValueType);
            }

            _stringParser = stringParser;
        }

        public SetArgumentResult TrySetArgument(string argumentName, string argumentValue)
        {
            var argumentNameParseResult = base.TrySetArgument(argumentName);

            if (argumentNameParseResult != SetArgumentResult.Success)
            {
                return argumentNameParseResult;
            }

            T parsedArgumentValue;
            var argumentValueValid = _stringParser.TryParse(argumentValue, out parsedArgumentValue);

            if (!argumentValueValid)
            {
                ParsedArgumentName = null;
                return SetArgumentResult.InvalidArgumentValue;
            }

            ParsedArgumentValue = parsedArgumentValue;
            return SetArgumentResult.Success;
        }

        public static readonly IList<Type> SupportedTypes = new List<Type>
        {
            typeof(char),
            typeof(string),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(double),
            typeof(decimal),
            typeof(bool),
        };
    }
}