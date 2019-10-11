using Newtonsoft.Json;

namespace FluentAssertions.Web.Internal
{
    internal static class ObjectExtensions
    {
        public static string ToJson(this object source) => JsonConvert.SerializeObject(source);
    }
}