using System.Collections.Generic;

namespace ArgumentParsing
{
    public interface IArgumentValueMapperResult
    {
        IDictionary<string, string> ArgumentToValueMap { get; set; }
        List<string> UnknownArgumentStrings { get; set; }
    }

    public class ArgumentValueMapperResult : IArgumentValueMapperResult
    {
        public IDictionary<string, string> ArgumentToValueMap { get; set; } = new Dictionary<string, string>(); 
        public List<string> UnknownArgumentStrings { get; set; } = new List<string>();
    }
}