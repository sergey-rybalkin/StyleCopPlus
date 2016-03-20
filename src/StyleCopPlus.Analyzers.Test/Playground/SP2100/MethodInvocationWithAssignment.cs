namespace AnalyzerPlayground
{
    public class SP2100MethodInvocationWithAssignment
    {
        public static int MethodWithVeryLongParametersList(
            string parameter1,
            string parameter2,
            int parameter3,
            long parameter4)
        {
            return 1;
        }

        public void Target()
        {
            int retVal = MethodWithVeryLongParametersList("This is parameter 1 value", "This is parameter 2 value", 123132, 45234234);
        }
    }
}