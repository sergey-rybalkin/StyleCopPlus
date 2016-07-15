using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Test.Helpers;
using TestHelper;
namespace StyleCopPlus.Test
{
    [TestClass]
    public class SP1131AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void Reports_IncorrectIfOperands()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP1131IncorrectOperands);
            DiagnosticResult expected = CreateResult(7, 17);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP1131Analyzer();
        }

        private DiagnosticResult CreateResult(int lineNumber, int column)
        {
            return new DiagnosticResult
            {
                Id = SP1131Analyzer.DiagnosticId,
                Message = SP1131Analyzer.MessageFormat,
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation("Test0.cs", lineNumber, column)
                }
            };
        }
    }
}