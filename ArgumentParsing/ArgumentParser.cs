using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing
{
    public interface IArgumentParser
    {
        IList<string> ArgumentDelimeters { get; set; }
        IList<string> ValueDelimeters { get; set; }
        IList<Argument> AllowedArguments { get; }

        IArgumentParsingResult Parse(string[] args);
    }

    public class ArgumentParser : IArgumentParser
    {
        public IList<string> ArgumentDelimeters { get; set; } = new List<string> {"-", "--", "/"};
        public IList<string> ValueDelimeters { get; set; } = new List<string> {"=", ":"};
        public IList<Argument> AllowedArguments { get; }

        public ArgumentParser(IList<Argument> allowedArguments)
        {
            AllowedArguments = allowedArguments;
        }

        public IArgumentParsingResult Parse(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
