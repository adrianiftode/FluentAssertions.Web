#if AAV
namespace AwesomeAssertions.Web.Internal;
#else
namespace FluentAssertions.Web.Internal;
#endif

internal static class ObjectExtensions
{
    public static string ToJson(this object source) => JsonSerializer.Serialize(source);
}