namespace Rick.Cli;

public record AppConfig
{
    public string MyValue { get; init; } = "config value";
}