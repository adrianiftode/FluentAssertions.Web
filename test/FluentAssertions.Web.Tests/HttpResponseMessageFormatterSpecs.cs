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

        [Theory]
        [InlineData(AspNetCore22InternalServerErrorResponse, "*System.Exception: Wow!*DeveloperExceptionPageMiddleware*", "<!DOCTYPE html>")]
        [InlineData(AspNetCore30InternalServerErrorResponse, "*System.Exception: Wow!*DeveloperExceptionPageMiddleware*", "HEADERS")]
        [InlineData(@"<pre class=""rawExceptionStackTrace"">", "*rawExceptionStackTrace*", "DOCTYPE")]
        [InlineData(@"</pre>", "*</pre>*", "DOCTYPE")]
        public void GivenInternalServerError_ShouldPrintExceptionDetails(string content, string expected, string unexpected)
        {
            // Arrange
            using var subject = new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                Content = new StringContent(content)
            };
            var sut = new HttpResponseMessageFormatter();

            // Act
            var formatted = sut.Format(subject, null, null);

            // Assert
            formatted.Should().Match(expected);
            formatted.Should().NotContain(unexpected);
        }

        private const string AspNetCore22InternalServerErrorResponse = @"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
    <head>
        <meta charset=""utf-8"" />
        <title>Internal Server Error</title>
        <style>
            body {
    font-family: 'Segoe UI', Tahoma, Arial, Helvetica, sans-serif;
    font-size: .813em;
    color: #222;
    background-color: #fff;
}

h1, h2, h3, h4, h5 {
    /*font-family: 'Segoe UI',Tahoma,Arial,Helvetica,sans-serif;*/
    font-weight: 100;
}

h1 {
    color: #44525e;
    margin: 15px 0 15px 0;
}

h2 {
    margin: 10px 5px 0 0;
}

h3 {
    color: #363636;
    margin: 5px 5px 0 0;
}

code {
    font-family: Consolas, ""Courier New"", courier, monospace;
}

body .titleerror {
    padding: 3px 3px 6px 3px;
    display: block;
    font-size: 1.5em;
    font-weight: 100;
}

body .location {
    margin: 3px 0 10px 30px;
}

#header {
    font-size: 18px;
    padding: 15px 0;
    border-top: 1px #ddd solid;
    border-bottom: 1px #ddd solid;
    margin-bottom: 0;
}

    #header li {
        display: inline;
        margin: 5px;
        padding: 5px;
        color: #a0a0a0;
        cursor: pointer;
    }

    #header .selected {
        background: #44c5f2;
        color: #fff;
    }

#stackpage ul {
    list-style: none;
    padding-left: 0;
    margin: 0;
    /*border-bottom: 1px #ddd solid;*/
}

#stackpage .details {
    font-size: 1.2em;
    padding: 3px;
    color: #000;
}

#stackpage .stackerror {
    padding: 5px;
    border-bottom: 1px #ddd solid;
}


#stackpage .frame {
    padding: 0;
    margin: 0 0 0 30px;
}

    #stackpage .frame h3 {
        padding: 2px;
        margin: 0;
    }

#stackpage .source {
    padding: 0 0 0 30px;
}

    #stackpage .source ol li {
        font-family: Consolas, ""Courier New"", courier, monospace;
        white-space: pre;
        background-color: #fbfbfb;
    }

#stackpage .frame .source .highlight li span {
    color: #FF0000;
}

#stackpage .source ol.collapsible li {
    color: #888;
}

    #stackpage .source ol.collapsible li span {
        color: #606060;
    }

.page table {
    border-collapse: separate;
    border-spacing: 0;
    margin: 0 0 20px;
}

.page th {
    vertical-align: bottom;
    padding: 10px 5px 5px 5px;
    font-weight: 400;
    color: #a0a0a0;
    text-align: left;
}

.page td {
    padding: 3px 10px;
}

.page th, .page td {
    border-right: 1px #ddd solid;
    border-bottom: 1px #ddd solid;
    border-left: 1px transparent solid;
    border-top: 1px transparent solid;
    box-sizing: border-box;
}

    .page th:last-child, .page td:last-child {
        border-right: 1px transparent solid;
    }

.page .length {
    text-align: right;
}

a {
    color: #1ba1e2;
    text-decoration: none;
}

    a:hover {
        color: #13709e;
        text-decoration: underline;
    }

.showRawException {
    cursor: pointer;
    color: #44c5f2;
    background-color: transparent;
    font-size: 1.2em;
    text-align: left;
    text-decoration: none;
    display: inline-block;
    border: 0;
    padding: 0;
}

.rawExceptionStackTrace {
    font-size: 1.2em;
}

.rawExceptionBlock {
    border-top: 1px #ddd solid;
    border-bottom: 1px #ddd solid;
}

.showRawExceptionContainer {
    margin-top: 10px;
    margin-bottom: 10px;
}

