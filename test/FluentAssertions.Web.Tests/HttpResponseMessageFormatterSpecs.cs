using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace FluentAssertions.Web.Tests
{
    public class HttpResponseMessageFormatterSpecs
    {
        [Fact]
        public void GivenUnspecifiedResponse_ShouldPrintProtocolAndHaveContentLengthZero()
        {
            // Arrange
            using var subject = new HttpResponseMessage();
            var sut = new HttpResponseMessageFormatter();

            // Act
            var formatted = sut.Format(subject, null, null);

            // Assert
            formatted.Should().Match(@"*
The HTTP response was:*
HTTP/1.1 200 OK*
Content-Length: 0*
The originated HTTP request was <null>.*");
        }

        [Fact]
        public void GivenHeaders_ShouldPrintAllHeaders()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("", Encoding.UTF8, "text/html")
            };
            subject.Headers.Add("Cache-Control", "no-store, no-cache, max-age=0");
            subject.Headers.Add("Pragma", "no-cache");
            subject.Headers.Add("Request-Context", "appId=cid-v1:2e15fa14-72b6-44b3-a9b2-560e7b3234e5");
            subject.Headers.Add("Strict-Transport-Security", "max-age=31536000");
            subject.Headers.Add("Date", "Thu, 26 Sep 2019 22:33:34 GMT");
            subject.Headers.Add("Connection", "close");
            var sut = new HttpResponseMessageFormatter();

            // Act
            var formatted = sut.Format(subject, null, null);

            // Assert
            formatted.Should().Match(
@"*The HTTP response was:*
HTTP/1.1 200 OK*
Cache-Control: no-store, no-cache, max-age=0*
Pragma: no-cache*
Request-Context: appId=cid-v1:2e15fa14-72b6-44b3-a9b2-560e7b3234e5*
Strict-Transport-Security: max-age=31536000*
Date: Thu, 26 Sep 2019 22:33:34 GMT*
Connection: close*
Content-Type: text/html; charset=utf-8*
Content-Length: 0*
The originated HTTP request was <null>.*");
        }

        [Fact]
        public void GivenResponseWithContent_ShouldPrintContent()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
    ""glossary"": {
        ""title"": ""example glossary"",
        ""GlossDiv"": {
            ""title"": ""S"",
            ""GlossList"": {
                ""GlossEntry"": {
                    ""ID"": ""SGML"",
                    ""SortAs"": ""SGML"",
                    ""GlossTerm"": ""Standard Generalized Markup Language"",
                    ""Acronym"": ""SGML"",
                    ""Abbrev"": ""ISO 8879:1986"",
                    ""GlossDef"": {
                        ""para"": ""A meta-markup language, used to create markup languages such as DocBook."",
                        ""GlossSeeAlso"": [
                            ""GML"",
                            ""XML""
                        ]
                    },
                    ""GlossSee"": ""markup""
                }
            }
        }
    }
}", Encoding.UTF8, "application/json")
            };
            var sut = new HttpResponseMessageFormatter();

            // Act
            var formatted = sut.Format(subject, null, null);

            // Assert
            formatted.Should().Match(@"*The HTTP response was:*
HTTP/1.1 200 OK*
Content-Type: application/json; charset=utf-8*
Content-Length:*
{*
    ""glossary"": {*
        ""title"": ""example glossary"",*
        ""GlossDiv"": {*
            ""title"": ""S"",*
            ""GlossList"": {*
                ""GlossEntry"": {*
                    ""ID"": ""SGML"",*
                    ""SortAs"": ""SGML"",*
                    ""GlossTerm"": ""Standard Generalized Markup Language"",*
                    ""Acronym"": ""SGML"",*
                    ""Abbrev"": ""ISO 8879:1986"",*
                    ""GlossDef"": {*
                        ""para"": ""A meta-markup language, used to create markup languages such as DocBook."",*
                        ""GlossSeeAlso"": [*
                            ""GML"",*
                            ""XML""*
                        ]*
                    },*
                    ""GlossSee"": ""markup""*
                }*
            }*
        }*
    }*
}*
The originated HTTP request was <null>.*");
        }

        [Fact]
        public void GivenResponseWithMinifiedJson_ShouldPrintFormattedJson()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{""glossary"":{""title"":""example glossary"",""GlossDiv"":{""title"":""S"",""GlossList"":{""GlossEntry"":{""ID"":""SGML"",""SortAs"":""SGML"",""GlossTerm"":""Standard Generalized Markup Language"",""Acronym"":""SGML"",""Abbrev"":""ISO 8879:1986"",""GlossDef"":{""para"":""A meta-markup language, used to create markup languages such as DocBook."",""GlossSeeAlso"":[""GML"",""XML""]},""GlossSee"":""markup""}}}}}", Encoding.UTF8, "application/json")
            };
            var sut = new HttpResponseMessageFormatter();

            // Act
            var formatted = sut.Format(subject, null, null);

            // Assert
            formatted.Should().Match(@"*
The HTTP response was:*
HTTP/1.1 200 OK*
Content-Type: application/json; charset=utf-8*
Content-Length:*
{*
    ""glossary"": {*
        ""title"": ""example glossary"",*
        ""GlossDiv"": {*
            ""title"": ""S"",*
            ""GlossList"": {*
                ""GlossEntry"": {*
                    ""ID"": ""SGML"",*
                    ""SortAs"": ""SGML"",*
                    ""GlossTerm"": ""Standard Generalized Markup Language"",*
                    ""Acronym"": ""SGML"",*
                    ""Abbrev"": ""ISO 8879:1986"",*
                    ""GlossDef"": {*
                        ""para"": ""A meta-markup language, used to create markup languages such as DocBook."",*
                        ""GlossSeeAlso"": [*
                            ""GML"",*
                            ""XML""*
                        ]*
                    },*
                    ""GlossSee"": ""markup""*
                }*
            }*
        }*
    }*
}*
The originated HTTP request was <null>.*");
        }


        [Fact]
        public void GivenHtmlResponse_ShouldPrintAsItIs()
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"<html>
<head>
<title>Title of the document</title>
</head>

