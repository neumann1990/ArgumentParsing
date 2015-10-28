using System;
using System.ComponentModel;

namespace ArgumentParsing
{
    public interface IStringParser
    {
        bool TryParse<T>(string argumentValue, out T parsedArgumentValue);
        bool TryParse(string argumentValue, out DateTime parsedArgumentValue);
    }

    public class StringParser : IStringParser
    {
        public bool TryParse<T>(string argumentValue, out T parsedArgumentValue)
        {
            parsedArgumentValue = default(T);

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                parsedArgumentValue = (T)converter.ConvertFromString(argumentValue);
                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public bool TryParse(string argumentValue, out DateTime parsedArgumentValue)
        {
            return DateTime.TryParse(argumentValue, out parsedArgumentValue);
        }
    }
}
