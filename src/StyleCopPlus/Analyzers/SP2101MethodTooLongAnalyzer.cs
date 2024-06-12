using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP2100 rule analyzer - validates that code lines do not exceed configured length.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP2101MethodTooLongAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP2101";

        public const string Category = "Maintainability";

        public const string Title = "Method is too long";

        public const string MessageFormat = "Method body should not exceed {0} lines (now {1}).";

        public const string Description = "Method body should not exceed {0} lines.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: string.Format(Description, Settings.SP2101MaxMethodLengthDefault));

        private int _methodLengthLimit = Settings.SP2101MaxMethodLengthDefault;

        /// <summary>
        /// Gets a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        /// <summary>
        /// Registers analyzer actions for the specified compilation session using the specified settings.
        /// </summary>
        /// <param name="context">Analysis context to register actions in.</param>
        /// <param name="settings">Options for controlling the operation.</param>
        protected override void Register(CompilationStartAnalysisContext context, Settings settings)
        {
            _methodLengthLimit = settings.SP2101MaxMethodLength;
            context.RegisterCodeBlockAction(HandleCodeBlock);
        }

        private void HandleCodeBlock(CodeBlockAnalysisContext context)
        {
            if (!IsInsideMethod(context))
                return;

            int lines = GetNumberOfLinesInCodeBlock(context.CodeBlock);
            if (lines <= _methodLengthLimit)
                return;

            // Report diagnostic on the symbol definition that is in the same file as its long body.
            Location location =
                context.OwningSymbol.Locations.First(l => l.SourceTree == context.CodeBlock.SyntaxTree);

            Diagnostic result = Diagnostic.Create(
                Rule,
                location,
                _methodLengthLimit,
                lines);

            context.ReportDiagnostic(result);
        }
    }
}
