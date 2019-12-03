namespace FluentAssertions.Web.Internal
{
    internal static class ContentFormatterOptions
    {
        private const int KilobytesInMegabyte = 1024;
        public const int MaximumReadableBytes = (int)(0.25 * 1000) * KilobytesInMegabyte;
        public const int MaximumPrintableBytes = 10 * KilobytesInMegabyte;
        public const string WarningMessageWhenDisposed = "***** Content is disposed so it cannot be read. *****";
        public const string WarningMessageWhenContentIsTooLarge = "***** Content is too large to display and only a part of it it is printed. *****";
        public const string ContentIsSomeTypeHavingLength = "***** Content is of a {0} type having the length {1}. *****";
    }
}