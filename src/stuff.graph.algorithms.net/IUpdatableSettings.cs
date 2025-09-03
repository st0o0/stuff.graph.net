namespace stuff.graph.algorithms.net;

public interface IUpdatableSettings<in TSettings> : IUpdatable<TSettings> where TSettings : ISettings;
