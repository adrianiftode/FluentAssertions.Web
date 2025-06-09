[assembly: Xunit.TestFramework("AwesomeAssertions.Web.Serializers.NewtonsoftJson.Tests.ConfigureFluentAssertionsWebFixture", "AwesomeAssertions.Web.Serializers.NewtonsoftJson.Tests")]
namespace AwesomeAssertions.Web.Serializers.NewtonsoftJson.Tests;

public class ConfigureFluentAssertionsWebFixture : XunitTestFramework
{
    public ConfigureFluentAssertionsWebFixture(IMessageSink messageSink) : base(messageSink)
    {
        NewtonsoftJsonSerializerConfig.Options.Converters.Add(new YesNoBooleanJsonConverter());

        AwesomeAssertionsWebConfig.Serializer = new NewtonsoftJsonSerializer();
    }
}
