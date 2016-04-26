using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
            switch (node.RawKind)
            {
                case (int)SyntaxKind.ConstructorDeclaration:
                    return new ConstructorDefinitionSplitter(editor, (ConstructorDeclarationSyntax)node);
                case (int)SyntaxKind.LocalDeclarationStatement:
                    return GetSplitterForLocalDeclaration(editor, node);
                default:
                    return null;
            }
        }

        private static ILongLineSplitter GetSplitterForLocalDeclaration(
            DocumentEditor editor,
            SyntaxNode node)
        {
            var constructorCall = node.DescendantNodes()
                                      .OfType<ObjectCreationExpressionSyntax>()
                                      .FirstOrDefault();

            // If local declaration contains a call to the constructor with multiple arguments then we can
            // split it to reduce line length. 
            if (null != constructorCall && constructorCall.ArgumentList.Arguments.Count > 1)
                return new ConstructorInvocationSplitter(editor, node, constructorCall);

            return null;
        }
    }
}