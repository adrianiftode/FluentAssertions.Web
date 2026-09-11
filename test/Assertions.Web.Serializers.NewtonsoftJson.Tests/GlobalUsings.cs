global using Assertions.Web;
global using System;
global using System.Net.Http;
global using System.Text;
global using Xunit;
global using Xunit.Abstractions;
global using Xunit.Sdk;
#if SH
global using Shouldly;
#elif AAV
global using AwesomeAssertions;
#else
global using FluentAssertions;
#endif