.expandCollapseButton {
    cursor: pointer;
    float: left;
    height: 16px;
    width: 16px;
    font-size: 10px;
    position: absolute;
    left: 10px;
    background-color: #eee;
    padding: 0;
    border: 0;
    margin: 0;
}

        </style>
    </head>
    <body>
        <h1>An unhandled exception occurred while processing the request.</h1>
            <div class=""titleerror"">Exception: Wow!</div>
                <p class=""location"">Sample.Api.Tests.CustomStartupConfigurationsTests&#x2B;&lt;&gt;c.&lt;GetException_WhenDeveloperPageIsConfigured_ShouldBeInternalServerError&gt;b__0_2(HttpContext context, Func&lt;Task&gt; next) in <code title=""E:\projects\mine\FluentAssertions.Web\samples\Sample.Api.Net22.Tests\CustomStartupConfigurationsTests.cs"">CustomStartupConfigurationsTests.cs</code>, line 26</p>
        <ul id=""header"">
            <li id=""stack"" tabindex=""1"" class=""selected"">
                Stack
            </li>
            <li id=""query"" tabindex=""2"">
                Query
            </li>
            <li id=""cookies"" tabindex=""3"">
                Cookies
            </li>
            <li id=""headers"" tabindex=""4"">
                Headers
            </li>
        </ul>

        <div id=""stackpage"" class=""page"">
            <ul>
                                    <li>
                        <h2 class=""stackerror"">Exception: Wow!</h2>
                        <ul>
                            <li class=""frame"" id=""frame1"">
                                    <h3>Sample.Api.Tests.CustomStartupConfigurationsTests&#x2B;&lt;&gt;c.&lt;GetException_WhenDeveloperPageIsConfigured_ShouldBeInternalServerError&gt;b__0_2(HttpContext context, Func&lt;Task&gt; next) in <code title=""E:\projects\mine\FluentAssertions.Web\samples\Sample.Api.Net22.Tests\CustomStartupConfigurationsTests.cs"">CustomStartupConfigurationsTests.cs</code></h3>

                                    <button class=""expandCollapseButton"" data-frameId=""frame1"">+</button>
                                    <div class=""source"">
                                            <ol start=""20"" class=""collapsible"">
                                                    <li><span>            {</span></li>
                                                    <li><span>                services.AddMvc();</span></li>
                                                    <li><span>            });</span></li>
                                                    <li><span>            builder.Configure(app =&gt; app</span></li>
                                                    <li><span>                .UseDeveloperExceptionPage()</span></li>
                                                    <li><span>                .Use((context, next) =&gt; {</span></li>
                                            </ol>

                                        <ol start=""26"" class=""highlight"">
                                                <li><span>                    throw new Exception(&quot;Wow!&quot;);</span></li>
                                        </ol>

                                            <ol start='27' class=""collapsible"">
                                                    <li><span>                }));</span></li>
                                                    <li><span></span></li>
                                                    <li><span>            using var testServer = new TestServer(builder);</span></li>
                                                    <li><span>            using var client = testServer.CreateClient();</span></li>
                                                    <li><span></span></li>
                                                    <li><span>            // Act</span></li>
                                            </ol>
                                    </div>
                            </li>
                            <li class=""frame"" id=""frame2"">
                                    <h3>Microsoft.AspNetCore.Builder.UseExtensions&#x2B;&lt;&gt;c__DisplayClass0_1.&lt;Use&gt;b__1(HttpContext context)</h3>

                            </li>
                            <li class=""frame"" id=""frame3"">
                                    <h3>Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)</h3>

                            </li>
                        </ul>
                    </li>
                    <li>
                        <br/>
                        <div class=""rawExceptionBlock"">
                            <div class=""showRawExceptionContainer"">
                                <button class=""showRawException"" data-exceptionDetailId=""exceptionDetail1"">Show raw exception details</button>
                            </div>
                            <div id=""exceptionDetail1"" class=""rawExceptionDetails"">
                                <pre class=""rawExceptionStackTrace"">System.Exception: Wow!&#xD;&#xA;   at Sample.Api.Tests.CustomStartupConfigurationsTests.&lt;&gt;c.&lt;GetException_WhenDeveloperPageIsConfigured_ShouldBeInternalServerError&gt;b__0_2(HttpContext context, Func`1 next) in E:\projects\mine\FluentAssertions.Web\samples\Sample.Api.Net22.Tests\CustomStartupConfigurationsTests.cs:line 26&#xD;&#xA;   at Microsoft.AspNetCore.Builder.UseExtensions.&lt;&gt;c__DisplayClass0_1.&lt;Use&gt;b__1(HttpContext context)&#xD;&#xA;   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)</pre>
                            </div>
                        </div>
                    </li>
            </ul>
        </div>

        <div id=""querypage"" class=""page"">
                <p>No QueryString data.</p>
        </div>

        <div id=""cookiespage"" class=""page"">
                <p>No cookie data.</p>
        </div>
        <div id=""headerspage"" class=""page"">
                <table>
                    <thead>
                        <tr>
                            <th>Variable</th>
                            <th>Value</th>
                        </tr>
                    </thead>
                    <tbody>
                                <tr>
                                    <td>Host</td>
                                    <td>localhost</td>
                                </tr>
                    </tbody>
                </table>
        </div>
        <script>
            //<!--
            (function (window, undefined) {
    ""use strict"";

    function ns(selector, element) {
        return new NodeCollection(selector, element);
    }

    function NodeCollection(selector, element) {
        this.items = [];
        element = element || window.document;

        var nodeList;

        if (typeof (selector) === ""string"") {
            nodeList = element.querySelectorAll(selector);
            for (var i = 0, l = nodeList.length; i < l; i++) {
                this.items.push(nodeList.item(i));
            }
        }
    }

    NodeCollection.prototype = {
        each: function (callback) {
            for (var i = 0, l = this.items.length; i < l; i++) {
                callback(this.items[i], i);
            }
            return this;
        },

        children: function (selector) {
            var children = [];

            this.each(function (el) {
                children = children.concat(ns(selector, el).items);
            });

            return ns(children);
        },

        hide: function () {
            this.each(function (el) {
                el.style.display = ""none"";
            });

            return this;
        },

        toggle: function () {
            this.each(function (el) {
                el.style.display = el.style.display === ""none"" ? """" : ""none"";
            });

            return this;
        },

        show: function () {
            this.each(function (el) {
                el.style.display = """";
            });

            return this;
        },

        addClass: function (className) {
            this.each(function (el) {
                var existingClassName = el.className,
                    classNames;
                if (!existingClassName) {
                    el.className = className;
                } else {
                    classNames = existingClassName.split("" "");
                    if (classNames.indexOf(className) < 0) {
                        el.className = existingClassName + "" "" + className;
                    }
                }
            });

            return this;
        },

        removeClass: function (className) {
            this.each(function (el) {
                var existingClassName = el.className,
                    classNames, index;
                if (existingClassName === className) {
                    el.className = """";
                } else if (existingClassName) {
                    classNames = existingClassName.split("" "");
                    index = classNames.indexOf(className);
                    if (index > 0) {
                        classNames.splice(index, 1);
                        el.className = classNames.join("" "");
                    }
                }
            });

            return this;
        },

        attr: function (name) {
            if (this.items.length === 0) {
                return null;
            }

            return this.items[0].getAttribute(name);
        },

        on: function (eventName, handler) {
            this.each(function (el, idx) {
                var callback = function (e) {
                    e = e || window.event;
                    if (!e.which && e.keyCode) {
                        e.which = e.keyCode; // Normalize IE8 key events
                    }
                    handler.apply(el, [e]);
                };

                if (el.addEventListener) { // DOM Events
                    el.addEventListener(eventName, callback, false);
                } else if (el.attachEvent) { // IE8 events
                    el.attachEvent(""on"" + eventName, callback);
                } else {
                    el[""on"" + type] = callback;
                }
            });

            return this;
        },

        click: function (handler) {
            return this.on(""click"", handler);
        },

        keypress: function (handler) {
            return this.on(""keypress"", handler);
        }
    };

    function frame(el) {
        ns("".source .collapsible"", el).toggle();
    }

    function expandCollapseButton(el) {
        var frameId = el.getAttribute(""data-frameId"");
        frame(document.getElementById(frameId));
        if (el.innerText === ""+"") {
            el.innerText = ""-"";
        }
        else {
            el.innerText = ""+"";
        }
    }

    function tab(el) {
        var unselected = ns(""#header .selected"").removeClass(""selected"").attr(""id"");
        var selected = ns(""#"" + el.id).addClass(""selected"").attr(""id"");

        ns(""#"" + unselected + ""page"").hide();
        ns(""#"" + selected + ""page"").show();
    }

    ns("".rawExceptionDetails"").hide();
    ns("".collapsible"").hide();
    ns("".page"").hide();
    ns(""#stackpage"").show();

    ns("".expandCollapseButton"")
        .click(function () {
            expandCollapseButton(this);
        })
        .keypress(function (e) {
            if (e.which === 13) {
                expandCollapseButton(this);
            }
        });

    ns(""#header li"")
        .click(function () {
            tab(this);
        })
        .keypress(function (e) {
            if (e.which === 13) {
                tab(this);
            }
        });

    ns("".showRawException"")
        .click(function () {
            var exceptionDetailId = this.getAttribute(""data-exceptionDetailId"");
            ns(""#"" + exceptionDetailId).toggle();
        });
})(window);
            //-->
        </script>
    </body>
</html>";
        private const string AspNetCore30InternalServerErrorResponse = @"System.Exception: Wow!
   at Sample.Api.Tests.CustomStartupConfigurationsTests.<>c.<GetException_WhenDeveloperPageIsConfigured_ShouldBeInternalServerError>b__0_3(HttpContext context) in E:\projects\mine\FluentAssertions.Web\samples\Sample.Api.Net30.Tests\CustomStartupConfigurationsTests.cs:line 30
   at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext)
--- End of stack trace from previous location where exception was thrown ---
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)

HEADERS
=======
Host: localhost
";
    }
}
