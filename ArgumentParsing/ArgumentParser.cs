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
        IArgumentParsingResult Parse(string[] args, IList<IArgument> allowedArguments);
    }

    public class ArgumentParser : IArgumentParser
    {
        public IList<string> ArgumentDelimeters { get; set; } = new List<string> {"-", "--", "/"};
        public IList<string> ValueDelimeters { get; set; } = new List<string> {"=", ":"};

        public IArgumentParsingResult Parse(string[] args, IList<IArgument> allowedArguments)
        {
            throw new NotImplementedException();
        }
    }
}
