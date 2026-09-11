global using Assertions.Web;
global using Assertions.Web.Internal;
#if !SH
global using FluentAssertions.Equivalency;
global using FluentAssertions.Execution;
#endif
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Net.Http.Headers;
global using System.Net.Http;
global using System.Text;
global using System.Text.Json;
global using System.Threading.Tasks;
global using Xunit;
global using Xunit.Sdk;
#if SH
global using Shouldly;
#else
global using FluentAssertions;
#endif
