using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Analyzers
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
            DiagnosticResult expected = CreateResult("Test0.cs", Settings.SP2103MaxFileLengthDefault + 1);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2103FileTooLongAnalyzer();
        }

        private DiagnosticResult CreateResult(string fileName, int lines)
        {
            return new DiagnosticResult
            {
                Id = SP2103FileTooLongAnalyzer.DiagnosticId,
                Message = string.Format(
                    SP2103FileTooLongAnalyzer.MessageFormat,
                    fileName,
                    Settings.SP2103MaxFileLengthDefault,
                    lines),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation("Test0.cs", lines, 0)
                }
            };
        }
    }
}
