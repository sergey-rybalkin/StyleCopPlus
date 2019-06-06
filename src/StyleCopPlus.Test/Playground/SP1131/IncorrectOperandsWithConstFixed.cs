namespace AnalyzerPlayground
{
    public class IncorrectOperands
    {
        public const int MyValue = 0;

        public void Test(int i)
        {
            if (i is MyValue)
               i = 1;
        }
    }
}
