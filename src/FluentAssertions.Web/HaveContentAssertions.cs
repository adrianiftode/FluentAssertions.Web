using FluentAssertions.Execution;

namespace FluentAssertions.Web
{
    public partial class HttpResponseMessageAssertions
    {
        [CustomAssertion]
        public AndConstraint<HttpResponseMessageAssertions> HaveContent<TModel>(TModel expectedModel, string because = "", params object[] becauseArgs)
        {
            var subjectModel = GetSubjectModel<TModel>();

            using (var assertion = new AssertionScope())
            {
                subjectModel.Should().BeEquivalentTo(expectedModel, because, becauseArgs);

                if (assertion.HasFailures())
                {
                    Execute.Assertion
                        .FailWith("{0}", Subject);
                }
            }

            return new AndConstraint<HttpResponseMessageAssertions>(this);
        }
    }
}