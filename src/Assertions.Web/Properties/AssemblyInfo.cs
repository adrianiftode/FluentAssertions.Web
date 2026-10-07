using System.Runtime.CompilerServices;

// Test assemblies only. Never a shipped assembly: shared internals reach the flavours
// as compiled source (Assertions.Web.Shared), not through this (decision D2).
[assembly: InternalsVisibleTo("Assertions.Web.Tests")]
[assembly: InternalsVisibleTo("FluentAssertions.Web.Tests")]
[assembly: InternalsVisibleTo("FluentAssertions.Web.v8.Tests")]
[assembly: InternalsVisibleTo("AwesomeAssertions.Web.Tests")]
[assembly: InternalsVisibleTo("FluentAssertions.Web.FluentAssertionsWebConfig.Tests")]
[assembly: InternalsVisibleTo("AwesomeAssertions.Web.AwesomeAssertionsWebConfig.Tests")]