namespace AnalyzerPlayground
{
    public class IncorrectOperands
    {
        public const int MyValue = 0;

        public void Test(int i)
        {
            if (/*C*/i == MyValue)
               i = 1;
        }
    }
}
