namespace Assertions.Web.Internal;

internal static class ObjectExtensions
{
    public static string ToJson(this object source) => JsonSerializer.Serialize(source);
}