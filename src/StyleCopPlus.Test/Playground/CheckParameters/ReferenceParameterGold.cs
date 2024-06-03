namespace AnalyzerPlayground.CheckParameters
{
    class ReferenceParameter
    {
        public void TestMethod(string param1)
        {
            Verify.ArgumentNotEmpty(param1);
        }
    }
}
