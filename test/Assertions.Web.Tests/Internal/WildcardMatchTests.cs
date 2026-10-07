using FluentAssertions.Execution;
using Xunit.Sdk;

namespace Assertions.Web.Tests.Internal;

public class WildcardMatchTests
{
    [Fact]
    public void Given_an_exact_match_Then_it_matches()
    {
        "hello world".WildcardMatch("hello world").Should().BeTrue();
    }

    [Fact]
    public void Given_a_different_string_Then_it_does_not_match()
    {
        "hello world".WildcardMatch("hello there").Should().BeFalse();
    }

    [Theory]
    [InlineData("prefix and more", "pre*", true)]
    [InlineData("pre", "pre*", true)]
    [InlineData(" something else", "pre*", false)]
    public void Given_a_star_suffix_Then_it_matches_any_ending(string actual, string pattern, bool expected)
        => actual.WildcardMatch(pattern).Should().Be(expected);

    [Theory]
    [InlineData("and more suf", "*suf", true)]
    [InlineData("suf", "*suf", true)]
    [InlineData("suffix and more", "*suf", false)]
    public void Given_a_star_prefix_Then_it_matches_any_beginning(string actual, string pattern, bool expected)
        => actual.WildcardMatch(pattern).Should().Be(expected);

    [Theory]
    [InlineData("alpha beta gamma", "alpha*gamma", true)]
    [InlineData("alpha gamma", "alpha*gamma", true)]
    [InlineData("beta alpha gamma", "alpha*gamma", false)]
    public void Given_a_star_in_the_middle_Then_it_matches_the_gap(string actual, string pattern, bool expected)
        => actual.WildcardMatch(pattern).Should().Be(expected);

    [Theory]
    [InlineData("cat", "ca?", true)]
    [InlineData("can", "ca?", true)]
    [InlineData("cart", "ca?", false)]
    [InlineData("x", "?", true)]
    [InlineData("", "?", false)]
    public void Given_a_question_mark_Then_it_matches_exactly_one_character(string actual, string pattern, bool expected)
        => actual.WildcardMatch(pattern).Should().Be(expected);

    [Theory]
    [InlineData("a.b", "a.b", true)]
    [InlineData("aXb", "a.b", false)]
    [InlineData("aXb", "a.*", false)]
    [InlineData("a.b.c", "a.*", true)]
    [InlineData("a+b", "a+b", true)]
    [InlineData("(x)", "(x)", true)]
    [InlineData("[1]", "[1]", true)]
    [InlineData("$end^", "$end^", true)]
    [InlineData("a|b", "a|b", true)]
    [InlineData(@"back\slash", @"back\slash", true)]
    public void Given_regex_metacharacters_in_the_pattern_Then_they_are_matched_literally(string actual, string pattern, bool expected)
        => actual.WildcardMatch(pattern).Should().Be(expected);

    [Fact]
    public void Given_a_multi_line_actual_Then_a_star_crosses_newlines()
    {
        "line1\nline2\nline3".WildcardMatch("line1*line3").Should().BeTrue();
    }

    [Theory]
    [InlineData("Hello", "hello")]
    [InlineData("hello", "Hello")]
    [InlineData("HELLo", "hello")]
    public void Given_a_differing_case_Then_it_does_not_match(string actual, string pattern)
        => actual.WildcardMatch(pattern).Should().BeFalse();

    [Fact]
    public void Given_an_empty_pattern_and_an_empty_actual_Then_it_matches()
    {
        string.Empty.WildcardMatch(string.Empty).Should().BeTrue();
    }

    [Fact]
    public void Given_an_empty_pattern_and_a_non_empty_actual_Then_it_does_not_match()
    {
        "something".WildcardMatch(string.Empty).Should().BeFalse();
    }

    [Fact]
    public void Given_a_null_actual_Then_it_does_not_match()
    {
        string? actual = null;
        actual.WildcardMatch("*").Should().BeFalse();
        actual.WildcardMatch(string.Empty).Should().BeFalse();
        actual.WildcardMatch("exact").Should().BeFalse();
    }

    [Theory]
    [InlineData("hello", "hello", true)]
    [InlineData("hello", "hell", false)]
    [InlineData("hello world", "hello*", true)]
    [InlineData("hello world", "*world", true)]
    [InlineData("hello world", "hello*world", true)]
    [InlineData("hello world", "h?llo world", true)]
    [InlineData("file.txt", "file.???", true)]
    [InlineData("file.txt", "file.txt.bak", false)]
    [InlineData("abc", "a.c", false)]
    [InlineData("a.c", "a.c", true)]
    [InlineData("abc", "*", true)]
    [InlineData("abc", "a*b*c", true)]
    [InlineData("2+2=4", "2+2=4", true)]
    [InlineData("nothing", "some*thing", false)]
    public void Given_pairs_when_cross_checked_with_FluentAssertions_Match_then_they_agree(string actual, string pattern, bool expected)
    {
        var ours = actual.WildcardMatch(pattern);
        ours.Should().Be(expected, "our WildcardMatch must follow its own spec");

        bool theirs;
        try
        {
            actual.Should().Match(pattern);
            theirs = true;
        }
        catch (Exception ex) when (ex is AssertionFailedException or XunitException)
        {
            theirs = false;
        }

        theirs.Should().Be(ours, "WildcardMatch must agree with FluentAssertions' Should().Match on {0} vs {1}", actual, pattern);
    }
}
