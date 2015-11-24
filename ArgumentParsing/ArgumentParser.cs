using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArgumentParsing.Arguments;

namespace ArgumentParsing
{
    public interface IArgumentParser
    {
        IList<string> ArgumentDelimeters { get; set; }
        IList<string> ValueDelimeters { get; set; }
        bool FailOnUnknownArgument { get; set; }
        IArgumentParsingResult Parse(string[] rawArgs, IList<IArgument> allowedArguments);
        string GetUsage(string programDescription, IList<IArgument> allowedArguments);
    }

    public class ArgumentParser : IArgumentParser
    {
        private readonly IArgumentValueMapper _argumentValueMapper;

        public IList<string> ArgumentDelimeters { get; set; } = new List<string> {"-", "--", "/"};
        public IList<string> ValueDelimeters { get; set; } = new List<string> {"=", ":"};
        public bool FailOnUnknownArgument { get; set; } = true;

        public ArgumentParser() : this(new ArgumentValueMapper())
        {}

        public ArgumentParser(IArgumentValueMapper argumentValueMapper)
        {
            _argumentValueMapper = argumentValueMapper;
        }

        public IArgumentParsingResult Parse(string[] rawArgs, IList<IArgument> allowedArguments)
        {
            var mapperResult = _argumentValueMapper.GetArgumentToValueMap(rawArgs, ArgumentDelimeters, ValueDelimeters);
            var argumentToValueMap = mapperResult.ArgumentToValueMap;
            var unparsableArguments = new Dictionary<string, string>();
            mapperResult.UnknownArgumentStrings.ForEach(u => unparsableArguments.Add(u, null));

            foreach (var argumentValuePair in argumentToValueMap)
            {
                var argumentParsedSuccessfully = false;

                var argumentNameString = argumentValuePair.Key;
                var argumentValueString = argumentValuePair.Value;
                foreach (var unparsedArgument in allowedArguments)
                {
                    if (unparsedArgument.ParsedSuccessfully) { continue; }

                    var argumentWithValue = unparsedArgument as IArgumentWithValue;
                    var setArgumentResult = argumentWithValue?.TrySetArgumentNameAndValue(argumentNameString, argumentValueString) 
                                            ?? unparsedArgument.TrySetArgumentName(argumentNameString);

                    if (setArgumentResult != SetArgumentDataResult.Success) { continue; }

                    argumentParsedSuccessfully = true;
                    break;
                }

                if (!argumentParsedSuccessfully)
                {
                    unparsableArguments.Add(argumentValuePair.Key, argumentValuePair.Value);
                }
            }

            var areAllRequiredArgumentsSet = allowedArguments.Where(a => a.IsRequired).All(a => a.ParsedSuccessfully);
            var unknownArgumentFailureOccured = FailOnUnknownArgument && unparsableArguments.Any();
            var wasParsingSuccessful = areAllRequiredArgumentsSet && !unknownArgumentFailureOccured;

            return new ArgumentParsingResult(wasParsingSuccessful, allowedArguments, unparsableArguments);
        }

        public string GetUsage(string programDescription, IList<IArgument> allowedArguments)
        {
            const string usageNameDelimeter = ", ";
            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(programDescription))
            {
                stringBuilder.Append($"\r\nPROGRAM DESCRIPTION\r\n\t{programDescription}");
            }

            stringBuilder.Append($"\r\nARGUMENTS\r\n");

            foreach (var allowedArgument in allowedArguments)
            {
                stringBuilder.Append("\t");
                foreach (var argumentName in allowedArgument.PossibleArgumentNames)
                {
                    stringBuilder.Append($"{ArgumentDelimeters.First()}{argumentName}{usageNameDelimeter}");
                }
                
                //Remove trailing usage delimeters
                stringBuilder.Replace(usageNameDelimeter, string.Empty, stringBuilder.Length - usageNameDelimeter.Length, usageNameDelimeter.Length);

                //Add whether or not argument is optional
                var requiredValue = allowedArgument.IsRequired ? string.Empty : "(optional)";
                stringBuilder.Append($"\t{requiredValue}");

                //Add what type of argument it is
                if (allowedArgument is IArgumentWithValue)
                {
                    var argumentType = allowedArgument.GetType().GetProperty(nameof(IBoolArgument.ParsedArgumentValue)).PropertyType;
                    stringBuilder.Append($"\t[{argumentType.ToString().Replace("System.", string.Empty)}]");
                }

                //Add usage description for argument
                stringBuilder.Append($"\t{allowedArgument.UsageDescription}\r\n");
            }

            return stringBuilder.ToString();
        }
    }
}
