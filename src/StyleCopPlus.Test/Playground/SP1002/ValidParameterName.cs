using System;

namespace AnalyzerPlayground
{
    public class SP1002CancellationTokenNameAnalyzer
    {
        public void Main(string param1, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
        }
    }
}
