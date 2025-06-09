[assembly: Xunit.TestFramework("FluentAssertions.Web.Serializers.NewtonsoftJson.Tests.ConfigureFluentAssertionsWebFixture", "FluentAssertions.Web.Serializers.NewtonsoftJson.Tests")]
namespace FluentAssertions.Web.Serializers.NewtonsoftJson.Tests;

public class ConfigureFluentAssertionsWebFixture : XunitTestFramework
{
    public ConfigureFluentAssertionsWebFixture(IMessageSink messageSink) : base(messageSink)
    {
        NewtonsoftJsonSerializerConfig.Options.Converters.Add(new YesNoBooleanJsonConverter());

        FluentAssertionsWebConfig.Serializer = new NewtonsoftJsonSerializer();
    }
}