<body>
The content of the document......
</body>

</html>", Encoding.UTF8, "text/html")
            };
            var sut = new HttpResponseMessageFormatter();

            // Act
            var formatted = sut.Format(subject, null, null);

            // Assert
            formatted.Should().Match(@"*
The HTTP response was:*
HTTP/1.1 200 OK*
Content-Type: text/html; charset=utf-8*
Content-Length:*
<html>*
<head>*
<title>Title of the document</title>*
</head>*
<body>*
The content of the document......*
</body>*
</html>*
The originated HTTP request was <null>.*");
        }

        [Fact]
        public void GivenContentLengthInHeaders_ShouldNotPrintItTwice()
        {
            using var subject = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };
            subject.Content.Headers.Add("Content-Length", "0");
            var sut = new HttpResponseMessageFormatter();

            // Act
            var formatted = sut.Format(subject, null, null);

            // Assert
            formatted.Should().Match(
                @"*The HTTP response was:*
HTTP/1.1 200 OK*
Content-Type: text/plain; charset=utf-8*
Content-Length: 0*
The originated HTTP request was <null>.*");
        }

        [Fact]
        public void GivenRequest_ShouldPrintRequestDetails()
        {
            using var subject = new HttpResponseMessage(HttpStatusCode.OK)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5001/")
                {
                    Content = new StringContent("Some content."),
                    Headers = { { "Authorization", "Bearer xyz" } }
                }
            };
            var sut = new HttpResponseMessageFormatter();

            // Act
            var formatted = sut.Format(subject, null, null);

            // Assert
            formatted.Should().Match(
                @"*The HTTP response was:*
HTTP/1.1 200 OK*
Content-Length: 0*
The originated HTTP request was:*
POST http://localhost:5001/ HTTP 1.1*
Authorization: Bearer xyz*
Content-Type: text/plain; charset=utf-8*
Content-Length: *
Some content.*");
        }
    }
}
