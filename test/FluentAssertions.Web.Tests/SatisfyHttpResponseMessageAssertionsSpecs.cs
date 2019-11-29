using System;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests
{
    public class SatisfyHttpResponseMessageAssertionsSpecs
    {
        [Fact]
        public void When_asserting_response_with_a_certain_assertion_to_satisfy_assertions_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage();

            // Act
            Action act = () =>
                subject.Should().Satisfy(response => true.Should().BeTrue());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_response_without_having_satisfiable_assertion_to_satisfy_assertions_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage();

            // Act
            Action act = () =>
                subject.Should().Satisfy(c => c.Headers.AcceptRanges.Should().Contain("byte"), "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected value to satisfy one or more assertions, but it wasn't because we want to test the reason:*expected*{empty} to contain \"byte\"*HTTP response*");
        }

        [Fact]
        public void When_asserting_response_without_having_satisfiable_assertion_to_satisfy_several_assertions_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage();

            // Act
            Action act = () =>
                subject.Should().Satisfy(
                    response =>
                    {
                        response.Headers.AcceptRanges.Should().Contain("byte");
                        response.Headers.Should().BeNull();
                    }, "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"Expected value to satisfy one or more assertions, but it wasn't because we want to test the reason:*expected*""byte""*expected*to be <null>*The HTTP response was:*");
        }

        [Fact]
        public void When_asserting_response_to_satisfy_against_null_assertion_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage();

            // Act
            Action act = () =>
                subject.Should().Satisfy(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*Cannot verify the subject satisfies a `null` assertion.*");
        }

        [Fact]
        public void When_asserting_null_response_to_be_satisfy_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage subject = null;

            // Act
            Action act = () =>
                subject.Should().Satisfy(response => true.Should().BeTrue(), "because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
    }
}