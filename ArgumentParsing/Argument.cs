using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ArgumentParsing
{
    public abstract class Argument
    {
        public abstract bool IsRequired { get; set; }
        public abstract IList<string> PossibleArgumentNames { get; set; }
        public abstract StringComparison ArgumentNameStringComparison { get; set; }
        public abstract bool ValueRequired { get; set; }

        public abstract string ParsedArgumentName { get; protected set; }

        public abstract SetArgumentResult TrySetArgument(string argumentName);
        public abstract SetArgumentResult TrySetArgument(string argumentName, string argumentValue);
    }

    public class Argument<T> : Argument
    {
        private readonly IStringParser _stringParser;
        public override bool IsRequired { get; set; }
        public override IList<string> PossibleArgumentNames { get; set; }
        public override StringComparison ArgumentNameStringComparison { get; set; } = StringComparison.CurrentCultureIgnoreCase;
        public override bool ValueRequired { get; set; }

        public override string ParsedArgumentName { get; protected set; }
        public T ParsedArgumentValue { get; private set; }

        public Argument(IList<string> possibleArgumentNames) : this(possibleArgumentNames, new StringParser())
        {}

        public Argument(IList<string> possibleArgumentNames, IStringParser stringParser)
        {
            var valueType = typeof(T);
            if (!SupportedTypes.Contains(valueType))
            {
                throw new TypeLoadException(nameof(Argument<object>) + " created with unsupported type T:" + valueType);
            }

            PossibleArgumentNames = possibleArgumentNames;

            _stringParser = stringParser;
        }

        public override SetArgumentResult TrySetArgument(string argumentName)
        {
            return TrySetArgument(argumentName, null);
        }

        public override SetArgumentResult TrySetArgument(string argumentName, string argumentValue)
        {
            if (PossibleArgumentNames == null) { return SetArgumentResult.InvalidArgumentName; }

            var argumentNameValid = PossibleArgumentNames.Any(possibleArgumentName => possibleArgumentName.Equals(argumentName, ArgumentNameStringComparison));

            if (!argumentNameValid)
            {
                return SetArgumentResult.InvalidArgumentName;
            }

            if (!ValueRequired)
            {
                ParsedArgumentName = argumentName;
                return SetArgumentResult.Success;
            }

            T parsedArgumentValue;
            var argumentValueValid = _stringParser.TryParse(argumentValue, out parsedArgumentValue);

            if (!argumentValueValid) { return SetArgumentResult.InvalidArgumentValue; }

            ParsedArgumentName = argumentName;
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

    public enum SetArgumentResult
    {
        Success = 0,
        InvalidArgumentName = 1,
        InvalidArgumentValue = 2,
    }
}