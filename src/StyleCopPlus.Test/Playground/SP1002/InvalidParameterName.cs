using System;
using System.Threading;

namespace AnalyzerPlayground
{
    public class SP1002CancellationTokenNameAnalyzer
    {
        public void Main(string param1, CancellationToken /*C*/token)
        {
            token.ThrowIfCancellationRequested();
        }
    }
}
