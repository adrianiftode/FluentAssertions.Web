using FluentAssertions.Web.Internal.Serializers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentAssertions.Web.Tests.Internal.Serializers
{
    public class SystemTextJsonSerializerTests
    {
        [Fact]
        public async Task GivenJsonWithAsAnArray_When_Deserialize_To_An_Object_Then_It_Returns_Null()
        {
            // Arrange
            var sut = new SystemTextJsonSerializer();
            var content = new MemoryStream(Encoding.UTF8.GetBytes("[]"));
            var model = new { item = default(int) };

            // Act
            var result = await sut.Deserialize(content, model.GetType());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GivenJsonWithAnObject_When_Deserialize_To_An_Object_Then_It_Returns_The_Object()
        {
            // Arrange
            var sut = new SystemTextJsonSerializer();
            var content = new MemoryStream(Encoding.UTF8.GetBytes("{ \"item\": 1}"));
            var model = new { item = 1 };

            // Act
            var result = await sut.Deserialize(content, model.GetType());

            // Assert
            result.Should().BeEquivalentTo(new { item = 1 });
        }
  }
}
