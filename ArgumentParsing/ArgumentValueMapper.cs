using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgumentParsing
{
    public interface IArgumentValueMapper
    {
        IArgumentValueMapperResult GetArgumentToValueMap(string[] argumentValueStrings, IList<string> argumentDelimeters, IList<string> valueDelimeters);
    }

    public class ArgumentValueMapper : IArgumentValueMapper
    {
        public IArgumentValueMapperResult GetArgumentToValueMap(string[] argumentValueStrings, IList<string> argumentDelimeters, IList<string> valueDelimeters)
        {
            var result = new ArgumentValueMapperResult();
            var argumentToValueMap = result.ArgumentToValueMap;
            var orderedArgumentDelimeters = argumentDelimeters.OrderByDescending(a => a.Length);
            var orderedValueDelimeters = valueDelimeters.OrderByDescending(v => v.Length);

            string previousArgumentName = null;
            foreach (var argumentValueString in argumentValueStrings)
            {
                //Find argument name delimeter
                var argumentDelimeterLength = 0;
                foreach (var argumentDelimeter in orderedArgumentDelimeters)
                {
                    if (!argumentValueString.StartsWith(argumentDelimeter)) { continue; }

                    argumentDelimeterLength = argumentDelimeter.Length;
                    break;
                }

                //If no argument delimeter found, this may be a space delimeted argument value
                if (argumentDelimeterLength == 0 && previousArgumentName != null && argumentToValueMap[previousArgumentName] == null)
                {
                    argumentToValueMap[previousArgumentName] = argumentValueString;
                    continue;
                }

                //If no argument delimeter found and it could not be treated as a space delimeted argument value
                if (argumentDelimeterLength == 0)
                {
                    result.UnknownArgumentStrings.Add(argumentValueString);
                    continue;
                }

                //Find earliest instance of a argument value delimeter
                var firstValueDelimeterIndex = int.MaxValue;
                var valueDelimeterLength = 0;
                foreach (var valueDelimeter in orderedValueDelimeters)
                {
                    var currentValueDelimeterIndex = argumentValueString.IndexOf(valueDelimeter, StringComparison.Ordinal);
                    if (currentValueDelimeterIndex == -1 || currentValueDelimeterIndex >= firstValueDelimeterIndex)
                    {
                        continue;
                    }
                    firstValueDelimeterIndex = currentValueDelimeterIndex;
                    valueDelimeterLength = valueDelimeter.Length;
                }

                //Set argument name and value
                string argumentString;
                string valueString = null;
                if (valueDelimeterLength > 0)
                {
                    argumentString = argumentValueString.Substring(argumentDelimeterLength, firstValueDelimeterIndex - argumentDelimeterLength);
                    valueString = argumentValueString.Substring(firstValueDelimeterIndex + valueDelimeterLength);
                }
                else
                {
                    argumentString = argumentValueString.Substring(argumentDelimeterLength);
                }

                argumentToValueMap.Add(argumentString, valueString);
                previousArgumentName = argumentString;
            }

            return result;
        } 
    }
}