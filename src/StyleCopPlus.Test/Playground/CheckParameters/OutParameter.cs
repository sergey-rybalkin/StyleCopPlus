namespace AnalyzerPlayground.CheckParameters
{
    class OutParameter
    {
        public void TestMethod(out string /*C*/param1)
        {
            param1 = string.Empty;
        }
    }
}