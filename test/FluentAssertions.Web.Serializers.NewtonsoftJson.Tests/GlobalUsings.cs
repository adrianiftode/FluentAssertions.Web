global using System;
global using System.Net.Http;
global using System.Text;
global using Xunit;
global using Xunit.Abstractions;
global using Xunit.Sdk;
#if AAV
global using AwesomeAssertions;
#elif SH
global using Shouldly;
#else
global using FluentAssertions;
#endif