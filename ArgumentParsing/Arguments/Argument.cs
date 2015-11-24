using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgumentParsing.Arguments
{
    public interface IArgument
    {
        bool IsRequired { get; set; }
        StringComparison ArgumentNameStringComparison { get; set; }
        string ParsedArgumentName { get; }
        IList<string> PossibleArgumentNames { get; }
        string UsageDescription { get; set; }
        SetArgumentDataResult TrySetArgumentName(string argumentName);
        bool ParsedSuccessfully { get; }
    }

    public class Argument : IArgument
    {
        public const string DefaultUsageDescription = "<No description provided>";

        public bool IsRequired { get; set; } = true;
        public IList<string> PossibleArgumentNames { get; }
        public string UsageDescription { get; set; } = DefaultUsageDescription;
        public StringComparison ArgumentNameStringComparison { get; set; } = StringComparison.CurrentCultureIgnoreCase;

        public string ParsedArgumentName { get; protected set; }
        public bool ParsedSuccessfully { get; protected set; }

        public Argument(string possibleArgumentName) : this(new List<string> { possibleArgumentName }) { }

        public Argument(IList<string> possibleArgumentNames)
        {
            PossibleArgumentNames = possibleArgumentNames;
        }

        public virtual SetArgumentDataResult TrySetArgumentName(string argumentName)
        {
            if (PossibleArgumentNames == null) { return SetArgumentDataResult.InvalidArgumentName; }

            var argumentNameValid = PossibleArgumentNames.Any(possibleArgumentName => possibleArgumentName.Equals(argumentName, ArgumentNameStringComparison));

            if (!argumentNameValid) { return SetArgumentDataResult.InvalidArgumentName; }

            ParsedArgumentName = argumentName;
            ParsedSuccessfully = true;
            return SetArgumentDataResult.Success;
        }
    }
}