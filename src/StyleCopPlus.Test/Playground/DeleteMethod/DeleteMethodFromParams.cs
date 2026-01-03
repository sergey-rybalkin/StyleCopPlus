namespace AnalyzerPlayground.DeleteMethod
{
    public class InnerMethodContainer
    {
        /// <summary>
        /// Preceeding method comments.
        /// </summary>
        /// <param name="param1">Parameter of the method.</param>
        public void PreceedingMethod(string param1)
        {
            System.Console.Write($"Method body {param1}");
        }

        /// <summary>
        /// Target method comments.
        /// </summary>
        /// <param name="param1">Parameter of the method.</param>
        public void TargetMethod(string/*C*/ param1)
        {
            System.Console.Write($"Method body {param1}");
        }
    }
}
