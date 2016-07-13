namespace AnalyzerPlayground
{
    public class SP2100ConstructorInvocation
    {
        public SP2100ConstructorInvocation(
            string parameter1,
            string parameter2,
            int parameter3,
            long parameter4,
            string parameter5)
        {

        }

        public void MethodWithLongConstructorInvocationAndAssignment()
        {
            SP2100ConstructorInvocation variable = new SP2100ConstructorInvocation(
                "This is parameter 1 value",
                "This is parameter 2 value",
                123132,
                45234234,
                string.Empty);
        }
    }
}