using System;
using System.Threading;

namespace AnalyzerPlayground
{
    public class SP1002CancellationTokenNameAnalyzer : IBaseInterface
    {
        public void Main(string param1, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
        }
    }

    public interface IBaseInterface
    {
        public void Main(string param1, CancellationToken /*C*/token);
    }
}
