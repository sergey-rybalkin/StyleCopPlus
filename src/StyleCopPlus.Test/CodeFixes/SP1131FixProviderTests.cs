using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;
using StyleCopPlus.Test.Helpers;
using StyleCopPlus.Analyzers;
using StyleCopPlus.CodeFixes;

namespace StyleCopPlus.Test.CodeFixes
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
            return new SP1131UnsafeConditionAnalyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SP1131UnsafeConditionFixProvider();
        }
    }
}