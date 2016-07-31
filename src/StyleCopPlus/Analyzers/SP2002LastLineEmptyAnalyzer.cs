using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP2002 rule analyzer - validates that file does not end with an empty line.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP2002LastLineEmptyAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP2002";

        public const string Category = "Formatting";

        public const string Title = "Last code line should not be empty.";

        public const string MessageFormat = "Last code line should not be empty.";

        public const string Description = "Checks whether last code line is empty or not.";

        private static DiagnosticDescriptor _rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

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
            if (!context.Tree.TryGetText(out text) || text.Lines.Count < 2)
                return;

            TextLine lastLine = text.Lines[text.Lines.Count - 1];
            if (lastLine.Span.Length == 0)
            {
                Location location = Location.Create(context.Tree, lastLine.Span);
                context.ReportDiagnostic(Diagnostic.Create(_rule, location));
            }
        }
    }
}