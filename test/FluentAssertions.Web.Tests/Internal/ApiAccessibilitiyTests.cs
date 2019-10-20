using FluentAssertions.Execution;
using System.Linq;
using Xunit;

namespace FluentAssertions.Web.Tests.Internal
{
    public class ApiAccessibilitiyTests
    {
        [Fact]
        public void GivenTypesFromTheInternalNamespace_ThenShouldBeInternalOrLessThanInternal()
        {
            // Arrange
            var typesFromInternal = typeof(HttpResponseMessageAssertions).Assembly
                .GetTypes()
                .Where(c => c.Namespace.Contains("Internal"));

            // Assert
            using (new AssertionScope())
                foreach (var type in typesFromInternal)
                {
                    type.Should().NotHaveAccessModifier(Common.CSharpAccessModifier.Public);
                }
        }
    }
}
