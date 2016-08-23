namespace AnalyzerPlayground.CheckParameters
{
    class ReferenceParameter
    {
        public void TestMethod(string param1)
        {
            Verify.ArgumentNotNull(param1, nameof(param1));
        }
    }
}