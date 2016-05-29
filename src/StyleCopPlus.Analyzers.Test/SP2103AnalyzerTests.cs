using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Analyzers.Test
{
    [TestClass]
    public class SP2103AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void DoesNotReport_SmallFiles()
        {
            var test = DataHelper.GetEmbeddedResource(DataHelper.SP2103ValidClass);
            
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Reports_LongFiles()
        {
            var test = DataHelper.GetEmbeddedResource(DataHelper.SP2103LongClass);
            DiagnosticResult expected = CreateResult("Test0.cs", 401);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2103Analyzer();
        }

        private DiagnosticResult CreateResult(string fileName, int lines)
        {
            return new DiagnosticResult
            {
                Id = SP2103Analyzer.DiagnosticId,
                Message = string.Format(
                    SP2103Analyzer.MessageFormat,
                    fileName,
                    Settings.SP2103MaxFileLength,
                    lines),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation("Test0.cs", Settings.SP2103MaxFileLength + 1, 0)
                }
            };
        }
    }
}