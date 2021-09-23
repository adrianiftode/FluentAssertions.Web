namespace FluentAssertions.Web.Tests.TestModels
{
    internal class TestModel
    {
        public string? Property { get; set; }
    }

    internal class TestModelWithEnum
    {
        public TestEnum TestEnum { get; set; }
    }
}