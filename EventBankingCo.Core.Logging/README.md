Example JSON for LoggingOptions:

```json
{
  "LoggingOptions": {
    "Console": {
      "Enabled": true,
      "MinLevel": "Debug"
    },
    "File": {
      "Enabled": true,
      "MinLevel": "Information",
      "Path": "logs/service-log-.txt"
    },
    "Database": {
      "Enabled": true,
      "MinLevel": "Error",
      "ConnectionString": "Server=localhost;Database=Logs;Integrated Security=true;"
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    }
  }
}

```

If you want to adjust log levels live, you could expose a minimal API in each service:

```c#
    app.MapPost("/loglevel/{sink}/{level}", (string sink, string level, Dictionary<string, LoggingLevelSwitch> switches) =>
    {
        if (!switches.TryGetValue(sink, out var levelSwitch))
            return Results.NotFound("Sink not found");

        if (!Enum.TryParse<LogEventLevel>(level, true, out var newLevel))
            return Results.BadRequest("Invalid log level");

        levelSwitch.MinimumLevel = newLevel;
        return Results.Ok($"Log level for {sink} set to {newLevel}");
    });
```
