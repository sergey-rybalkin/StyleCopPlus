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
    public class SP2103FileTooLongAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP2103";

        public const string Category = "Maintainability";

        public const string Title = "File is too long.";

        public const string MessageFormat = "File {0} should be no longer than {1} lines (now {2}).";

        public const string Description = "File length should not exceed {0} lines.";

        private static DiagnosticDescriptor _rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: string.Format(Description, Settings.SP2103MaxFileLength));

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
            SyntaxTree tree = context.Tree;
            SourceText text;

            if (!tree.TryGetText(out text))
                return;

            int fileLength = text.Lines.Count;

            if (fileLength <= Settings.SP2103MaxFileLength)
                return;

            // Only mark the first line past the limit.
            Location location = Location.Create(tree, text.Lines[Settings.SP2103MaxFileLength].Span);
            var diagnostic = Diagnostic.Create(
                _rule,
                location,
                tree.FilePath,
                Settings.SP2103MaxFileLength,
                fileLength);

            context.ReportDiagnostic(diagnostic);
        }
    }
}