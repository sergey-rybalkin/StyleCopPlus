using System;
using System.Threading;

namespace AnalyzerPlayground
{
    public class SP1002CancellationTokenNameAnalyzer : BaseClass
    {
        public override void Main(string param1, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
        }
    }

    public abstract class BaseClass
    {
        public abstract void Main(string param1, CancellationToken /*C*/token);
    }
}
