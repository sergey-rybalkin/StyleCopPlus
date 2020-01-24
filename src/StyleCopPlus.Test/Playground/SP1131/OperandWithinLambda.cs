using System.Linq;

namespace AnalyzerPlayground.SP1131
{
    public class OperandWithinLambda
    {
        public void Test()
        {
            string[] data = new string[0];
            data.Select(s => /*C*/s == null);
        }
    }
}
