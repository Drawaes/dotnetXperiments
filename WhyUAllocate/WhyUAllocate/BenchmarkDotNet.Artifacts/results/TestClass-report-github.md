``` ini

BenchmarkDotNet=v0.10.9, OS=Windows 10.0.16296
Processor=Intel Xeon CPU E3-1505M v5 2.80GHz, ProcessorCount=8
Frequency=2742185 Hz, Resolution=364.6727 ns, Timer=TSC
.NET Core SDK=2.1.0-preview1-007214
  [Host]     : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 2.0.0 (Framework 4.6.00001.0), 64bit RyuJIT


```
 |     Method |     Mean |    Error |   StdDev |    Gen 0 | Allocated |
 |----------- |---------:|---------:|---------:|---------:|----------:|
 |    Closure | 608.6 us | 8.726 us | 8.163 us | 208.9844 | 859.38 KB |
 | NotClosure | 274.7 us | 2.162 us | 1.806 us |  57.1289 | 234.38 KB |
