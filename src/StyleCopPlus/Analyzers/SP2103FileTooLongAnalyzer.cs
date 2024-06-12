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

        public const string Title = "File is too long";

        public const string MessageFormat = "File {0} should be no longer than {1} lines (now {2}).";

        public const string Description = "File length should not exceed {0} lines.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: string.Format(Description, Settings.SP2103MaxFileLengthDefault));

        private int _fileLengthLimit = Settings.SP2103MaxFileLengthDefault;

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
            _fileLengthLimit = settings.SP2103MaxFileLength;
            context.RegisterSyntaxTreeAction(HandleSyntaxTree);
        }

        private void HandleSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            SyntaxTree tree = context.Tree;
            SourceText text;

            if (!tree.TryGetText(out text))
                return;

            int fileLength = text.Lines.Count;

            if (fileLength <= _fileLengthLimit)
                return;

            // Only mark the first line past the limit.
            Location location = Location.Create(tree, text.Lines[_fileLengthLimit].Span);
            var diagnostic = Diagnostic.Create(
                Rule,
                location,
                tree.FilePath,
                Settings.SP2103MaxFileLengthDefault,
                fileLength);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
