namespace SmartMetaData.Exceptions;

public class InvalidConfigurationException : ArgumentException
{
    public InvalidConfigurationException(string? message, string? paramName)
        : base(message, paramName)
    {
    }
}
