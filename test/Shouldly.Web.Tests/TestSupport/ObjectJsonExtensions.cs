namespace Shouldly.Web.Tests.TestSupport;

/// <summary>
/// Mirrors the internal <c>FluentAssertions.Web.Internal.ObjectExtensions.ToJson</c> that the shared
/// spec files use in their Arrange sections; internals of the referenced libraries are not accessible
/// from this assembly.
/// </summary>
public static class ObjectJsonExtensions
{
    public static string ToJson(this object source) => JsonSerializer.Serialize(source);
}