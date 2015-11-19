using ArgumentParsing.Arguments;

namespace ArgumentParsing
{
    public interface IArgumentWithValue : IArgument
    {
        SetArgumentDataResult TrySetArgumentNameAndValue(string argumentName, string argumentValue);
    }
}