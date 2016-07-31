using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;
using StyleCopPlus.Test.Helpers;
using StyleCopPlus.CodeFixes;
using StyleCopPlus.Analyzers;

namespace StyleCopPlus.Test.CodeFixes
{
    [TestClass]
    public class SP2002FixProviderTests : CodeFixVerifier
    {
        [TestMethod]
        public void Fixes_ByRemovingEmptyLine()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2002EmptyLine);
            string expected = DataHelper.GetEmbeddedResource(DataHelper.SP2002EmptyLineFixed);

            VerifyCSharpFix(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2002LastLineEmptyAnalyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SP2002LastLineEmptyFixProvider();
        }
    }
}