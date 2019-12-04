using FluentAssertions.Execution;
using FluentAssertions.Formatting;
using FluentAssertions.Primitives;
using FluentAssertions.Web.Internal;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentAssertions.Web
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="HttpResponseMessage"/> is in the expected state.
    /// </summary>
    public partial class HttpResponseMessageAssertions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>
    {
        static HttpResponseMessageAssertions()
        {
            Formatter.AddFormatter(new HttpResponseMessageFormatter());
            Formatter.AddFormatter(new AssertionsFailuresFormatter());
        }

        /// <summary>
        /// Initialized a new instance of the <see cref="HttpResponseMessageAssertions"/>
        /// class.
        /// </summary>
        /// <param name="value">The subject value to be asserted.</param>
        public HttpResponseMessageAssertions(HttpResponseMessage value) => Subject = value;

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => $"{nameof(HttpResponseMessage)}";

        private protected void ExecuteSubjectNotNull(string because, object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected an HTTP {context:response} to assert{reason}, but found <null>.");
        }

        private void ExecuteStatusAssertion(string because, object[] becauseArgs, HttpStatusCode expected, string? otherName = null)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(expected == Subject.StatusCode)
                .FailWith("Expected HTTP {context:response} to be {0}{reason}, but found {1}.{2}"
                    , otherName ?? expected.ToString(), Subject.StatusCode, Subject);
        }

        private protected string? GetContent()
        {
            Func<Task<string?>> content = () => Subject.GetStringContent();
            return content.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
        }

        private protected bool TryGetSubjectModel<TModel>(out TModel model)
        {
            Func<Task<TModel>> readModel = () => Subject.Content.ReadAsAsync<TModel>();
            try
            {
                model = readModel.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
                return true;
            }
            catch (JsonReaderException)
            {
                model = default;
                return false;
            }
        }

        private string[] CollectFailuresFromAssertion<TAsserted>(Action<TAsserted> assertion, TAsserted subject)
        {
            string[] collectionFailures;
            using (var collectionScope = new AssertionScope())
            {
                string[] assertionFailures;
                using (var itemScope = new AssertionScope())
                {
                    try
                    {
                        assertion(subject);
                        assertionFailures = itemScope.Discard();
                    }
                    catch (Exception ex)
                    {
                        assertionFailures = new[] { $"Expected to successfully verify an assertion, but the following exception occured: { ex }" };
                    }

                }

                foreach (var assertionFailure in assertionFailures)
                {
                    collectionScope.AddPreFormattedFailure($"{assertionFailure}");
                }

                collectionFailures = collectionScope.Discard();
            }

            return collectionFailures;
        }
    }
}