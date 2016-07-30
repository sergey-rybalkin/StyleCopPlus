using System.IO;

namespace AnalyzerPlayground.Refactorings
{
    public class CreateVariablePlayground
    {
        public void TestMethodCall()
        {
            // test

            var v = File.ReadAllText(@"c:\temp\test.txt");

            // test
        }
    }
}