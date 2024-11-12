namespace stuff.graph.algorithms.net;

public interface IUpdatableSettings<TSettings> : IUpdatable<TSettings> where TSettings : ISettings
{ }
