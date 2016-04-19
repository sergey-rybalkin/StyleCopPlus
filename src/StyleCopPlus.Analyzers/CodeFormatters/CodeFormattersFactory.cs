using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace StyleCopPlus.Analyzers.CodeFormatters
{
    /// <summary>
    /// Factory method pattern implementation for code formatters.
    /// </summary>
    internal static class CodeFormattersFactory
    {
        /// <summary>
        /// Gets line splitter for the specified syntax node.
        /// </summary>
        /// <param name="editor">Editor for the target document.</param>
        /// <param name="node">Syntax node containing long code line.</param>
        internal static ILongLineSplitter CreateLineSplitter(DocumentEditor editor, SyntaxNode node)
        {
            ConstructorDeclarationSyntax constructor = node as ConstructorDeclarationSyntax;
            if (null != constructor)
                return new ConstructorDefinitionSplitter(editor, constructor);

            return null;
        }
    }
}