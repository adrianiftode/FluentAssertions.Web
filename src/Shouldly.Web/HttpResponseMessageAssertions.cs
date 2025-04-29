using FluentAssertions;

namespace Shouldly
{
    [ShouldlyMethods]
    public static partial class HttpResponseMessageAssertions
    {
        #region Be2XXSuccessful
        /// <summary>
        /// Asserts that a HTTP response has a successful HTTP status code.
        /// </summary>
        /// <remarks>The HTTP response was successful if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was in the range 200-299.</remarks>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        // ReSharper disable once InconsistentNaming
        public static void ShouldBe2XXSuccessful(this HttpResponseMessage response, string because = "", params object[] becauseArgs)
        {
            response.ShouldNotBeNull("Expected a {context:response} to assert{reason}, but found <null>.");
            response.IsSuccessStatusCode.ShouldBeTrue("Expected {context:response} to have a successful HTTP status code, but it was {0}{reason}.{1}");
        }
        #endregion

        #region NotBe2XXSuccessful
        /// <summary>
        /// Asserts that a HTTP response has no successful HTTP status code.
        /// </summary>
        /// <remarks>The HTTP response was successful if <see cref="P:System.Net.Http.HttpResponseMessage.StatusCode" /> was NOT in the range 200-299.</remarks>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        // ReSharper disable once InconsistentNaming
        public static void ShouldNotBe2XXSuccessful(this HttpResponseMessage response, string because = "", params object[] becauseArgs)
        {
            response.ShouldNotBeNull("Expected a {context:response} to assert{reason}, but found <null>.");
            response.IsSuccessStatusCode.ShouldNotBe(true, "Expected {context:response} to have a successful HTTP status code, but it was {0}{reason}." + HttpResponseMessageFormatted.GetFormatted(response));
        }
        #endregion

        /// <summary>
        /// Asserts that HTTP response content can be an equivalent representation of the expected model.
        /// </summary>
        /// <param name="expectedModel">
        /// The expected model.
        /// </param>
        /// <param name="options">
        /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{TExpectation}"/> class. The global defaults are determined by the
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see paramref="because" />.
        /// </param>
        public static void ShouldBeAs<TModel>(this HttpResponseMessage response, TModel? expectedModel, string because = "", params object[] becauseArgs)
        {
            //Guard.ThrowIfArgumentIsNull(expectedModel, nameof(expectedModel), "Cannot verify a HTTP response content match a <null> wildcard pattern.");


            response.ShouldNotBeNull("Expected a {context:response} to assert{reason}, but found <null>.");

            var expectedModelType = expectedModel?.GetType();

            var (success, errorMessage) = TryGetSubjectModel(response, out var subjectModel, expectedModelType);

            success.ShouldBeTrue();
            try
            {
                subjectModel.ShouldBeEquivalentTo(expectedModel, "Expected {context:response} to have a content equivalent to a model, but it has differences:{0}{reason}. {1}" + HttpResponseMessageFormatted.GetFormatted(response));
            }
            catch (Exception ex)
            {
                return;
            }
                //Execute.Assertion
            //    .BecauseOf(because, becauseArgs)
            //    .ForCondition(success)
            //    .FailWith("Expected {context:response} to have a content equivalent to a model of type {0}, but the JSON representation could not be parsed, as the operation failed with the following message: {2}{reason}. {1}",
            //        expectedModelType.ToString() ?? "unknown type", Subject, errorMessage);

            string[] failures;

            //using (var scope = new AssertionScope())
            //{
            //    subjectModel.Should().BeEquivalentTo(expectedModel, options);

            //    failures = scope.Discard();
            //}

            //Execute.Assertion
            //           .BecauseOf(because, becauseArgs)
            //           .ForCondition(failures.Length == 0)
            //           .FailWith("Expected {context:response} to have a content equivalent to a model, but it has differences:{0}{reason}. {1}",
            //               new AssertionsFailures(failures),
            //               Subject);

            //return new AndConstraint<HttpResponseMessageAssertions>(this);
        }

        private static (bool success, string? errorMessage) TryGetSubjectModel<TModel>(HttpResponseMessage subject, out TModel? model)
        {
            var (success, errorMessage) = TryGetSubjectModel(subject, out var subjectModel, typeof(TModel));
            model = (TModel?)subjectModel;
            return (success, errorMessage);
        }

        private static (bool success, string? errorMessage) TryGetSubjectModel(HttpResponseMessage subject, out object? model, Type modelType)
        {
            Func<Task<object?>> readModel = () => subject.Content.ReadAsAsync(modelType, ShouldlyWebConfig.Serializer);
            try
            {
                model = readModel.ExecuteInDefaultSynchronizationContext().GetAwaiter().GetResult();
                return (true, null);
            }
            catch (Exception ex) when (ex is DeserializationException or NotSupportedException)
            {
                model = default;
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += $": {ex.InnerException.Message}";
                }

                return (false, message);
            }
        }

        private static string[] CollectFailuresFromAssertion<TAsserted>(Action<TAsserted> assertion, TAsserted subject)
        {
            //using var collectionScope = new AssertionScope();
            //string[] assertionFailures;
            //using (var itemScope = new AssertionScope())
            //{
            //    try
            //    {
            //        assertion(subject);
            //        assertionFailures = itemScope.Discard();
            //    }
            //    catch (Exception ex)
            //    {
            //        assertionFailures = new[] { $"Expected to successfully verify an assertion, but the following exception occurred: {ex}" };
            //    }

            //}

            //foreach (var assertionFailure in assertionFailures)
            //{
            //    collectionScope.AddPreFormattedFailure($"{assertionFailure}");
            //}

            //return collectionScope.Discard();
            return new string[] { };
        }
    }
}
