namespace AnalyzerPlayground.CheckParameters
{
    class ConstructorParameter
    {
        public ConstructorParameter(string /*C*/param1)
        {
            Verify.ArgumentNotEmpty(param1, nameof(param1));
        }
    }
}