#if AAV
namespace AwesomeAssertions.Web.Tests.TestModels;
#else
namespace FluentAssertions.Web.Tests.TestModels;
#endif

internal class TestModel
{
    public string? Property { get; set; }
}

internal class TestModelWithEnum
{
    public TestEnum TestEnum { get; set; }
}