using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Analyzers.Test
{
    [TestClass]
    public class SP2100AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void DoesNotReport_EmptyString()
        {
            var test = string.Empty;

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Reports_LongConstructorDefinition()
        {
            var test = DataHelper.GetEmbeddedResource(DataHelper.SP2100ConstructorDefinition);

            var expected = new DiagnosticResult
            {
                Id = SP2100Analyzer.DiagnosticId,
                Message = string.Format(SP2100Analyzer.MessageFormat, Settings.SP2100MaxLineLength, 132),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 5, Settings.SP2100MaxLineLength + 1)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2100Analyzer();
        }
    }
}