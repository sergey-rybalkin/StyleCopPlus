using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers.Test.Helpers;
using TestHelper;
namespace StyleCopPlus.Analyzers.Test
{
    [TestClass]
    public class SP2002AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void DoesNotReport_CorrectFile()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2002NonEmptyLine);

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Reports_LastEmptyLineInFile()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2002EmptyLine);
            DiagnosticResult expected = CreateResult(7);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2002Analyzer();
        }

        private DiagnosticResult CreateResult(int lineNumber)
        {
            return new DiagnosticResult
            {
                Id = SP2002Analyzer.DiagnosticId,
                Message = SP2002Analyzer.MessageFormat,
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation("Test0.cs", lineNumber, 0)
                }
            };
        }
    }
}