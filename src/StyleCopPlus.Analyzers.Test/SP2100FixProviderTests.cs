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
        public void Fixes_ConstructorDefinition()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorDefinition);
            string expected = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorDefinitionFixed);

            VerifyCSharpFix(test, expected);
        }

        [TestMethod]
        public void Fixes_ConstructorInvocation()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorInvocation);
            string expected = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorInvocationFixed);

            VerifyCSharpFix(test, expected);
        }

        [TestMethod]
        public void Fixes_MethodDefinition()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100MethodDefinition);
            string expected = DataHelper.GetEmbeddedResource(DataHelper.SP2100MethodDefinitionFixed);

            VerifyCSharpFix(test, expected);
        }

        [TestMethod]
        public void Fixes_MethodInvocation()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100MethodInvocation);
            string expected = DataHelper.GetEmbeddedResource(DataHelper.SP2100MethodInvocationFixed);

            VerifyCSharpFix(test, expected);
        }

        [TestMethod]
        public void Fixes_MethodInvocationWithAssignment()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2100MethodInvocationWithAssignment);
            string expected = DataHelper.GetEmbeddedResource(
                DataHelper.SP2100MethodInvocationWithAssignmentFixed);

            VerifyCSharpFix(test, expected);
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