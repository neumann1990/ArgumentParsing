using System;
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
            var developerModeArgument = new Argument(new List<string> { "developerMode" });
            var fileTypeArgument = new CharArgument(new List<string> { "fileType", "type" });
            var fileNameArgument = new StringArgument(new List<string> { "fileName", "file" });
            var refDateArgument = new DateTimeArgument(new List<string> { "refdate" });
            var countArgument = new IntArgument(new List<string> { "argumentCount", "count" });
            var allowDuplicateArgument = new BoolArgument(new List<string> { "allowduplicatefile" })
            {
                ArgumentNameStringComparison = StringComparison.CurrentCulture, //Case-sensitive 
                IsRequired = false
            };

            var argumentParser = new ArgumentParser();
            IList<IArgument> allowedArguments = new List<IArgument>
            {
                developerModeArgument,
                fileTypeArgument,
                fileNameArgument,
                refDateArgument,
                countArgument,
                allowDuplicateArgument
            };

            var result = argumentParser.Parse(args, allowedArguments);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}
