using Microsoft.Extensions.Options;

namespace SmartMetaData.Options;

public class Options<T> : IOptions<T>
    where T : class
{
    public Options(T value) => Value = value;
    public T Value { get; }
}
