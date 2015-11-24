﻿using System;
using System.Collections.Generic;
using ArgumentParsing;
using ArgumentParsing.Arguments;
using Newtonsoft.Json;

namespace TestConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var helpArgument = new Argument(new List<string> { "help", "h", "?" }) {IsRequired = false};
            var fileTypeArgument = new CharArgument(new List<string> { "fileType", "type" });
            var fileNameArgument = new StringArgument(new List<string> { "fileName", "file" });
            var refDateArgument = new DateTimeArgument("refdate") {IsRequired = false};
            var countArgument = new IntArgument(new List<string> { "argumentCount", "count" }) {IsRequired = false};
            var allowDuplicateArgument = new BoolArgument("allowduplicatefile")
            {
                ArgumentNameStringComparison = StringComparison.CurrentCulture, //Case-sensitive 
                IsRequired = false,
                UsageDescription = "Indicates whether or not to allow duplicate files"
            };

            var argumentParser = new ArgumentParser();
            IList<IArgument> allowedArguments = new List<IArgument>
            {
                helpArgument,
                fileTypeArgument,
                fileNameArgument,
                refDateArgument,
                countArgument,
                allowDuplicateArgument
            };

            var result = argumentParser.Parse(args, allowedArguments);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            if (!helpArgument.ParsedSuccessfully) return;

            var usage = argumentParser.GetUsage("Test program", allowedArguments);
            Console.WriteLine(usage);
        }
    }
}