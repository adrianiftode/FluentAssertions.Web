namespace FluentAssertions.Web.Internal
{
    internal static class ContentFormatterOptions
    {
        public const int MaximumReadableBytes = 1 * 1000 * 1024;  //2MB
        public const string WarningMessageWhenDisposed = ">>>Content is disposed so it cannot be read<<<.";
        public const string WarningMessageWhenContentIsTooLarge = ">>>Content is too large to display<<<.";
    }
}