namespace stuff.graph.algorithms.net;

public interface IGeneratorConfig<TSettings> : IGeneratorConfig, IConfig<TSettings> where TSettings : ISettings
{
}

public interface IGeneratorConfig : IConfig
{
    Random? Instance { get; init; }
};

public record GeneratorConfig(Random? Instance) : IGeneratorConfig;

public record GeneratorConfig<TSettings>(TSettings Settings, Random? Instance = null): IGeneratorConfig<TSettings> where TSettings : ISettings;