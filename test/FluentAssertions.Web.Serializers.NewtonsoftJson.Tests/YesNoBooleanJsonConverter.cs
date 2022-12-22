using Newtonsoft.Json;

namespace FluentAssertions.Web.Serializers.NewtonsoftJson.Tests;

public class YesNoBooleanJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(bool);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var value = reader.Value.ToString();

        if (value != null && value.Equals("yes", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (value != null && value.Equals("no", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return new JsonSerializer().Deserialize(reader, objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
