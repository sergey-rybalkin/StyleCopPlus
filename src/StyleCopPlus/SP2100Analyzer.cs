using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace StyleCopPlus
{
    /// <summary>
    /// SP2100 rule analyzer - validates that code lines do not exceed configured length.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP2100Analyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP2100";

        public const string Category = "Maintainability";

        public const string Title = "Code line is too long.";

        public const string MessageFormat = "Code line should not exceed {0} characters (now {1}).";

        public const string Description = "Code line should not exceed {0} characters.";

        private static DiagnosticDescriptor _rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: string.Format(Description, Settings.SP2100MaxLineLength));

        /// <summary>
        /// Gets a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(_rule); }
        }

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context">Analysis context to register actions in.</param>
        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);
            context.RegisterSyntaxTreeAction(HandleSyntaxTree);
        }

        private static void HandleSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            SourceText text;
            if (!context.Tree.TryGetText(out text))
                return;

            int maxLength = Settings.SP2100MaxLineLength;
            foreach (TextLine line in text.Lines)
            {
                TextSpan lineSpan = line.Span;

                // Visual length can exceed lineSpan.Length if it has tab symbols. Assume that user does not
                // use tabs and there is another rule that validates this.
                if (lineSpan.Length <= maxLength)
                    continue;

                Location location = Location.Create(context.Tree, lineSpan);
                context.ReportDiagnostic(Diagnostic.Create(_rule, location, maxLength, lineSpan.Length));
            }
        }
    }
}