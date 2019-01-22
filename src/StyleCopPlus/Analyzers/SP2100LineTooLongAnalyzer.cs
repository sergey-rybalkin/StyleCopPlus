using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP2100 rule analyzer - validates that code lines do not exceed configured length.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP2100LineTooLongAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP2100";

        public const string Category = "Maintainability";

        public const string Title = "Code line is too long.";

        public const string MessageFormat = "Code line should not exceed {0} characters (now {1}).";

        public const string Description = "Code line should not exceed {0} characters.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: string.Format(Description, Settings.SP2100MaxLineLengthDefault));

        private int _lineLengthLimit = Settings.SP2100MaxLineLengthDefault;

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
            _lineLengthLimit = settings.SP2100MaxLineLength;
            context.RegisterSyntaxTreeAction(HandleSyntaxTree);
        }

        private void HandleSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            SourceText text;
            if (!context.Tree.TryGetText(out text))
                return;

            foreach (TextLine line in text.Lines)
            {
                TextSpan lineSpan = line.Span;

                // Visual length can exceed lineSpan.Length if it has tab symbols. Assume that user does not
                // use tabs and there is another rule that validates this.
                if (lineSpan.Length <= _lineLengthLimit)
                    continue;

                Location location = Location.Create(context.Tree, lineSpan);
                context.ReportDiagnostic(
                    Diagnostic.Create(Rule, location, _lineLengthLimit, lineSpan.Length));
            }
        }
    }
}
