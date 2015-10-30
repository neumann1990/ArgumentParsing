using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgumentParsing
{
    public interface IArgument
    {
        StringComparison ArgumentNameStringComparison { get; set; }
        bool IsRequired { get; set; }
        string ParsedArgumentName { get; }
        IList<string> PossibleArgumentNames { get; set; }
        SetArgumentResult TrySetArgument(string argumentName);
    }

    public class Argument : IArgument
    {
        public bool IsRequired { get; set; }
        public IList<string> PossibleArgumentNames { get; set; }
        public StringComparison ArgumentNameStringComparison { get; set; } = StringComparison.CurrentCultureIgnoreCase;

        public string ParsedArgumentName { get; protected set; }

        public Argument(IList<string> possibleArgumentNames)
        {
            PossibleArgumentNames = possibleArgumentNames;
        }

        public virtual SetArgumentResult TrySetArgument(string argumentName)
        {
            if (PossibleArgumentNames == null) { return SetArgumentResult.InvalidArgumentName; }

            var argumentNameValid = PossibleArgumentNames.Any(possibleArgumentName => possibleArgumentName.Equals(argumentName, ArgumentNameStringComparison));

            if (!argumentNameValid) { return SetArgumentResult.InvalidArgumentName; }

            ParsedArgumentName = argumentName;
            return SetArgumentResult.Success;
        }
    }

    public enum SetArgumentResult
    {
        Success = 0,
        InvalidArgumentName = 1,
        InvalidArgumentValue = 2,
    }
}