#if AAV
using AwesomeAssertions.Primitives;
#else
using FluentAssertions.Primitives;
#endif
using System.Reflection;

#if AAV
namespace AwesomeAssertions.Web.Tests.Internal;
#else
namespace FluentAssertions.Web.Tests.Internal;
#endif

public class ApiAccessibilityTests
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
#if AAV
    public void GivenAssertionsExtensions_ThenShouldHaveAwesomeAssertionsNamespaceOnly()
#else
    public void GivenAssertionsExtensions_ThenShouldHaveFluentAssertionsNamespaceOnly()
#endif
    {
        // Arrange
        var assertionsExtensionsTypes = typeof(HttpResponseMessageAssertions).Assembly
            .GetTypes()
            .Where(c => c.Name.EndsWith("AssertionsExtensions"));
#if AAV
        var expectedNamespace = "AwesomeAssertions";  
#else
        var expectedNamespace = "FluentAssertions";
#endif

        // Assert
        using (new AssertionScope())
            foreach (var type in assertionsExtensionsTypes)
            {
                type.Namespace.Should().Be(expectedNamespace, $"because we want to make sure {{0}} has the {expectedNamespace} namespace", type.Name);
            }
    }

    [Fact]
    public void ThereShouldAlways_Be_Assertions_Extensions_Ending_With_The_AssertionsExtensions_Suffix()
    {
        // Arrange
        var assertionsExtensionsTypes = typeof(HttpResponseMessageAssertions).Assembly
            .GetTypes()
            .Where(c => c.Name.EndsWith("AssertionsExtensions"));

        // Assert
        assertionsExtensionsTypes.Should().NotBeEmpty();
    }

    [Fact]
    public void Each_HttpResponseMessage_Assertion_Should_Have_The_CustomAssertion_Attribute()
    {
        // Arrange
        var baseAssertionsType = typeof(ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>);
        var httpResponseMessageAssertions = typeof(HttpResponseMessageAssertions).Assembly
            .GetTypes()
            .Where(c => c.BaseType == baseAssertionsType)
            .SelectMany(assertionType =>
                assertionType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .ToList();

        // Assert
        httpResponseMessageAssertions
            .Should().AllSatisfy(method => method.Should().BeDecoratedWith<CustomAssertionAttribute>());
    }

    [Fact]
    public void Each_HttpResponseMessage_Assertion_Extension_Should_Be_In_The_Fluent_Assertions_Namespace_Only()
    {
        // Arrange
        var httpResponseMessageAssertionsExtensionsTypes = typeof(HttpResponseMessageAssertions).Assembly
            .GetTypes()
            .Where(c => c.Name.EndsWith("AssertionsExtensions"))
            .ToList();

#if AAV
        var expectedNamespace = "AwesomeAssertions";  
#else
        var expectedNamespace = "FluentAssertions";
#endif

        // Assert
        httpResponseMessageAssertionsExtensionsTypes
            .Should().OnlyContain(type => type.Namespace == expectedNamespace);
    }

#if !FAV8

    [Fact]
    public void Each_HttpResponseMessage_Assertion_ShouldHaveAnExtensionOverPrimitivesHttpResponseMessageAssertions()
    {
        // Arrange
        var baseAssertionsType = typeof(ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>);
        var httpResponseMessageAssertions = typeof(HttpResponseMessageAssertions).Assembly
            .GetTypes()
            .Where(c => c.BaseType == baseAssertionsType)
            .ToList();
        httpResponseMessageAssertions.Should().NotBeEmpty();

        var allExtensions = typeof(HttpResponseMessageAssertions).Assembly
            .GetTypes()
            .Where(c => c.Name.EndsWith("AssertionsExtensions"))
            .SelectMany(c => c.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .ToList();

        // Assert
        foreach (var assertionType in httpResponseMessageAssertions)
        {
            foreach (var assertionMethod in assertionType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                allExtensions.Should().Contain(extensionMethod =>
                    extensionMethod.Name == assertionMethod.Name
                    && extensionMethod.ReturnType.GetType() == assertionMethod.ReturnType.GetType()
                    && SameParameters(extensionMethod, assertionMethod),
                    "\n we wanted to check \n" +
                    "the assertion method {0} \n" +
                    "from assertion type {1} \n" +
                    "and return type {2}  \n" +
                    "and arguments {3} \n" +
                    "has a corresponding extension method",
                    assertionMethod.Name,
                    assertionType.FullName,
                    assertionMethod.ReturnType.FullName,
                    string.Join(", ", assertionMethod.GetParameters().Select(c => $"{c.Name} of type {c.ParameterType.FullName}")));
            }
        }
    }

    [Fact]
    public void Each_HttpResponseMessage_Assertion_Extension_Should_Have_The_CustomAssertion_Attribute()
    {
        // Arrange
        var allExtensions = typeof(HttpResponseMessageAssertions).Assembly
            .GetTypes()
            .Where(c => c.Name.EndsWith("AssertionsExtensions"))
            .SelectMany(c => c.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .ToList();

        // Assert
        allExtensions.Should().AllSatisfy(method => method.Should().BeDecoratedWith<CustomAssertionAttribute>());
    }

    private static bool SameParameters(MethodInfo extensionMethod, MethodInfo assertionMethod)
    {
        var extensionMethodParameters = extensionMethod.GetParameters();
        var assertionMethodParameters = assertionMethod.GetParameters();

        if (assertionMethodParameters.Length == 0)
        {
            return true;
        }

        if (assertionMethodParameters.Length == extensionMethodParameters.Length - 1) // skip this from ext
        {
            for (var i = 0; i < assertionMethodParameters.Length; i++)
            {
                if (extensionMethodParameters[i + 1].Name != assertionMethodParameters[i].Name
                    && extensionMethodParameters[i + 1].ParameterType != assertionMethodParameters[i].ParameterType)
                {
                    return false;
                }
            }
        }

        return true;
    }
#endif
}
