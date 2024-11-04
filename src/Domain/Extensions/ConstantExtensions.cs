using System.Reflection;

namespace Clean_Architecture.Domain.Extensions;

public static class ConstantExtensions
{
    public static IEnumerable<string> GetAllConstants(this Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(fi => (string)fi.GetRawConstantValue()!);
    }
}
