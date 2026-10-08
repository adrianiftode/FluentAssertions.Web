# Assertions.Web.Serializers.NewtonsoftJson

A [Newtonsoft.Json](https://www.newtonsoft.com/json) based serializer for the Assertions.Web family of packages: [FluentAssertions.Web](https://www.nuget.org/packages/FluentAssertions.Web), [FluentAssertions.Web.v8](https://www.nuget.org/packages/FluentAssertions.Web.v8), [AwesomeAssertions.Web](https://www.nuget.org/packages/AwesomeAssertions.Web) and [Shouldly.Web](https://www.nuget.org/packages/Shouldly.Web).

By default those packages deserialize the HTTP response content with `System.Text.Json`. Install this package to switch the default serializer to Newtonsoft.Json.

## Install

```shell
dotnet add package Assertions.Web.Serializers.NewtonsoftJson
```

## Usage

```csharp
AssertionsWebConfig.Serializer = new NewtonsoftJsonSerializer();
```

The `Newtonsoft.Json.JsonSerializerSettings` is accessible via the `NewtonsoftJsonSerializerConfig.Options` static field, so it can be customized, for example to add a custom converter:

```csharp
NewtonsoftJsonSerializerConfig.Options.Converters.Add(new YesNoBooleanJsonConverter());
```

The change must be done before the tests are run.