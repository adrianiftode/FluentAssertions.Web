#if SH
namespace Shouldly.Web.Tests.TestModels;
#elif AAV
namespace AwesomeAssertions.Web.Tests.TestModels;
#else
namespace FluentAssertions.Web.Tests.TestModels;
#endif

internal enum TestEnum
{
    Type1 = 2,
    Type2 = 4
}
