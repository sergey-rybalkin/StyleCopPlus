namespace AnalyzerPlayground
{
    public class LineLengthCases
    {
        public LineLengthCases(string parameter1, string parameter2, int parameter3, long parameter4, string parameter5)
        {

        }

        public static int MethodWithVeryLongParametersList(string parameter1, string parameter2, int parameter3, long parameter4)
        {
            return 1;
        }

        public void MethodWithLongMethodInvocation()
        {
            MethodWithVeryLongParametersList("This is parameter 1 value", "This is parameter 2 value", 123132, 45234234);
        }

        public void MethodWithLongMethodInvocationAndAssignment()
        {
            int retVal = MethodWithVeryLongParametersList("This is parameter 1 value", "This is parameter 2 value", 123132, 45234234);
        }

        public void MethodWithLongConstructorInvocationAndAssignment()
        {
            LineLengthCases variable = new LineLengthCases("This is parameter 1 value", "This is parameter 2 value", 123132, 45234234, string.Empty);
        }
    }
}