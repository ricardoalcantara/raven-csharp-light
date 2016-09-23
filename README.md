# RavenSharp Light
SharpRaven Light a lightweight .NET client for [Sentry](https://getsentry.com/welcome/).

## Disclaimer
It is a `Light` version of the oficial lib RavenSharp, it contains just the basic to log the event.
It is designed to be compatible with the oficial lib, so it implements the same interface `IClientSentry.cs`.
The usage is also very similar.

## Usage 
- Retrieved from [RavenSharp](https://github.com/wakawaka54/raven-csharp/blob/develop/README.md)

Instantiate the client with your DSN:

```csharp
var ravenClient = new RavenClientLight("http://public:secret@example.com/project-id");
```

### Capturing Exceptions
Call out to the client in your catch block:

```csharp
try
{
    int i2 = 0;
    int i = 10 / i2;
}
catch (Exception exception)
{
    ravenClient.Capture(new SentryEvent(exception));
}
```

### Logging Non-Exceptions
You can capture a message without being bound by an exception:

```csharp
ravenClient.Capture(new SentryEvent("Hello World!"));
```

### Additional Data
You can add additional data to the
[`Exception.Data`](https://msdn.microsoft.com/en-us/library/system.exception.data.aspx)
property on exceptions thrown about in your solution:

```csharp
try
{
    // ...    
}
catch (Exception exception)
{
    exception.Data.Add("SomeKey", "SomeValue");
    throw;
}
```