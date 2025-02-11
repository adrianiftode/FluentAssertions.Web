namespace Ited.HttpFormatter;

public static class ObjectExtensions
{
    public static string ToJson(this object source) => JsonSerializer.Serialize(source);
}