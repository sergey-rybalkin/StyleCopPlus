using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// Base class for all analyzers.
    /// </summary>
    public abstract class StyleCopPlusAnalyzer : DiagnosticAnalyzer
    {
        // Name of the StyleCop settings file that we inject our configuration into.
        private const string SettingsFileName = "stylecop.json";

        private static readonly SyntaxKind[] MethodSyntaxKinds = new SyntaxKind[]
        {
            SyntaxKind.MethodDeclaration,
            SyntaxKind.ConstructorDeclaration,
            SyntaxKind.DestructorDeclaration,
            SyntaxKind.ConversionOperatorDeclaration,
            SyntaxKind.OperatorDeclaration
        };

        // Performance hack. All values in MethodSyntaxKinds are declared next to each other, so we don't have
        // to compare target value to each item in the array - range comparison is enough.
        private static readonly ushort MethodSyntaxLowerBound = (ushort)MethodSyntaxKinds.Min();
        private static readonly ushort MethodSyntaxUpperBound = (ushort)MethodSyntaxKinds.Max();

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context">Analysis context to register actions in.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(ctx => {
                Settings settings = ParseAnalyzerSettings(ctx.Options.AdditionalFiles);
                Register(ctx, settings);
            });
        }

        /// <summary>
        /// Registers analyzer actions for the specified compilation session using the specified settings.
        /// </summary>
        /// <param name="context">Analysis context to register actions in.</param>
        /// <param name="settings">Options for controlling the operation.</param>
        protected abstract void Register(CompilationStartAnalysisContext context, Settings settings);

        /// <summary>
        /// Checks whether specified code block is inside a method-like declaration.
        /// </summary>
        /// <param name="context">Analysis context for the block to check.</param>
        protected static bool IsInsideMethod(CodeBlockAnalysisContext context)
        {
            if (SymbolKind.Method != context.OwningSymbol.Kind)
                return false;

            SyntaxNode block = context.CodeBlock;
            ushort blockKind = (ushort)block.RawKind;

            return blockKind >= MethodSyntaxLowerBound && blockKind <= MethodSyntaxUpperBound;
        }

        /// <summary>
        /// Checks whether specified code block is inside property getter or setter.
        /// </summary>
        /// <param name="context">Analysis context for the block to check.</param>
        protected static bool IsInsideProperty(CodeBlockAnalysisContext context)
        {
            if (SymbolKind.Method != context.OwningSymbol.Kind)
                return false;

            SyntaxNode block = context.CodeBlock;
            ushort blockKind = (ushort)block.RawKind;

            return (ushort)SyntaxKind.GetAccessorDeclaration == blockKind ||
                   (ushort)SyntaxKind.SetAccessorDeclaration == blockKind;
        }

        protected static int GetNumberOfLinesInCodeBlock(SyntaxNode codeBlock)
        {
            SyntaxTree syntax = codeBlock.SyntaxTree;
            SourceText treeText;
            if (null == syntax || !syntax.TryGetText(out treeText))
                return 0;

            SourceText blockText = treeText.GetSubText(codeBlock.Span);

            return blockText.Lines.Count;
        }

        private static Settings ParseAnalyzerSettings(ImmutableArray<AdditionalText> additionalFiles)
        {
            AdditionalText settingsFile = additionalFiles.FirstOrDefault(
                f => Path.GetFileName(f.Path)
                         .Equals(SettingsFileName, StringComparison.Ordinal));

            return null == settingsFile ? Settings.GetDefault() :
                                          Settings.Parse(settingsFile.GetText().ToString());
        }
    }
}
