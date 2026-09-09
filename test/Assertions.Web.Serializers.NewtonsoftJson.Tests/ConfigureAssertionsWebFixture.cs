#if SH
[assembly: Xunit.TestFramework("Assertions.Web.Serializers.NewtonsoftJson.Tests.ConfigureAssertionsWebFixture", "Shouldly.Web.Serializers.NewtonsoftJson.Tests")]
#elif AAV
[assembly: Xunit.TestFramework("Assertions.Web.Serializers.NewtonsoftJson.Tests.ConfigureAssertionsWebFixture", "AwesomeAssertions.Web.Serializers.NewtonsoftJson.Tests")]
#else
[assembly: Xunit.TestFramework("Assertions.Web.Serializers.NewtonsoftJson.Tests.ConfigureAssertionsWebFixture", "Assertions.Web.Serializers.NewtonsoftJson.Tests")]
#endif

namespace Assertions.Web.Serializers.NewtonsoftJson.Tests;

public class ConfigureAssertionsWebFixture : XunitTestFramework
{
    public ConfigureAssertionsWebFixture(IMessageSink messageSink) : base(messageSink)
    {
        NewtonsoftJsonSerializerConfig.Options.Converters.Add(new YesNoBooleanJsonConverter());

        AssertionsWebConfig.Serializer = new NewtonsoftJsonSerializer();
    }
}
