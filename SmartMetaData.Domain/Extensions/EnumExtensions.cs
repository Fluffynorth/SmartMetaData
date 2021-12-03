using System.ComponentModel;

namespace SmartMetaData.Domain.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        var enumType = enumValue.GetType();
        var enumField = Enum.GetName(enumType, enumValue);
        var descriptionAttribute = enumType.GetField(enumField!)?.GetAttribute<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? enumValue.ToString();
    }
}
