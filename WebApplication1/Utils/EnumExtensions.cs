using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace FaturaTakip.Utils;

public static class EnumExtensions
{
    // Note that we never need to expire these cache items, so we just use ConcurrentDictionary rather than MemoryCache
    private static readonly ConcurrentDictionary<string, string> DisplayNameCache = new ConcurrentDictionary<string, string>();

    // Description Attribute
    public static string DisplayName(this Enum value)
    {
        var key = $"{value.GetType().FullName}.{value}";

        var displayName = DisplayNameCache.GetOrAdd(key, x =>
        {
            var name = (DescriptionAttribute[])value
                .GetType()
                .GetTypeInfo()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return name.Length > 0 ? name[0].Description : value.ToString();
        });

        return displayName;
    }

    // Display Attribute
    public static string GetDescriptionOfEnum(Enum value)
    {
        var type = value.GetType();
        if (!type.IsEnum) throw new ArgumentException(String.Format("Type '{0}' is not Enum", type));

        var members = type.GetMember(value.ToString());
        if (members.Length == 0) throw new ArgumentException(String.Format("Member '{0}' not found in type '{1}'", value, type.Name));

        var member = members[0];
        var attributes = member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);
        if (attributes.Length == 0) throw new ArgumentException(String.Format("'{0}.{1}' doesn't have DisplayAttribute", type.Name, value));

        var attribute = (System.ComponentModel.DataAnnotations.DisplayAttribute)attributes[0];
        return attribute.Name;
    }
}