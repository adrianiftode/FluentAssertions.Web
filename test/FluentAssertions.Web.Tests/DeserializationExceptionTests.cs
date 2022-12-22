namespace FluentAssertions.Web.Tests;

public class DeserializationExceptionTests
{
    [Fact]
    public void Ctor_DoesNotThrow()
    {
        // Act
        Action act = () => new DeserializationException();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Ctor_WhenMessage_DoesNotThrow()
    {
        // Act
        Action act = () => new DeserializationException("Exception message");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Ctor_WhenInnerException_DoesNotThrow()
    {
        // Act
        Action act = () => new DeserializationException("Exception message", new Exception());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Exception_ShouldBeBinarySerializable()
    {
        // Arrange
        var sut = new DeserializationException("Exception message", new Exception());

        // Assert
        sut.Should().BeBinarySerializable();
    }
}
