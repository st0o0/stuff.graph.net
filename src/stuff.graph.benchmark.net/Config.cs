using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;

namespace stuff.graph.benchmark.net;

public class Config : ManualConfig
{
    public Config()
    {
        AddExporter(CsvMeasurementsExporter.Default);
        AddExporter(RPlotExporter.Default);
    }
}