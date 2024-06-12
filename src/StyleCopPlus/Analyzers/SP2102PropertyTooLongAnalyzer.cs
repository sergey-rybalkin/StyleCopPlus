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
    public class SP2102PropertyTooLongAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP2102";

        public const string Category = "Maintainability";

        public const string Title = "Property accessor is too long";

        public const string MessageFormat = "Property accessor body should not exceed {0} lines (now {1}).";

        public const string Description = "Property accessor body should not exceed {0} lines.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: string.Format(Description, Settings.SP2102MaxPropertyAccessorLengthDefault));

        private int _memberLengthLimit = Settings.SP2102MaxPropertyAccessorLengthDefault;

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
            _memberLengthLimit = settings.SP2102MaxPropertyAccessorLength;
            context.RegisterCodeBlockAction(HandleCodeBlock);
        }

        private void HandleCodeBlock(CodeBlockAnalysisContext context)
        {
            if (!IsInsideProperty(context))
                return;

            int lines = GetNumberOfLinesInCodeBlock(context.CodeBlock);
            if (lines <= _memberLengthLimit)
                return;

            // Report diagnostic on the symbol definition that is in the same file as its long body.
            Location location =
                context.OwningSymbol.Locations.First(l => l.SourceTree == context.CodeBlock.SyntaxTree);

            Diagnostic result = Diagnostic.Create(
                Rule,
                location,
                _memberLengthLimit,
                lines);

            context.ReportDiagnostic(result);
        }
    }
}
