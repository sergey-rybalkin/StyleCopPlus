using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;
using StyleCopPlus.Analyzers.Test.Helpers;

namespace StyleCopPlus.Analyzers.Test
{
    [TestClass]
    public class SP2100FixProviderTests : CodeFixVerifier
    {
        [TestMethod]
        public void Fixes_LongConstructorDefinition()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorDefinition);
            string expected = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorDefinitionFixed);

            VerifyCSharpFix(test, test);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2100Analyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new SP2100FixProvider();
        }
    }
}