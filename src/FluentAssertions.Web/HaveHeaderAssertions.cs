using FluentAssertions.Execution;
using FluentAssertions.Web.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FluentAssertions.Web
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state related to HTTP headers.
    /// </summary>
    public class HeadersAssertions : HttpResponseMessageAssertions
    {
        private readonly string _header;

        /// <summary>
        /// Initialized a new instance of the <see cref="HeadersAssertions"/>
        /// class.
        /// </summary>
        /// <param name="value">The subject value to be asserted.</param>
        /// <param name="header">The HTTP header name to be asserted.</param>
        public HeadersAssertions(HttpResponseMessage value, string header) : base(value) => _header = header;

        /// <summary>
        /// Asserts that an existing HTTP header in a HTTP response contains at least a value that matches a wildcard pattern.
        /// </summary>
        /// <param name="expectedWildcardValue">
        /// The wildcard pattern with which the subject is matched, where * and ? have special meanings.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<HeadersAssertions> Match(string expectedWildcardValue, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expectedWildcardValue, nameof(expectedWildcardValue), "Cannot verify a HTTP header to be a value against a <null> value. Use And.BeEmpty to test if the HTTP header has no values.");

            ExecuteSubjectNotNull(because, becauseArgs);

            IEnumerable<string> headerValues = Subject.GetHeaderValues(_header);
            
            var matchFound = headerValues.Any(headerValue => {
                using (var scope = new AssertionScope())
                {
                    headerValue.Should().Match(expectedWildcardValue);
                    return !scope.Discard().Any();
                }
            });

            Execute.Assertion
                         .BecauseOf(because, becauseArgs)
                         .ForCondition(matchFound)
                         .FailWith("Expected {context:value} to contain " +
                                   "the HTTP header {0} having value {1}, but no such header was found in the actual response{reason}. {2}",
                             _header,
                             expectedWildcardValue,
                             Subject);

            return new AndConstraint<HeadersAssertions>(this);
        }

        /// <summary>
        /// Asserts that an existing HTTP header in a HTTP response has no values.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<HeadersAssertions> BeEmpty(string because = "", params object[] becauseArgs)
        {            
            var headerValues = Subject.GetHeaderValues(_header);

            Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .ForCondition(!headerValues.Any())
                        .FailWith("Expected {context:value} to contain " +
                                  "the HTTP header {0} with no header values, but the found the header and it has values {1} in the actual response{reason}. {2}",
                            _header,
                            headerValues,
                            Subject);

            return new AndConstraint<HeadersAssertions>(this);
        }

        /// <summary>
        /// Asserts that an existing HTTP header in a HTTP response has an expected list of header values.
        /// </summary>
        /// <param name="expectedValues">
        /// The expected values with which the HTTP headers values list is compared.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<HeadersAssertions> BeValues(IEnumerable<string> expectedValues,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expectedValues, nameof(expectedValues), "Cannot verify a HTTP header to be a collection of expected values against a <null> collection. Use And.BeEmpty to test if the HTTP header has no values.");
            if (!expectedValues.Any())
            {
                throw new ArgumentException("Cannot verify a HTTP header to be a collection of expected values against an empty collection. Use And.BeEmpty to test if the HTTP header has no values.", nameof(expectedValues));
            }

            var values = Subject.GetHeaders().FirstOrDefault(c => c.Key == _header).Value;

            string[] failures;

            using (var scope = new AssertionScope())
            {
                values.Should().BeEquivalentTo(expectedValues);

                failures = scope.Discard();
            }

            Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .ForCondition(failures.Length == 0)
                        .FailWith("Expected {context:value} to contain " +
                                  "the HTTP header {0} having values {1}, but the found values have differences{reason}. {2}",
                            _header,
                            expectedValues,
                            Subject);

            return new AndConstraint<HeadersAssertions>(this);
        }
    }

    public partial class HttpResponseMessageAssertions
    {
        /// <summary>
        /// Asserts that an HTTP response has a named header.
        /// </summary>
        /// <param name="expectedHeader">
        /// The expected header with which the HTTP headers list is matched.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<HeadersAssertions> HaveHeader(string expectedHeader,
            string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(expectedHeader, nameof(expectedHeader), "Cannot verify having a header against a <null> header.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(IsHeaderPresent(expectedHeader))
                .FailWith("Expected {context:value} to contain " +
                          "the HTTP header {0}, but no such header was found in the actual response{reason}.{1}",
                    expectedHeader,
                    Subject);

            return new AndConstraint<HeadersAssertions>(new HeadersAssertions(Subject, expectedHeader));
        }

        private bool IsHeaderPresent(string expectedHeader) => Subject.GetHeaders().Any(c => string.Equals(c.Key, expectedHeader, StringComparison.OrdinalIgnoreCase));
    }
}