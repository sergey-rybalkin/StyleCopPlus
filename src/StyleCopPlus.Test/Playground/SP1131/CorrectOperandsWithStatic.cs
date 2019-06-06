namespace AnalyzerPlayground
{
    public class CorrectOperands
    {
        public static readonly int MyValue = 0;

        public void Test(int i)
        {
            if (/*C*/i == MyValue)
               i = 1;
        }
    }
}
