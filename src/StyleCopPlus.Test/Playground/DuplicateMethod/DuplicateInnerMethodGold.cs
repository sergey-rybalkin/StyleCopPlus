namespace AnalyzerPlayground.DuplicateMethod
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
        public void TargetMethod/*C*/(string param1)
        {
            System.Console.Write($"Method body {param1}");
        }

        /// <summary>
        /// Target method comments.
        /// </summary>
        /// <param name="param1">Parameter of the method.</param>
        public void TargetMethod(string param1)
        {
            System.Console.Write($"Method body {param1}");
        }
    }
}
