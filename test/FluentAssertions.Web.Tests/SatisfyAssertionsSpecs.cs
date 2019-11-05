using System;
using System.Net.Http;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Web.Tests
{
    public class SatisfyAssertionsSpecs
    {
        [Fact]
        public void When_asserting_response_with_a_certain_predicate_to_satisfy_predicate_it_should_succeed()
        {
            // Arrange
            using var subject = new HttpResponseMessage();

            // Act
            Action act = () =>
                subject.Should().Satisfy(response => true);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_response_without_having_satisfiable_predicate_to_satisfy_predicate_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage();

            // Act
            Action act = () =>
                subject.Should().Satisfy(c => c.Headers.AcceptRanges.Contains("byte"), "we want to test the {0}", "reason");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*c.Headers.AcceptRanges.Contains(\"byte\"), but it was not*reason*");
        }

        [Fact]
        public void When_asserting_response_to_satisfy_against_null_predicate_it_should_throw_with_descriptive_message()
        {
            // Arrange
            using var subject = new HttpResponseMessage();

            // Act
            Action act = () =>
                subject.Should().Satisfy(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*Cannot verify the subject satisfies a predicate that is `null`.*");
        }

        [Fact]
        public void When_asserting_null_response_to_be_satisfy_it_should_throw_with_descriptive_message()
        {
            // Arrange
            HttpResponseMessage subject = null;

            // Act
            Action act = () =>
                subject.Should().Satisfy(response => true, "because we want to test the failure {0}", "message"); ;

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(@"*Expected an HTTP response to assert because we want to test the failure message, but found <null>.");
        }
    }
}