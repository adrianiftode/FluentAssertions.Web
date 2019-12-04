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
                .Where(c => c.Namespace?.Contains("Internal") == true);

            // Assert
            using (new AssertionScope())
                foreach (var type in typesFromInternal)
                {
                    type.Should().NotHaveAccessModifier(Common.CSharpAccessModifier.Public);
                }
        }

        [Fact]
        public void GivenAssertionsExtensions_ThenShouldHaveFluentAssertionsNamespaceOnly()
        {
            // Arrange
            var typesFromInternal = typeof(HttpResponseMessageAssertions).Assembly
                .GetTypes()
                .Where(c => c.Name.EndsWith("AssertionsExtensions"));

            // Assert
            using (new AssertionScope())
                foreach (var type in typesFromInternal)
                {
                    type.Namespace.Should().Be("FluentAssertions", "because we want to make sure {0} has the FluentAssertions namespace", type.Name);
                }
        }
    }
}
