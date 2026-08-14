[assembly: Xunit.TestFramework("Shouldly.Web.Serializers.NewtonsoftJson.Tests.ConfigureShouldlyWebFixture", "Shouldly.Web.Serializers.NewtonsoftJson.Tests")]
namespace Shouldly.Web.Serializers.NewtonsoftJson.Tests;

public class ConfigureShouldlyWebFixture : XunitTestFramework
{
    public ConfigureShouldlyWebFixture(IMessageSink messageSink) : base(messageSink)
    {
        NewtonsoftJsonSerializerConfig.Options.Converters.Add(new YesNoBooleanJsonConverter());

        ShouldlyWebConfig.Serializer = new NewtonsoftJsonSerializer();
    }
}
