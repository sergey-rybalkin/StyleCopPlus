using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;
using StyleCopPlus.Test.Helpers;

namespace StyleCopPlus.Test
{
    [TestClass]
    public class SP1131FixProviderTests : CodeFixVerifier
    {
        [TestMethod]
        public void Fixes_BySwapingOperands()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP1131IncorrectOperands);
            string expected = DataHelper.GetEmbeddedResource(DataHelper.SP1131IncorrectOperandsFixed);

            VerifyCSharpFix(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP1131Analyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SP1131FixProvider();
        }
    }
}